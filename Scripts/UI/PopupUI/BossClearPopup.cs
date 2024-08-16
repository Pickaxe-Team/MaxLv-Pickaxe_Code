using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossClearPopup : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI ClearText;
    [SerializeField] private TextMeshProUGUI ExitText;
    [SerializeField] private Button ExitButton;

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
        Tween clearText = ClearText.DOFade(1f, 1.5f).SetUpdate(true);
        Tween exitText = ExitText.DOFade(1f, 1.5f).SetUpdate(true);

        // 시퀀스에 트윈 추가
        sequence.Append(clearText);
        sequence.Append(exitText);
    }

    public void OnClickExitBtn()
    {
        sequence.Kill();

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(Layer.Player), LayerMask.NameToLayer(Layer.BossBullet), false);
        SceneLoadManager.LoadScene(2);
    }
}
