using DG.Tweening;
using TMPro;
using UnityEngine;

public class TitleUI : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI TitleText;
    [SerializeField] private RectTransform StartButton;
    [SerializeField] private RectTransform LoadButton;
    [SerializeField] private RectTransform SettingButton;
    [SerializeField] private RectTransform ExitButton;
    [SerializeField] private GameObject SettingPopup;
    [SerializeField] private GameObject NewGamePopup;

    public Sequence Sequence;

    private void Start()
    {
        GM.LoadingUI.SetActive(false);

        StartTween();
    }

    private void StartTween()
    {
        // 시퀀스 생성
        Sequence = DOTween.Sequence();

        // 트윈 생성
        Tween titleTxt = TitleText.DOFade(1f, 1f);
        Tween startBtn = StartButton.DOLocalMoveX(225f, 1f).SetEase(Ease.OutBack);
        Tween loadBtn = LoadButton.DOLocalMoveX(225f, 1f).SetEase(Ease.OutBack);
        Tween settingBtn = SettingButton.DOLocalMoveX(225f, 1f).SetEase(Ease.OutBack);
        Tween exitBtn = ExitButton.DOLocalMoveX(225f, 1f).SetEase(Ease.OutBack);

        // 시퀀스에 트윈 추가
        Sequence.Append(titleTxt);
        Sequence.Append(startBtn);
        Sequence.Insert(1.2f, loadBtn);
        Sequence.Insert(1.4f, settingBtn);
        Sequence.Insert(1.8f, exitBtn);
    }

    public void OnClickStartBtn()
    {
        NewGamePopup.SetActive(true);
        GM.PlaySFX(SFX.Click);
    }

    public void OnClickLoadBtn()
    {
        if (GM.LoadAllData())
        {
            GM.PlaySFX(SFX.Click);

            SceneLoadManager.LoadScene(2);
        }
        else
        {
            GM.ShowAlert("저장된 데이터가 없습니다.");
        }
        Sequence.Kill();
    }

    public void OnClickSettingBtn()
    {
        SettingPopup.SetActive(true);
        GM.PlaySFX(SFX.Click);
    }

    public void OnClickExitBtn()
    {
        GM.PlaySFX(SFX.Click);

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
