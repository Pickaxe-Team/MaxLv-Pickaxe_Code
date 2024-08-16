using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ForestSpawner : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;
    protected ForestManager FM => ForestManager.Instance;
    protected ObjectPool ObjectPool => ForestManager.Instance.ObjectPool;

    [Header("TileMap")]
    [SerializeField] private Tilemap Ground;
    private Vector3 _worldTopLeft;
    private Vector3 _worldBottomRight;

    [Header("Random Position")]
    [SerializeField] private LayerMask Layer;
    private Vector2 _boxSize = Vector2.one;

    [Header("Spawn")]
    private int _maxAmount = 10;
    private bool _isInitializing = true;
    private WaitForSeconds _spawnTime = new WaitForSeconds(5f);
    private WaitForSeconds _fastSpawnTime = new WaitForSeconds(2f);
    private WaitForFixedUpdate _initSpawnTime = new WaitForFixedUpdate();

    [Header("Tree")]
    [SerializeField] private PoolObject Tree;
    public int CurTreeAmount = 0;
    private bool _isTreeSpawning = false;

    [Header("Rock")]
    [SerializeField] private PoolObject Rock;
    public int CurRockAmount = 0;
    private bool _isRockSpawning = false;

    [Header("Item")]
    public int _maxItemAmount = 40;

    [Header("Ghost")]
    [SerializeField] private PoolObject Ghost;
    private int _maxGhostAmount = 10;

    [Header("Bullet")]
    [SerializeField] private PoolObject Bullet;
    private int _maxBulletAmount = 20;

    public void AddObjectPool()
    {
        // Tree
        ObjectPool.AddObjectPool(SpawnObject.Tree, Tree, _maxAmount);

        // Rock
        ObjectPool.AddObjectPool(SpawnObject.Rock, Rock, _maxAmount);

        // Item
        ObjectPool.AddObjectPool(SpawnObject.Fruit, GameManager.Instance.GetItemData(ItemID.Fruit).Prefab, _maxItemAmount);
        ObjectPool.AddObjectPool(SpawnObject.Wood, GameManager.Instance.GetItemData(ItemID.Wood).Prefab, _maxItemAmount);
        ObjectPool.AddObjectPool(SpawnObject.Stone, GameManager.Instance.GetItemData(ItemID.Stone).Prefab, _maxItemAmount);

        // Ghost
        ObjectPool.AddObjectPool(Tag.Ghost, Ghost, _maxGhostAmount);

        // Bullet
        ObjectPool.AddObjectPool(Tag.Bullet, Bullet, _maxBulletAmount);
    }

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

    #region 초기 자원 생성

    public void InitSpawn()
    {
        StartCoroutine(InitSpawnDelay());
    }

    private IEnumerator InitSpawnDelay()
    {
        // 맵 데이터 있으면 맵 데이터로
        if (GM.NowMapData.InteractableObjects.Count != 0)
        {
            foreach (var interactObj in GM.NowMapData.InteractableObjects)
            {
                SpawnFromMapData(interactObj.Key, interactObj.Value.Position.GetVector(), interactObj.Value.Tag);
            }
        }
        else
        {
            for (int i = 0; i < _maxAmount; i++)
            {
                SpawnInteractObj(SpawnObject.Tree);
                yield return _initSpawnTime;
                SpawnInteractObj(SpawnObject.Rock);
                yield return _initSpawnTime;
            }
        }

        _isInitializing = false;

        // 로딩 UI 비활성화
        GameManager.Instance.LoadingUI.SetActive(false);
    }

    private void SpawnFromMapData(int index, Vector3 position, string tag)
    {
        PoolObject interactObj = ObjectPool.SpawnFromPool(tag);
        interactObj.transform.position = position;
        InteractableObject interactableObject = interactObj.ReturnMyComponent<InteractableObject>();
        interactableObject.Index = index;
        interactableObject.ObjectPool = ObjectPool;

        switch (tag)
        {
            case SpawnObject.Tree:
                CurTreeAmount++;
                break;

            case SpawnObject.Rock:
                CurRockAmount++;
                break;
        }
    }

    #endregion

    #region 자원 생성

    public void TreeSpawnUpdate()
    {
        if (!_isInitializing && !_isTreeSpawning && CurTreeAmount < _maxAmount)
        {
            StartCoroutine(SpawnDelay(SpawnObject.Tree));
        }
    }

    public void RockSpawnUpdate()
    {
        if (!_isInitializing && !_isRockSpawning && CurRockAmount < _maxAmount)
        {
            StartCoroutine(SpawnDelay(SpawnObject.Rock));
        }
    }

    private IEnumerator SpawnDelay(string name)
    {
        switch (name)
        {
            case SpawnObject.Tree:
                _isTreeSpawning = true;
                 yield return CurTreeAmount <= 4 ? _fastSpawnTime : _spawnTime;
                SpawnInteractObj(name);
                _isTreeSpawning = false;
                break;

            case SpawnObject.Rock:
                _isRockSpawning = true;
                yield return CurRockAmount <= 4 ? _fastSpawnTime : _spawnTime;
                SpawnInteractObj(name);
                _isRockSpawning = false;
                break;
        }
    }

    private void SpawnInteractObj(string name)
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
                PoolObject interactObj = ObjectPool.SpawnFromPool(name);
                interactObj.transform.position = randomPosition;

                switch (name)
                {
                    case SpawnObject.Tree:
                        CurTreeAmount++;
                        break;

                    case SpawnObject.Rock:
                        CurRockAmount++;
                        break;
                }

                // 게임매니저의 MapData에 포지션이랑 타입 저장
                MapObject mapObject = new MapObject();
                mapObject.Position.SetVector(randomPosition);
                mapObject.Tag = name;
                GM.NowMapData.InteractableObjects.Add(GM.NowMapData.Index, mapObject);
                InteractableObject interactableObject = interactObj.ReturnMyComponent<InteractableObject>();
                interactableObject.Index = GM.NowMapData.Index++;
                interactableObject.ObjectPool = ObjectPool;
                break;
            }

            limitCount--;
        }
    }

    #endregion
}