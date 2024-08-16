using System;
using System.Collections;
using UnityEngine;
public class Boss : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;

    [Header("Data")]
    [SerializeField] private EntityID EntityID;
    public EnemyData EnemyData;

    public HealthSystem HealthSystem;
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private GameObject ClearPopUp;
    public float BossCurHP => HealthSystem.CurHP;
    public event Action OnChangeBossHP;

    private Animator _animator;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        EnemyData = GameManager.Instance.GetEntityData(EntityID) as EnemyData;
        HealthSystem = GetComponent<HealthSystem>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (HealthSystem != null)
        {
            HealthSystem.InitHP(EnemyData.HP, EnemyData.HP);
            HealthSystem.OnDeath += OnDie;
            HealthSystem.OnChangeHP += HandleChangeHP;
        }
    }
    private void HandleChangeHP()
    {
        OnChangeBossHP?.Invoke();
    }
    public void TakeDamage(float damage)
    {
        if (HealthSystem != null)
        {
            HealthSystem.TakeDamage(damage);
            StartCoroutine(TakeDamgeCoroutine());
        }
    }
    private IEnumerator TakeDamgeCoroutine()
    {
        float timer = 0f;
        while (timer < 0.1f)
        {
            _spriteRenderer.color = Color.red;
            timer += Time.deltaTime;
            yield return null;
        }
        _spriteRenderer.color = Color.white;
    }

    private void OnDie()
    {
        GM.Player.GetInvincible();
        Player player = GameManager.Instance.Player;

        if (player != null)
        {
            player.AddExperience(EnemyData.EXP);
        }

        GetComponent<Collider2D>().enabled = false;

        GM.NowUserData.ClearedBosses.Add(EntityID);
        _animator.SetTrigger("Die");
    }

    public void Heal(float amount)
    {
        if (HealthSystem != null)
        {
            HealthSystem.Heal(amount);
        }
    }

    private void ShowClearPopup()
    {
        ClearPopUp.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tag.Player))
        {
            GM.Player.TakeDamage(GM.Player.MaxHP * 0.1f);
        }
    }
}
