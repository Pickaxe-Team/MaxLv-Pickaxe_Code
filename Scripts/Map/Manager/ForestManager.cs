using System.Collections;
using UnityEngine;

public class ForestManager : Singleton<ForestManager>
{
    private GameManager GM => GameManager.Instance;

    [Header("Cashing")]
    [SerializeField] private Player Player;
    [SerializeField] private UIInventory UIInventory;

    [Header("Spawn")]
    public ForestSpawner ForestSpawner;

    public ObjectPool ObjectPool;

    protected override void Awake()
    {
        if (IsDuplicates()) return;

        base.Awake();

        // 씬 초기화
        InitScene();
    }

    private void Start()
    {
        // 오브젝트 풀에 자원 & 아이템 추가
        ForestSpawner.AddObjectPool();

        // 초기 자원 생성
        ForestSpawner.InitSpawn();
    }

    private void Update()
    {
        // 최대 나무 자원 수 만큼 자원 생성
        ForestSpawner.TreeSpawnUpdate();

        // 최대 바위 자원 수 만큼 자원 생성
        ForestSpawner.RockSpawnUpdate();
    }

    #region 씬 초기화

    private void InitScene()
    {
        Time.timeScale = 1f;

        GM.PlayBGM(BGM.Forest);

        GM.Player = Player;
        GM.UIInventory = UIInventory;

        ObjectPool = GetComponent<ObjectPool>();
        Player.ObjectPool = ObjectPool;

        Player.transform.SetPositionAndRotation(GM.NowPlayerData.Position.GetVector(), Quaternion.Euler(GM.NowPlayerData.Rotation.GetVector()));

        // 인벤토리 초기화
        UIInventory.InitInventory();

        // 타일맵 사이즈 계산
        ForestSpawner.CalculateTileBounds();

        GM.SaveAllData(Player.transform.position, Player.transform.rotation);
    }

    #endregion
}
