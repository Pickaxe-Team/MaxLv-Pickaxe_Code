using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [field: Header("Animations")]
    [field: SerializeField] public PlayerAnimationData AnimationData { get; private set; }

    public Animator Animator { get; private set; }
    public PlayerController Input { get; private set; }
    [SerializeField] private PickAxeController PickAxeController; // PickAxeController 추가
    private PlayerStateMachine _stateMachine;
    private SpriteRenderer _spriteRenderer;
    public HealthSystem HealthSystem;
    private BoxCollider2D _boxCollider2D;
    public GameObject LevelUpEffect;
    public GameObject HealEffect;
    public float MaxHP => 21.05f * NowPlayerData.LV + 78.95f; // 피통
    public float MaxEXP => 50f * (NowPlayerData.LV + 1); // 경험치통

    public event Action OnChangePlayerHP;
    public event Action OnChangeEXP;
    public event Action OnChangeLV;

    private GameManager GM => GameManager.Instance;
    private PlayerData NowPlayerData => GameManager.Instance.NowPlayerData;
    public PlayerSO PlayerSO => GameManager.Instance.GetPlayerData();

    [Header("Invincibility Settings")]
    private bool _isInvincible = false;

    [field: Header("Bullet")]
    public GameObject BulletPrefab;
    public Transform FirePoint;
    public ObjectPool ObjectPool;

    private float _healCooldown = 5.0f;
    private float _healCooldownTimer = 0f;
    public Image HealCoolDownImg;

    void Awake()
    {
        AnimationData.Initialize();
        Animator = GetComponent<Animator>();
        Input = GetComponent<PlayerController>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _stateMachine = new PlayerStateMachine(this);
        HealthSystem = GetComponent<HealthSystem>();
        _boxCollider2D = GetComponent<BoxCollider2D>();

        if (HealthSystem != null)
        {
            HealthSystem.InitHP(NowPlayerData.CurHP, MaxHP);
            HealthSystem.OnChangeHP += ApplyHP;
            HealthSystem.OnDeath += OnDie;
        }
    }

    private void Start()
    {
        _stateMachine.ChangeState(_stateMachine.IdleState);
        _boxCollider2D.enabled = true;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(Layer.Player), LayerMask.NameToLayer(Layer.Enemy), false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(Layer.Player), LayerMask.NameToLayer(Layer.BossBullet), false);

        LevelUpEffect.SetActive(false);
        HealEffect.SetActive(false);
    }

    private void Update()
    {
        _stateMachine.HandleInput();
        _stateMachine.Update();

        _healCooldownTimer -= Time.deltaTime;
        HealCoolDownImg.fillAmount -= Time.deltaTime / _healCooldown;
    }

    private void FixedUpdate()
    {
        _stateMachine.PhysicsUpdate();
    }

    private void ApplyHP()
    {
        NowPlayerData.CurHP = HealthSystem.CurHP;

        OnChangePlayerHP?.Invoke();
    }

    public bool Heal(float amount)
    {
        if(HealthSystem.IsDead) return false;

        if (_healCooldownTimer <= 0f)
        {
            if (HealthSystem.CurHP == HealthSystem.MaxHP)
            {
                // 플레이어 체력이 풀인 경우
                GM.ShowAlert("조금 더 먹었다간 배가 터질지도 몰라요!");
                return false;
            }

            _healCooldownTimer = _healCooldown;
            HealCoolDownImg.fillAmount = _healCooldown;

            HealthSystem.Heal(amount);
            HealEffect.SetActive(true);
            GM.PlaySFX(SFX.Heal);
            return true;
        }
        else
        {
            GM.ShowAlert("재사용 대기 시간입니다!");
        }
        return false;
    }

    private void OnDie()
    {
        NowPlayerData.CurHP = HealthSystem.CurHP;

        Animator.SetTrigger("Die");
        _boxCollider2D.enabled = false;
        Input.enabled = false;
        StartCoroutine(ActiveGameOverPopup());
    }

    private IEnumerator ActiveGameOverPopup()
    {
        yield return new WaitForSeconds(PlayerSO.InvincibilityDuration);
        GameManager.Instance.GameOverPopup.SetActive(true);
        Time.timeScale = 0;
    }

    public void FlipPickAxe(Vector2 moveInput)
    {
        if (PickAxeController != null)
        {
            PickAxeController.SetFlipDirection(moveInput);
        }
    }

    public void PlayPickAxeAttack()
    {
        if (PickAxeController != null)
        {
            PickAxeController.PlayAttackAnimation();
        }
    }

    public void ResetPickAxeAttack()
    {
        if (PickAxeController != null)
        {
            PickAxeController.ResetAttackAnimation();
        }
    }

    public void TakeDamage(float damage)
    {
        if (_isInvincible) return;

        if (HealthSystem != null)
        {
            HealthSystem.TakeDamage(damage);
            GameManager.Instance.PlaySFX(SFX.PlayerHit);
            StartCoroutine(BecomeInvincible());
        }
    }

    private IEnumerator BecomeInvincible()
    {
        _isInvincible = true;

        if (_spriteRenderer != null)
        {
            Color originalColor = _spriteRenderer.color;
            Color invincibleColor = Color.red;
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(Layer.Player), LayerMask.NameToLayer(Layer.Enemy), true);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(Layer.Player), LayerMask.NameToLayer(Layer.BossBullet), true);
            _spriteRenderer.color = invincibleColor;
            yield return new WaitForSeconds(PlayerSO.InvincibilityDuration / 2);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(Layer.Player), LayerMask.NameToLayer(Layer.Enemy), false);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(Layer.Player), LayerMask.NameToLayer(Layer.BossBullet), false);
            _spriteRenderer.color = originalColor;
        }

        _isInvincible = false;
    }

    public void AddExperience(float amount)
    {
        NowPlayerData.CurEXP += amount;

        while (NowPlayerData.CurEXP >= MaxEXP)
        {
            NowPlayerData.CurEXP -= MaxEXP;
            LevelUp();
        }

        OnChangeEXP?.Invoke();
    }

    // 레벨업 메서드
    private void LevelUp()
    {
        // 레벨 업
        NowPlayerData.LV++;
        // 플레이어 체력 최대로 회복
        NowPlayerData.CurHP = MaxHP;
        // 헬스 시스템 HP 초기화
        HealthSystem.InitHP(NowPlayerData.CurHP, MaxHP);

        GM.PlaySFX(SFX.LevelUp);

        if(LevelUpEffect.activeInHierarchy)
        {
            LevelUpEffect.SetActive(false);
        }
        LevelUpEffect.SetActive(true);

        OnChangeLV?.Invoke();
    }
    public void GetInvincible()
    {
        _boxCollider2D.enabled = false;
    }
}
