using System.Collections;
using NavMeshPlus.Components;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonManager : Singleton<DungeonManager>
{
    private GameManager GM => GameManager.Instance;

    [Header("Cashing")]
    [SerializeField] private Player Player;
    [SerializeField] private UIInventory UIInventory;
    [SerializeField] private GameObject GameOverPopup;

    [Header("TileMap")]
    [SerializeField] private NavMeshSurface NavMesh;
    [SerializeField] private Tilemap[] Terrains;

    [Header("Dungeon")]
    public DungeonSpawner NowDungeon;
    public List<DungeonSpawner> Dungeons;
    private int _nowStage;

    public ObjectPool ObjectPool;

    protected override void Awake()
    {
        if (IsDuplicates()) return;

        base.Awake();

        // 씬 초기화
        InitScene();
    }

    private void AfterInitComplete()
    {
        // 오브젝트 풀에 자원 & 몬스터 & 아이템 추가
        NowDungeon.AddObjectPool();

        // 초기 자원 및 몬스터 생성
        NowDungeon.InitSpawn();
    }

    #region 씬 초기화

    private void InitScene()
    {
        GM.PlayBGM(BGM.Dungeon);

        // 타일맵 중 랜덤으로 선택해서 활성화
        Terrains[Random.Range(0, Terrains.Length)].gameObject.SetActive(true);

        // 해당 던전 인덱스 세팅
        _nowStage = GM.StageNum;
        NowDungeon = Dungeons[_nowStage];
        ObjectPool = GetComponent<ObjectPool>();

        GM.Player = Player;
        GM.UIInventory = UIInventory;
        GM.GameOverPopup = GameOverPopup;
        Player.ObjectPool = ObjectPool;

        // 인벤토리 초기화
        UIInventory.InitInventory();

        // 타일맵 사이즈 계산
        NowDungeon.CalculateTileBounds();

        // NavMesh 베이크
        StartCoroutine(BakeNavMesh());
    }

    IEnumerator BakeNavMesh()
    {
        var asyncOperation = NavMesh.BuildNavMeshAsync();
        yield return new WaitUntil(() => asyncOperation.isDone);
        // NavMesh 최초 업데이트
        var asyncUpdateOperation = NavMesh.UpdateNavMesh(NavMesh.navMeshData);
        yield return new WaitUntil(() => asyncUpdateOperation.isDone);
        AfterInitComplete();

        while (true)
        {
            // 최대 자원 수 만큼 자원 생성
            NowDungeon.OreSpawnUpdate();

            // 최대 몬스터 수 만큼 몬스터 생성
            NowDungeon.EnemySpawnUpdate();

            // NavMesh 업데이트
            NavMesh.UpdateNavMesh(NavMesh.navMeshData);

            yield return null;
        }
    }

    #endregion
}