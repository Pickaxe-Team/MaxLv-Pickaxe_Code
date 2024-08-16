using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public UnityEvent<Vector2> OnMoveInput;
    public UnityEvent OnRollInput;
    public UnityEvent OnConsumableInput;
    public UnityEvent<Vector2> OnAttackInput;

    private PlayerInputs _playerInputActions;
    private InputAction _moveAction;
    private InputAction _rollAction;
    private InputAction _attackAction;
    private InputAction _consumableAction;
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    public PlayerInputs PlayerInputActions => _playerInputActions;

    private float _moveSpeed;
    private Player _player;
    private Ghost _ghost;

    private bool _isAttacking = false;
    private float _attackCooldown = 0.5f;
    private float _attackCooldownTimer = 0f;

    private float _rollCooldown = 1.0f;
    private float _rollCooldownTimer = 0f;

    public Image CoolDownImg;

    private Vector2 _lastAttackInput;

    private void Awake()
    {
        _playerInputActions = new PlayerInputs();
        _playerInputActions.Enable();

        _moveAction = _playerInputActions.Player.Move;
        _rollAction = _playerInputActions.Player.Roll;
        _attackAction = _playerInputActions.Player.Attack;
        _consumableAction = _playerInputActions.Player.Consumable;

        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _player = GetComponent<Player>();
        _ghost = GetComponent<Ghost>();

        _moveAction.performed += context =>
        {
            Vector2 input = context.ReadValue<Vector2>();
            OnMoveInput.Invoke(input);
            if (!_isAttacking)
            {
                FlipSprite(input);
            }
        };
        _moveAction.canceled += context =>
        {
            OnMoveInput.Invoke(Vector2.zero);
        };

        _rollAction.performed += context =>
        {
            if (_rollCooldownTimer <= 0f)
            {
                OnRollInput.Invoke();
                _rollCooldownTimer = _rollCooldown;
                CoolDownImg.fillAmount = _rollCooldown;
            }
        };
        _consumableAction.performed += context =>
        {
            OnConsumableInput.Invoke();
        };

        _attackAction.performed += context =>
        {
            Vector2 input = context.ReadValue<Vector2>();
            FlipSprite(input);
            if (input.magnitude > 0.1f)
            {
                _lastAttackInput = input;
                _isAttacking = true;
            }
        };
        _attackAction.canceled += context =>
        {
            _isAttacking = false;
        };

        _moveSpeed = _player.PlayerSO.BaseSpeed;
    }

    private void OnEnable()
    {
        _playerInputActions.Enable();
    }

    private void OnDisable()
    {
        _playerInputActions.Disable();
    }

    private void Update()
    {
        if (_rollCooldownTimer > 0)
        {
            _rollCooldownTimer -= Time.deltaTime;
            CoolDownImg.fillAmount -= Time.deltaTime / _rollCooldown;
        }

        if (_attackCooldownTimer > 0)
        {
            _attackCooldownTimer -= Time.deltaTime;
        }

        if (_isAttacking && _attackCooldownTimer <= 0f)
        {
            OnAttackInput.Invoke(_lastAttackInput);
            _attackCooldownTimer = _attackCooldown;
        }
    }

    private void FlipSprite(Vector2 moveInput)
    {
        if (moveInput.x > 0)
        {
            _spriteRenderer.flipX = false;
        }
        else if (moveInput.x < 0)
        {
            _spriteRenderer.flipX = true;
        }

        _player.FlipPickAxe(moveInput);
    }

    public void OnRollButtonPressed()
    {
        if (_rollCooldownTimer <= 0f)
        {
            OnRollInput.Invoke();
            _rollCooldownTimer = _rollCooldown;
            CoolDownImg.fillAmount = _rollCooldown;
        }
        else
        {
            GameManager.Instance.ShowAlert("재사용 대기 시간입니다!");
        }
    }

    public void Move(Vector2 movementInput, float movementSpeed, float movementSpeedModifier)
    {
        Vector2 movementDirection = movementInput.normalized;
        float speed = movementSpeed * movementSpeedModifier * _player.PlayerSO.SpeedMultiplier;
        Vector2 movement = movementDirection * speed * Time.deltaTime;
        if (_rigidbody != null)
        {
            _rigidbody.velocity = movement;
        }

    }

    public Vector2 GetVelocity()
    {
        return _rigidbody.velocity;
    }

    public void SetVelocity(Vector2 velocity)
    {
        _rigidbody.velocity = velocity;
    }

    public void PlayFootStep()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            GameManager.Instance.PlaySFX(SFX.FootstepGrass);
        }
        else
        {
            GameManager.Instance.PlaySFX(SFX.FootstepSand);
        }
    }
}
