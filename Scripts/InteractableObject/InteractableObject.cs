using DG.Tweening;
using System.Collections;
using UnityEngine;
 
public class InteractableObject : PoolObject, IItemDropperable, IObjectPool
{
    private MapData MapData => GameManager.Instance.NowMapData;

    [Header("Data")]
    [SerializeField] private EntityID EntityID;
    public InteractableObjectData InteractableObjectData;

    [Header("Hp UI")]
    private HealthSystem _healthSystem;
    [SerializeField] private HealthStatusUI HealthStatusUI;

    public int Index;
    private SpriteRenderer _spriteRenderer;
    private bool _isTweening = false;

    public ObjectPool ObjectPool;

    public void Awake()
    {
        InteractableObjectData = GameManager.Instance.GetEntityData(EntityID) as InteractableObjectData;

        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _healthSystem = GetComponent<HealthSystem>();

        if (_healthSystem != null)
        {
            _healthSystem.InitHP(InteractableObjectData.HP, InteractableObjectData.HP);
            _healthSystem.OnDeath += DestroyObject;
            HealthStatusUI.HealthSystem = _healthSystem;
        }

        HealthStatusUI.UpdateHPStatus();
        HealthStatusUI.SetStatusEvent();
        HealthStatusUI.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _healthSystem.InitHP(InteractableObjectData.HP, InteractableObjectData.HP);
        HealthStatusUI.UpdateHPStatus();
        HealthStatusUI.gameObject.SetActive(false);

        _spriteRenderer.color = Color.white;
        _spriteRenderer.transform.localScale = Vector3.one;
    }

    public void TakeAtk(int equipLv, float damage)
    {
        //곡괭이의 레벨과 자원 레벨을 비교
        //곡괭이보다 자원 레벨이 높으면 자원 공격 못함
        if (InteractableObjectData.LimitLV > equipLv)
        {
            GameManager.Instance.ShowAlert("곡괭이 레벨을 올리세요!");
        }
        else
        {
            _healthSystem.TakeDamage(damage);

            switch (InteractableObjectData.Type)
            {
                case InteractableObjectType.Tree:
                    GameManager.Instance.PlaySFX(SFX.WoodHit);
                    break;

                case InteractableObjectType.Rock:
                    GameManager.Instance.PlaySFX(SFX.RockHit);
                    break;

                case InteractableObjectType.Ore:
                    GameManager.Instance.PlaySFX(SFX.RockHit);
                    break;
            }

            if (!HealthStatusUI.gameObject.activeInHierarchy)
            {
                HealthStatusUI.gameObject.SetActive(true);
            }

            if (!_healthSystem.IsDead && !_isTweening)
            {
                _isTweening = true;
                _spriteRenderer.transform.DOPunchScale(new Vector3(0.3f,0.3f,0), 0.5f, 10, 1).OnComplete(() =>
                {
                    _isTweening = false;
                });
                StartCoroutine(ChangeInteractableObjColor());
            }
            
        }
    }

    private void DestroyObject()
    {
        GameManager.Instance.Player.AddExperience(InteractableObjectData.EXP);

        gameObject.SetActive(false);

        MapData.InteractableObjects.Remove(Index);

        switch (InteractableObjectData.Type)
        {
            case InteractableObjectType.Tree:
                ForestManager.Instance.ForestSpawner.CurTreeAmount--;
                break;

            case InteractableObjectType.Rock:
                ForestManager.Instance.ForestSpawner.CurRockAmount--;
                break;

            case InteractableObjectType.Ore:
                DungeonManager.Instance.NowDungeon.CurOreAmount--;
                break;
        }
    }

    private IEnumerator ChangeInteractableObjColor()
    {
        Color hitColor = new Color(1f, 1f, 1f, 0.5f); //예시임 원하는 색으로 바꾸면됨
        _spriteRenderer.color = hitColor;
        yield return new WaitForSeconds(0.2f); // 예시임 원하는 시간초로 바꾸면됨
        _spriteRenderer.color = Color.white;
    }

    public EntityData ReturnMyEntity()
    {
        return InteractableObjectData;
    }

    public ObjectPool ReturnMyObjectPool()
    {
        return ObjectPool;
    }
}