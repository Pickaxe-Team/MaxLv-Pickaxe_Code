using System.Collections.Generic;
using UnityEngine;

public class BossSceneManager : Singleton<BossSceneManager>
{
    private GameManager GM => GameManager.Instance;

    [Header("Cashing")]
    [SerializeField] private Player Player;
    [SerializeField] private UIInventory UIInventory;
    [SerializeField] private GameObject GameOverPopup;

    [Header("Ghost")]
    [SerializeField] private PoolObject GhostPrefab;
    public int GhostMaxCount = 10;

    [Header("Spawn")]
    public BossSpawner NowBoss;
    public List<BossSpawner> Bosses;
    [SerializeField] private List<GameObject> _bossPrefabs;
    private int _nowStage;

    [HideInInspector] public ObjectPool ObjectPool;

    protected override void Awake()
    {
        if (IsDuplicates()) return;

        base.Awake();

        // 맵 초기화
        InitScene();
    }

    private void Start()
    {
        NowBoss.AddObjectPool();
    }

    #region 씬 초기화

    private void InitScene()
    {
        GM.PlayBGM(BGM.Boss);

        // 해당 던전 인덱스 세팅
        _nowStage = GM.StageNum;
        NowBoss = Bosses[_nowStage];
        ObjectPool = GetComponent<ObjectPool>();

        GM.Player = Player;
        GM.UIInventory = UIInventory;
        GM.GameOverPopup = GameOverPopup;
        Player.ObjectPool = ObjectPool;

        // 인벤토리 초기화
        UIInventory.InitInventory();

        // 로딩 UI 비활성화
        GM.LoadingUI.SetActive(false);

        _bossPrefabs[GM.StageNum].SetActive(true);
    }

    #endregion
}
