using UnityEngine;

public class TitleManager : Singleton<TitleManager>
{
    private GameManager GM => GameManager.Instance;

    [SerializeField] private SettingPopup SettingPopup;

    protected override void Awake()
    {
        if (IsDuplicates()) return;

        base.Awake();
    }

    private void Start()
    {
        GM.LoadOptionData();
        SettingPopup.Initializer();

        GM.PlayBGM(BGM.Title);
    }
}
