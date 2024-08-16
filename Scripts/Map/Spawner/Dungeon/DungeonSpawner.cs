using NavMeshPlus.Components;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class DungeonSpawner : MonoBehaviour
{
    protected DungeonManager DM => DungeonManager.Instance;
    protected ObjectPool ObjectPool => DungeonManager.Instance.ObjectPool;

    [Header("TileMap")]
    [SerializeField] protected Tilemap Ground;
    [SerializeField] protected NavMeshSurface NavMesh;
    [SerializeField] protected Tilemap[] Terrains;
    protected Vector3 _worldTopLeft;
    protected Vector3 _worldBottomRight;

    [Header("Random Position")]
    [SerializeField] protected LayerMask Layer;
    protected Vector2 _boxSize = Vector2.one;

    [Header("Spawn")]
    protected bool _isInitializing = true;
    protected WaitForFixedUpdate _initSpawnTime = new WaitForFixedUpdate();

    [Header("Ore")]
    public int CurOreAmount = 0;
    protected int _maxOreAmount = 10;
    protected bool _isOreSpawning = false;
    protected WaitForSeconds _oreSpawnTime = new WaitForSeconds(5f);

    [Header("Item")]
    protected int _maxItemAmount = 20;

    [Header("Enemy")]
    public int CurEnemyAmount = 0;
    protected int _maxEnemyAmount = 5;
    protected int _initEnemyAmount = 3;
    protected bool _isEnemySpawning = false;
    protected WaitForSeconds _enemySpawnTime = new WaitForSeconds(5f);

    [Header("Ghost")]
    [SerializeField] protected PoolObject Ghost;
    protected int _maxGhostAmount = 10;

    [Header("Bullet")]
    [SerializeField] protected PoolObject Bullet;
    protected int _maxBulletAmount = 20;

    public abstract void AddObjectPool();
    protected abstract string OreNameByRatio();
    protected abstract string EnemyNameByRatio();

    #region 타일맵 사이즈 계산

    public void CalculateTileBounds()
    {
        // 실제로 타일이 그려진 부분의 경계 찾기
        Vector3Int min = new Vector3Int(int.MaxValue, int.MaxValue, 0);
        Vector3Int max = new Vector3Int(int.MinValue, int.MinValue, 0);

        foreach (Vector3Int pos in Ground.cellBounds.allPositionsWithin)
        {
            if (Ground.HasTile(pos))
            {
                min.x = Mathf.Min(min.x, pos.x);
                min.y = Mathf.Min(min.y, pos.y);
                max.x = Mathf.Max(max.x, pos.x);
                max.y = Mathf.Max(max.y, pos.y);
            }
        }

        // 가장자리 타일 제외
        min.x += 2;
        min.y += 2;
        max.x -= 2;
        max.y -= 2;

        // 좌측 상단과 우측 하단 모서리의 로컬 좌표 계산
        Vector3Int topLeft = new Vector3Int(min.x, max.y, 0);
        Vector3Int bottomRight = new Vector3Int(max.x, min.y, 0);

        // 로컬 좌표를 월드 좌표로 변환
        _worldTopLeft = Ground.CellToWorld(topLeft);
        _worldBottomRight = Ground.CellToWorld(bottomRight);
    }

    #endregion

    #region 초기 자원 및 적 생성

    public void InitSpawn()
    {
        StartCoroutine(InitSpawnDelay());
    }

    protected IEnumerator InitSpawnDelay()
    {
        for (int i = 0; i < _maxOreAmount; i++)
        {
            SpawnOre();
            yield return _initSpawnTime;
        }

        for (int i = 0; i < _initEnemyAmount; i++)
        {
            SpawnEnemy();
            yield return _initSpawnTime;
        }

        _isInitializing = false;

        // 로딩 UI 비활성화
        GameManager.Instance.LoadingUI.SetActive(false);
    }

    #endregion

    #region 자원 생성

    public void OreSpawnUpdate()
    {
        if (!_isInitializing && !_isOreSpawning && CurOreAmount < _maxOreAmount)
        {
            StartCoroutine(OreSpawnDelay());
        }
    }

    protected IEnumerator OreSpawnDelay()
    {
        _isOreSpawning = true;
        yield return _oreSpawnTime;
        SpawnOre();
        _isOreSpawning = false;
    }

    protected void SpawnOre()
    {
        int limitCount = 50;

        while (limitCount > 0)
        {
            // 랜덤 위치 계산
            Vector3 randomPosition = new Vector3(Random.Range(_worldTopLeft.x, _worldBottomRight.x), Random.Range(_worldBottomRight.y, _worldTopLeft.y), 0);

            // 해당 위치에 오브젝트가 있는지 콜라이더로 확인
            Collider2D obj = Physics2D.OverlapBox(randomPosition, _boxSize, 0, Layer);

            // 오브젝트가 없으면 자원 생성
            if (obj == null)
            {
                PoolObject ore = ObjectPool.SpawnFromPool(OreNameByRatio());
                ore.transform.position = randomPosition;
                ore.ReturnMyComponent<InteractableObject>().ObjectPool = ObjectPool;
                CurOreAmount++;
                break;
            }

            limitCount--;
        }
    }

    #endregion

    #region 적 생성

    public void EnemySpawnUpdate()
    {
        if (!_isInitializing && !_isEnemySpawning && CurEnemyAmount < _maxEnemyAmount)
        {
            StartCoroutine(EnemySpawnDelay());
        }
    }

    protected IEnumerator EnemySpawnDelay()
    {
        _isEnemySpawning = true;
        yield return _enemySpawnTime;
        SpawnEnemy();
        _isEnemySpawning = false;
    }

    protected void SpawnEnemy()
    {
        int limitCount = 50;

        while (limitCount > 0)
        {
            // 랜덤 위치 계산
            Vector3 randomPosition = new Vector3(Random.Range(_worldTopLeft.x, _worldBottomRight.x), Random.Range(_worldBottomRight.y, _worldTopLeft.y), 0);

            // 해당 위치에 오브젝트가 있는지 콜라이더로 확인
            Collider2D obj = Physics2D.OverlapBox(randomPosition, _boxSize, 0, Layer);

            // 오브젝트가 없으면 적 생성
            if (obj == null)
            {
                PoolObject enemy = ObjectPool.SpawnFromPool(EnemyNameByRatio());
                enemy.transform.position = randomPosition;
                enemy.ReturnMyComponent<Enemy>().ObjectPool = ObjectPool;
                CurEnemyAmount++;
                break;
            }

            limitCount--;
        }
    }

    #endregion
}