using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingPopup : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;
    private AudioMixer AudioMixer => GameManager.Instance.GetAudioMixer();

    [Header("Sound")]
    [SerializeField] private Slider BGMSlider;
    [SerializeField] private Slider SFXSlider;

    private void Awake()
    {
        BGMSlider.onValueChanged.AddListener(ChangeBGMVolume);
        SFXSlider.onValueChanged.AddListener(ChangeSFXVolume);
    }

    private void OnEnable()
    {
        if (AudioMixer.GetFloat(Mixer.BGM, out float BGMVolume))
        {
            BGMSlider.value = Mathf.Pow(10, (BGMVolume / 20));
        }

        if (AudioMixer.GetFloat(Mixer.SFX, out float SFXVolume))
        {
            SFXSlider.value = Mathf.Pow(10, (SFXVolume / 20));
        }
    }

    private void OnDisable()
    {
        AudioMixer.GetFloat(Mixer.BGM, out float BGMVolume);
        GM.NowOptionData.BGMVolume = BGMVolume;

        AudioMixer.GetFloat(Mixer.SFX, out float SFXVolume);
        GM.NowOptionData.SFXVolume = SFXVolume;

        GM.SaveOptionData();
    }

    public void Initializer()
    {
        AudioMixer.SetFloat(Mixer.BGM, GM.NowOptionData.BGMVolume);
        AudioMixer.SetFloat(Mixer.SFX, GM.NowOptionData.SFXVolume);
    }

    public void ChangeBGMVolume(float volume)
    {
        if (volume == 0)
        {
            AudioMixer.SetFloat(Mixer.BGM, -80f);
        }
        else
        {
            AudioMixer.SetFloat(Mixer.BGM, Mathf.Log10(volume) * 20);
        }
    }

    public void ChangeSFXVolume(float volume)
    {
        if (volume == 0)
        {
            AudioMixer.SetFloat(Mixer.SFX, -80f);
        }
        else
        {
            AudioMixer.SetFloat(Mixer.SFX, Mathf.Log10(volume) * 20);
        }
    }

    public void OnClickSaveBtn()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            GM.SaveAllData(GM.Player.transform.position, GM.Player.transform.rotation);
            GameManager.Instance.PlaySFX(SFX.Click);

            SceneLoadManager.LoadScene(0);
        }
        else
        {
            GM.ShowAlert("던전 안에서는 저장할 수 없습니다.");
        }
    }

    #region 설정 창 활성화 토글

    public void Toggle()
    {
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
            GameManager.Instance.PlaySFX(SFX.Click);
        }
        else
        {
            gameObject.SetActive(true);
            GameManager.Instance.PlaySFX(SFX.Click);
        }
    }

    #endregion
}
