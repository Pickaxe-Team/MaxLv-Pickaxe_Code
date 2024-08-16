using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    private PlayerData NowPlayerData => GameManager.Instance.NowPlayerData;
    private PickaxeData NowPickaxeData => GameManager.Instance.GetPickaxeData((PickaxeID)GameManager.Instance.NowPlayerData.PickaxeLV);

    private float _attackStartTime;
    private Bullet _bulletPrefab;
    private float _attackCooldown = 0.5f; // Define the attack cooldown duration
    private float _lastAttackTime;
    private float _rayLength = 1.5f; // 원하는 Ray 길이 설정

    public PlayerAttackState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
        _bulletPrefab = _stateMachine.Player.BulletPrefab.GetComponent<Bullet>();
    }

    public override void Enter()
    {
        _attackStartTime = Time.time;
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(_stateMachine.Player.AnimationData.WalkParameterHash);
        StopAnimation(_stateMachine.Player.AnimationData.IdleParameterHash);
        _stateMachine.Player.ResetPickAxeAttack();
        if (_stateMachine.Player.FirePoint != null)
        {
            _stateMachine.Player.FirePoint.transform.localPosition = new Vector3(0f, 0f, 0f);
        }
    }

    public override void Update()
    {
        base.Update();
        Move();
        PerformAttack();
        CheckForIdleState();
        CheckAnimation();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void PerformAttack()
    {
        Vector2 rayEndPoint = (Vector2)_stateMachine.Player.transform.position + _stateMachine.AttackInput.normalized * _rayLength / 2;
        PlaceObjectAtRayEnd(rayEndPoint);
        if (Time.time >= _lastAttackTime + _attackCooldown && _stateMachine.AttackInput != Vector2.zero)
        {
            ShootBullet();
            _stateMachine.Player.PlayPickAxeAttack();
            RaycastHit2D hit = PerformRaycast(_rayLength);
            if (hit.collider != null)
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                InteractableObject interactableObject = hit.collider.GetComponent<InteractableObject>();
                Boss boss = hit.collider.GetComponent<Boss>();
                if (enemy != null)
                {
                    int nowPickaxeLV = NowPlayerData.PickaxeLV;
                    Vector2 knockbackDirection = (enemy.transform.position - _stateMachine.Player.transform.position).normalized;
                    enemy.TakeDamage(NowPickaxeData.ATK, knockbackDirection); //(곡괭이 공격력, knockbackDirection)
                }
                else if (interactableObject != null)
                {
                    int nowPickaxeLV = NowPlayerData.PickaxeLV;
                    interactableObject.TakeAtk(nowPickaxeLV, NowPickaxeData.ATK); // (곡괭이레벨 , 곡괭이 공격력)
                }
                else if (boss != null)
                {
                    int nowPickaxeLV = NowPlayerData.PickaxeLV;
                    boss.TakeDamage(NowPickaxeData.ATK);
                }
            }
            else
            {
                GameManager.Instance.PlaySFX(SFX.Swing);
            }
            _lastAttackTime = Time.time;
        }
    }

    private void Move()
    {
        _stateMachine.Player.Input.Move(_stateMachine.MovementInput, _stateMachine.MovementSpeed, _playerData.SpeedMultiplier);
    }

    private void ShootBullet()
    {
        Vector2 bulletDirection = _stateMachine.AttackInput.normalized;
        if (bulletDirection != Vector2.zero && NowPickaxeData.CanPickaxeAura)
        {
            // 총알이 생성될 위치 설정
            Vector2 firePointPosition = _stateMachine.Player.FirePoint.position;

            // 총알을 생성하고 방향 설정
            CreateBullet("Bullet", firePointPosition, bulletDirection, Tag.Player);
        }
    }

    private void CreateBullet(string tag, Vector2 position, Vector2 direction, string ownerTag)
    {       
        PoolObject bullet = _stateMachine.Player.ObjectPool.SpawnFromPool(tag);

        if (bullet != null)
        {
            bullet.ReturnMyComponent<Bullet>().Initialize(position, direction, ownerTag);
            bullet.gameObject.SetActive(true);         
        }
    }

    private RaycastHit2D PerformRaycast(float rayRange)
    {
        Vector2 rayDirection = _stateMachine.AttackInput.normalized;
        Vector2 rayOrigin = _stateMachine.Player.transform.position;

        string[] layersToExclude = { Layer.Player, Layer.Confiner, Layer.Terrain, Layer.BossBullet, Layer.SafeArea };

        int layerMask = 0;
        foreach (string layer in layersToExclude)
        {
            layerMask |= 1 << LayerMask.NameToLayer(layer);
        }

        layerMask = ~layerMask;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayRange, layerMask);

        return hit;
    }

    private void PlaceObjectAtRayEnd(Vector2 position)
    {
        if (_stateMachine.Player.FirePoint != null)
        {
            _stateMachine.Player.FirePoint.transform.position = position;
        }
    }

    private void CheckForIdleState()
    {
        if (_stateMachine.AttackInput == Vector2.zero)
        {
            _stateMachine.ChangeState(_stateMachine.IdleState);
        }
    }
    private void CheckAnimation()
    {
        Vector2 currentMovementInput = _stateMachine.Player.Input.PlayerInputActions.Player.Move.ReadValue<Vector2>();
        if (currentMovementInput.magnitude > 0)
        {
            StartAnimation(_stateMachine.Player.AnimationData.WalkParameterHash);
            StopAnimation(_stateMachine.Player.AnimationData.IdleParameterHash);
        }
        else
        {
            StartAnimation(_stateMachine.Player.AnimationData.IdleParameterHash);
            StopAnimation(_stateMachine.Player.AnimationData.WalkParameterHash);
        }
    }
}