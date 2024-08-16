using UnityEngine;
using UnityEngine.AI;

public class EnemyIdleState : EnemyBaseState
{
    private float _waitTime = 2f; // 대기 시간
    private float _waitTimer;
    private float _moveRadius = 2f; // 랜덤 이동 반경
    private bool _isWaiting;
    private float _patrolTime = 3f; // 순찰 시간
    private float _patrolTimer;

    public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
        _waitTimer = _waitTime; // 초기화
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(_stateMachine.Enemy.AnimationData.IdleParameterHash);
        MoveToRandomPosition();
        _isWaiting = false; // 대기 상태 초기화
        _patrolTimer = _patrolTime; // 순찰 타이머 초기화.
        _stateMachine.Enemy.PlayerChasingRange = _stateMachine.Enemy.EnemyData.PlayerChasingRange;
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(_stateMachine.Enemy.AnimationData.IdleParameterHash);
    }

    public override void Update()
    {
        base.Update();

        // 순찰 시간이 3초 이상 지속되면 새로운 방향을 찾음
        _patrolTimer -= Time.deltaTime;
        if (_patrolTimer <= 0f)
        {
            MoveToRandomPosition();
            _patrolTimer = _patrolTime; // 순찰 타이머 초기화
        }

        // 적이 이동할 수 없는 경로로 가고 있는지 확인
        if (_stateMachine.Enemy.Agent.pathStatus == NavMeshPathStatus.PathInvalid ||
            _stateMachine.Enemy.Agent.pathStatus == NavMeshPathStatus.PathPartial)
        {
            MoveToRandomPosition();
        }

        if (_isWaiting)
        {
            _waitTimer -= Time.deltaTime;
            if (_waitTimer <= 0f)
            {
                _isWaiting = false;
                MoveToRandomPosition();
                _patrolTimer = _patrolTime; // 새로운 경로를 찾을 때마다 순찰 타이머 초기화
            }
        }
        else if (!_stateMachine.Enemy.Agent.pathPending && _stateMachine.Enemy.Agent.remainingDistance <= _stateMachine.Enemy.Agent.stoppingDistance)
        {
            _isWaiting = true; // 이동 완료 후 대기 상태로 전환
            _waitTimer = _waitTime; // 대기 타이머 초기화
        }

        float playerDistanceSqr = (_stateMachine.Target.transform.position - _stateMachine.Enemy.transform.position).sqrMagnitude;

        if (playerDistanceSqr <= _stateMachine.Enemy.PlayerChasingRange * _stateMachine.Enemy.PlayerChasingRange)
        {
            _stateMachine.ChangeState(_stateMachine.MoveState);
        }
    }

    private void MoveToRandomPosition()
    {
        Vector3 randomDirection = Vector3.zero;
        NavMeshHit hit;
        bool validPositionFound = false;
        int attempts = 0;

        while (!validPositionFound && attempts < 10)
        {
            attempts++;
            randomDirection = Random.insideUnitSphere * _moveRadius;
            randomDirection += _stateMachine.Enemy.transform.position; // 현재 적의 위치를 기준으로 랜덤 위치 설정

            // NavMesh.SamplePosition을 사용하여 랜덤 위치를 NavMesh 상의 유효한 위치로 변환
            if (NavMesh.SamplePosition(randomDirection, out hit, _moveRadius, NavMesh.AllAreas))
            {
                Vector3 finalPosition = hit.position;

                // 유효한 NavMesh 위치이고 벽에 너무 가깝지 않은지 확인
                if (IsValidNavMeshPosition(finalPosition) && !IsPositionNearWall(finalPosition) && !IsPositionNearNavMeshEdge(finalPosition))
                {
                    _stateMachine.Enemy.Agent.SetDestination(finalPosition);
                    _stateMachine.Enemy.Agent.isStopped = false;
                    validPositionFound = true;
                }
            }
        }

        // 벽에 부딪혔을 경우 반대 방향으로 이동 시도
        if (!validPositionFound)
        {
            attempts = 0;
            while (!validPositionFound && attempts < 10)
            {
                attempts++;
                randomDirection = -Random.insideUnitSphere * _moveRadius;
                randomDirection += _stateMachine.Enemy.transform.position; // 반대 방향도 적의 위치를 기준으로 설정
                if (NavMesh.SamplePosition(randomDirection, out hit, _moveRadius, NavMesh.AllAreas) && IsValidNavMeshPosition(hit.position) && !IsPositionNearWall(hit.position) && !IsPositionNearNavMeshEdge(hit.position))
                {
                    _stateMachine.Enemy.Agent.SetDestination(hit.position);
                    _stateMachine.Enemy.Agent.isStopped = false;
                    validPositionFound = true;
                }
            }
        }
    }

    private bool IsValidNavMeshPosition(Vector3 position)
    {
        NavMeshPath path = new NavMeshPath();
        _stateMachine.Enemy.Agent.CalculatePath(position, path);
        return path.status == NavMeshPathStatus.PathComplete;
    }

    private bool IsPositionNearWall(Vector3 position)
    {
        // 위치 주변에 벽이 있는지 확인
        float wallDetectionRadius = 1.0f; // 필요에 따라 조정 가능
        Collider[] hitColliders = Physics.OverlapSphere(position, wallDetectionRadius, LayerMask.GetMask("Terrain"));
        return hitColliders.Length > 0;
    }

    private bool IsPositionNearNavMeshEdge(Vector3 position)
    {
        NavMeshHit hit;
        float edgeDetectionRadius = 1.0f; // 필요에 따라 조정 가능
        // 위치에서 일정 반경 내에 네비게이션 메쉬 가장자리가 있는지 확인
        if (NavMesh.FindClosestEdge(position, out hit, NavMesh.AllAreas))
        {
            return hit.distance < edgeDetectionRadius;
        }
        return false;
    }
}
