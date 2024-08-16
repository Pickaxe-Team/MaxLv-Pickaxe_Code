using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUI : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;

    [Header("UI")]
    [SerializeField] private Slider HPBar;
    [SerializeField] private Slider EXPBar;
    [SerializeField] private TextMeshProUGUI HPText;
    [SerializeField] private TextMeshProUGUI EXPText;
    [SerializeField] private TextMeshProUGUI LVText;

    private void Start()
    {
        InitStatus();
        SetStatusEvent();
    }

    #region Status 초기화
    private void InitStatus()
    {
        UpdateHPStatus();
        UpdateEXPStatus();
        UpdateLevel();
    }
    #endregion

    #region 이벤트 등록
    private void SetStatusEvent()
    {
        GM.Player.OnChangePlayerHP += UpdateHPStatus;
        GM.Player.OnChangeEXP += UpdateEXPStatus;
        GM.Player.OnChangeLV += UpdateLevel;
        GM.Player.OnChangeLV += UpdateHPStatus;
    }
    #endregion

    #region HP 업데이트
    private void UpdateHPStatus()
    {
        float hpRatio = GM.NowPlayerData.CurHP / GM.Player.MaxHP;

        HPBar.value = hpRatio;
        HPText.text = $"{hpRatio * 100:F2} %";
    }
    #endregion

    #region EXP 업데이트
    private void UpdateEXPStatus()
    {
        float expRatio = GM.NowPlayerData.CurEXP / GM.Player.MaxEXP;

        EXPBar.value = expRatio;
        EXPText.text = $"{expRatio * 100:F2} %";
    }
    #endregion

    #region LV 업데이트
    private void UpdateLevel()
    {
        LVText.text = $"{GM.NowPlayerData.LV}";
    }
    #endregion
}
