using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public Player Player;
    public UIInventory UIInventory;
    public GameObject GameOverPopup;

    public GameObject LoadingUI;
    public Slider ProgressBar;
    public TextMeshProUGUI ProgressText;

    public int StageNum;

    protected override void Awake()
    {
        if (IsDuplicates()) return;

        base.Awake();

        // 실제 모바일 테스트 시 30, 60 비교해보기
        Application.targetFrameRate = 60;

        SoundManager.Initializer();
        DataManager.Initializer();
    }

    #region 경고 알림

    public GameObject AlertObject;
    public TextMeshProUGUI AlertText;
    private Coroutine _alertCoroutine;
    public void ShowAlert(string msg)
    {
        AlertText.text = msg;
        AlertObject.SetActive(true);
        PlaySFX(SFX.Alert);

        if (_alertCoroutine != null) StopCoroutine(_alertCoroutine);
        _alertCoroutine = StartCoroutine(AlertCo());
    }
    private IEnumerator AlertCo()
    {
        yield return new WaitForSecondsRealtime(2f);
        AlertObject.SetActive(false);
    }

    #endregion

    #region 데이터

    [SerializeField] private DataManager DataManager;

    public Color GetGradeColor(Grade grade)
    {
        return DataManager.GradeColor[(int)grade];
    }

    public Color GetHPColor(HPColor color)
    {
        return DataManager.HPColor[(int)color];
    }

    public PlayerSO GetPlayerData()
    {
        return DataManager.PlayerData;
    }

    public ItemData GetItemData(ItemID id)
    {
        return DataManager.ItemDatas[id];
    }

    public EntityData GetEntityData(EntityID id)
    {
        return DataManager.EntityDatas[id];
    }

    public PickaxeData GetPickaxeData(PickaxeID id)
    {
        return DataManager.PickaxeDatas[id];
    }

    #endregion

    #region 세이브

    [SerializeField] private SaveManager SaveManager;

    public PlayerData NowPlayerData;
    public void SavePlayerData(Vector3 pos, Quaternion rot)
    {
        NowPlayerData.Position.SetVector(pos);
        NowPlayerData.Rotation.SetVector(rot.eulerAngles);
        SaveManager.SaveData(NowPlayerData);
    }
    public bool LoadPlayerData()
    {
        if(SaveManager.TryLoadData(out PlayerData data))
        {
            NowPlayerData = data;
            return true;
        }
        else
        {
            return false;
        } 
    }

    public MapData NowMapData;
    public void SaveMapData()
    {
        SaveManager.SaveData(NowMapData);
    }
    public bool LoadMapData()
    {
        if (SaveManager.TryLoadData(out MapData data))
        {
            NowMapData = data;
            
            return true;
        }
        else
        {
            return false;
        } 
    }

    public OptionData NowOptionData;
    public void SaveOptionData()
    {
        SaveManager.SaveData(NowOptionData);
    }
    public bool LoadOptionData()
    {
        if (SaveManager.TryLoadData(out OptionData data))
        {
            NowOptionData = data;
            return true;
        }
        else
        {
            NowOptionData.BGMVolume = 0;
            NowOptionData.SFXVolume = 0;
            return false;
        }
    }

    public UserData NowUserData;
    public void SaveUserData()
    {
        SaveManager.SaveData(NowUserData);
    }
    public bool LoadUserData()
    {
        if (SaveManager.TryLoadData(out UserData data))
        {
            NowUserData = data;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SaveAllData(Vector3 playerPos, Quaternion PlayerRot)
    {
        SavePlayerData(playerPos, PlayerRot);
        SaveMapData();
        SaveUserData();
    }
    public bool LoadAllData() // todo: 수정 필요
    {
        if(LoadPlayerData() && LoadMapData() && LoadUserData())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion

    #region 사운드

    [SerializeField] private SoundManager SoundManager;

    public AudioMixer GetAudioMixer()
    {
        return SoundManager.AudioMixer;
    }

    public void PlayBGM(BGM target)
    {
        SoundManager.PlayBGM(target);
    }

    public void PlaySFX(SFX target)
    {
        SoundManager.PlaySFX(target);
    }

    #endregion

    #region 시간 관리
    [SerializeField] private TimeManager TimeManager;

    public DateTime GetLastCollectionTime(ItemID itemID)
    {
        return TimeManager.GetLastCollectionTime(itemID);
    }

    public float GetElapsedTime(ItemID id)
    {
        return TimeManager.GetElapsedTime(id);
    }

    #endregion
}
