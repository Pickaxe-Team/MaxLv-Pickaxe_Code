using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPopup : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI GameoverText;
    [SerializeField] private TextMeshProUGUI ContinueText;
    [SerializeField] private Button ContinueButton;

    private Sequence sequence;

    private void Start()
    {
        StartTween();
    }

    private void StartTween()
    {
        // 시퀀스 생성
        sequence = DOTween.Sequence().SetUpdate(true);

        // 트윈 생성
        Tween gameoverText = GameoverText.DOFade(1f, 1.5f).SetUpdate(true);
        Tween continueText = ContinueText.DOFade(1f, 1.0f).SetUpdate(true);

        // 시퀀스에 트윈 추가
        sequence.Append(gameoverText);
        sequence.Append(continueText);
    }

    public void OnClickContinue()
    {
        // 체력 풀로 회복
        GM.NowPlayerData.CurHP = GM.Player.MaxHP;

        // 경험치 감소 패널티
        GM.NowPlayerData.CurEXP = Mathf.Max(0, GM.NowPlayerData.CurEXP - GM.Player.MaxEXP * 0.1f);

        // 인벤토리 모든 아이템 제거 패널티
        GM.UIInventory.RemoveAllItem();

        sequence.Kill();

        SceneLoadManager.LoadScene(2);
    }
}
