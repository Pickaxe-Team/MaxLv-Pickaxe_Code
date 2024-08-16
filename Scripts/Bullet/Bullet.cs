using System.Collections;
using UnityEngine;

public class Bullet : PoolObject
{
    private GameManager GM => GameManager.Instance;
    private PickaxeData NowPickaxeData => GameManager.Instance.GetPickaxeData((PickaxeID)GameManager.Instance.NowPlayerData.PickaxeLV);

    private Rigidbody2D _rigidbody;
    private string _bulletOwnerTag;
    private float _playerATK;

    private Vector3 _initialScale;
    private Vector3 _targetScale;
    private SpriteRenderer _spriteRenderer;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerATK = NowPickaxeData.ATK / 2;

        _initialScale = transform.localScale;
        _targetScale = _initialScale * NowPickaxeData.PickaxeAuraSize;
    }

    private void OnAfterInitialize()
    {
        transform.localScale = _initialScale;
        if (_bulletOwnerTag == Tag.Player)
        {
            StartCoroutine(DeactivateAfterDelay(NowPickaxeData.PickaxeAuraRange));
            ChangePlayerAuraImage();
            StartCoroutine(GrowBulletOverTime(0.5f));
        }      
        StartCoroutine(DeactivateAfterDelay(3f));     
    }
    private IEnumerator GrowBulletOverTime(float duration)
    {
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            transform.localScale = Vector3.Lerp(_initialScale, _targetScale, time / duration);
            yield return null;
        }
    }
    private IEnumerator DeactivateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }

    public void Initialize(Vector2 position, Vector2 direction, string ownerTag)
    {
        transform.position = position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        _bulletOwnerTag = ownerTag; // 불렛 소유자의 태그 설정

        if (_bulletOwnerTag == Tag.Player)
        {
            _rigidbody.velocity = direction.normalized * NowPickaxeData.PickaxeAuraSpeed;
        }
        else if (_bulletOwnerTag == Tag.Boss)
        {
            _rigidbody.velocity = direction.normalized * 10f;
        }
        OnAfterInitialize();
    }

    #region 불렛 충돌
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_bulletOwnerTag == Tag.Player)
        {
            HandlePlayerBulletCollision(collision);
        }
        else if (_bulletOwnerTag == Tag.Boss && collision.CompareTag(Tag.Player))
        {
            HandleBossBulletCollision(collision);
        }
    }

    private void HandlePlayerBulletCollision(Collider2D collision)
    {
        if ((collision.gameObject.layer == LayerMask.NameToLayer(Layer.Terrain)))   
        {
            gameObject.SetActive(false);         
        }     
        if (collision.CompareTag(Tag.Boss))
        {
            HandleBossCollision(collision);
            CanPenetration();
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer(Layer.Enemy))
        {
            HandleEnemyCollision(collision);
            CanPenetration();
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer(Layer.InteractableObject))
        {
            HandleInteractableObjectCollision(collision);
            CanPenetration();
        }
    }
    private void CanPenetration()
    {
        if (!NowPickaxeData.CanPenetrationPickaxeAura)
        {
            gameObject.SetActive(false);
        }
    }
    private void HandleBossBulletCollision(Collider2D collision)
    {
        GM.Player.TakeDamage(GM.Player.MaxHP * 0.1f);
        gameObject.SetActive(false);
    }
    #endregion

    #region 충돌매서드
    private void HandleBossCollision(Collider2D collision)
    {
        Boss boss = collision.GetComponent<Boss>();
        if (boss != null)
        {
            boss.TakeDamage(_playerATK);
        }
    }

    private void HandleEnemyCollision(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(_playerATK, Vector2.zero);
        }
    }

    private void HandleInteractableObjectCollision(Collider2D collision)
    {
        InteractableObject interactableObject = collision.gameObject.GetComponent<InteractableObject>();
        if (interactableObject != null)
        {
            interactableObject.TakeAtk((int)NowPickaxeData.ID, _playerATK);
        }
    }
    public void ChangePlayerAuraImage()
    {
        _spriteRenderer.sprite = NowPickaxeData.PickaxeAuraImage;
    }
    #endregion
}
