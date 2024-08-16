using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : PoolObject, IItemDropperable, IObjectPool
{
    [Header("Data")]
    [SerializeField] private EntityID EntityID;
    public EnemyData EnemyData;

    [field: Header("Animations")]
    [field: SerializeField] public PlayerAnimationData AnimationData { get; private set; }

    [Header("Knockback Settings")]
    private float _knockbackDistance = 2f;
    private float _knockbackDuration = 0.5f;
    private float _stunDuration = 0.6f;
    private bool _isStunned = false;
    private bool _isChangedColor = false;
    private Tween _nowTween;
    private LayerMask _layerMask;
    private float _rayDistance = 0.8f;

    public float PlayerChasingRange;

    public Animator Animator { get; private set; }
    private EnemyStateMachine _stateMachine;
    private HealthSystem _healthSystem;
    [SerializeField] private HealthStatusUI HealthStatusUI;
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    public NavMeshAgent Agent;
    public ObjectPool ObjectPool;

    private void Awake()
    {
        EnemyData = GameManager.Instance.GetEntityData(EntityID) as EnemyData;

        AnimationData.Initialize();
        Animator = GetComponentInChildren<Animator>();
        _stateMachine = new EnemyStateMachine(this);
        _healthSystem = GetComponent<HealthSystem>();
        _rigidbody = GetComponent<Rigidbody2D>();
        Agent = GetComponent<NavMeshAgent>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (_healthSystem != null)
        {
            _healthSystem.InitHP(EnemyData.HP, EnemyData.HP);
            _healthSystem.OnDeath += OnDie;
            HealthStatusUI.HealthSystem = _healthSystem;
        }

        HealthStatusUI.UpdateHPStatus();
        HealthStatusUI.SetStatusEvent();
        HealthStatusUI.gameObject.SetActive(false);

        Agent.speed = EnemyData.Speed;
        _layerMask = LayerMask.GetMask(Layer.Terrain) | LayerMask.GetMask(Layer.InteractableObject);
    }

    private void OnEnable()
    {
        _healthSystem.InitHP(EnemyData.HP, EnemyData.HP);
        HealthStatusUI.UpdateHPStatus();
        HealthStatusUI.gameObject.SetActive(false);

        Agent.updateRotation = false;
        Agent.updateUpAxis = false;

        PlayerChasingRange = EnemyData.PlayerChasingRange;

        if(_isStunned == true)
        {
            Animator.speed = 1;
            Agent.isStopped = false;
            _isStunned = false;
        }
        if(_isChangedColor == true)
        {
            _spriteRenderer.color = Color.white;
            _isChangedColor = false;
        }
    }

    private void Start()
    {
        if (_stateMachine != null)
        {
            _stateMachine.ChangeState(_stateMachine.IdleState);
        }
    }

    private void Update()
    {
        if (_stateMachine != null)
        {
            _stateMachine.HandleInput();
            _stateMachine.Update();
        }
    }

    private void FixedUpdate()
    {
        if (_stateMachine != null)
        {
            _stateMachine.PhysicsUpdate();
        }
    }

    public void TakeDamage(float damage, Vector2 knockbackDirection)
    {
        if (_healthSystem != null)
        {
            _healthSystem.TakeDamage(damage);
            GameManager.Instance.PlaySFX(SFX.EnemyHit);

            if (!HealthStatusUI.gameObject.activeInHierarchy)
            {
                HealthStatusUI.gameObject.SetActive(true);
            }

            if(!_healthSystem.IsDead && !_isChangedColor)
            {
                if (!_isStunned && knockbackDirection != Vector2.zero)
                {
                    StartCoroutine(ApplyKnockback(knockbackDirection));
                }
                
                StartCoroutine(ChangeColor());
            }

            PlayerChasingRange = EnemyData.PlayerChasingRange * 2;
        }
    }

    private IEnumerator ApplyKnockback(Vector2 direction)
    {
        _isStunned = true;
        Agent.isStopped = true;
        Animator.speed = 0;
        _nowTween = transform.DOMove((Vector2)transform.position + direction * _knockbackDistance, _knockbackDuration).SetEase(Ease.OutQuad);
        _nowTween.OnUpdate(() => KnockbackRaycast(direction));

        yield return new WaitForSeconds(_stunDuration);

        Animator.speed = 1;
        Agent.isStopped = false;
        _isStunned = false;
    }

    private IEnumerator ChangeColor()
    {
        _isChangedColor = true;
        _spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(_stunDuration);

        _spriteRenderer.color = Color.white;
        _isChangedColor = false;
    }

    private void KnockbackRaycast(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, _rayDistance, _layerMask);
        if (hit.collider != null)
        {
            _nowTween.Kill();
        }
    }

    private void OnDie()
    {
        DungeonManager.Instance.NowDungeon.CurEnemyAmount--;

        Player player = GameManager.Instance.Player;

        if (player != null)
        {
            player.AddExperience(EnemyData.EXP);
        }
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Tag.Player))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(EnemyData.ATK);
            }
        }
    }

    public EntityData ReturnMyEntity()
    {
        return EnemyData;
    }

    public ObjectPool ReturnMyObjectPool()
    {
        return ObjectPool;
    }
}
