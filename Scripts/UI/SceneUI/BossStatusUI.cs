using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossStatusUI : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;
    [Header("UI")]
    [SerializeField] private Slider HPBar;
    [SerializeField] private Image HPbarImage;
    [SerializeField] private TextMeshProUGUI HPText;
    [SerializeField] private List<Boss> Bosses;
    private Boss _nowBoss;

    private void Start()
    {
        InitStatus();
        SetStatusEvent();
        HPbarImage.color = GM.GetHPColor(HPColor.Red);
    }

    #region Status 초기화
    private void InitStatus()
    {
        _nowBoss = Bosses[GM.StageNum];
        UpdateHPStatus();
    }
    #endregion

    #region 이벤트 등록
    private void SetStatusEvent()
    {
        _nowBoss.OnChangeBossHP += UpdateHPStatus;
    }
    #endregion

    #region HP 업데이트
    private void UpdateHPStatus()
    {
        float hpRatio = _nowBoss.BossCurHP / _nowBoss.EnemyData.HP;

        HPBar.value = hpRatio;
        HPText.text = $"{hpRatio * 100:F2} %";
    }
    #endregion
}
