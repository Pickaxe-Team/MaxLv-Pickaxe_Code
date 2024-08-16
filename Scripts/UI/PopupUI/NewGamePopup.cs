using DG.Tweening;
using UnityEngine;

public class NewGamePopup : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;

    [Header("UI")]
    [SerializeField] private TitleUI TitleUI;

    public void OnClickOK()
    {
        SetPlayerData();
        SetUserData();
        GM.PlaySFX(SFX.Click);
        SceneLoadManager.LoadScene(2);
        GM.NowUserData.ClearedBosses.Clear();
        TitleUI.Sequence.Kill();
    }

    public void OnClickCancle()
    {
        gameObject.SetActive(false);
        GM.PlaySFX(SFX.Click);
    }

    private void SetPlayerData()
    {
        GM.NowPlayerData.LV = GM.GetPlayerData().Level;
        GM.NowPlayerData.CurHP = GM.GetPlayerData().BaseHP;
        GM.NowPlayerData.CurEXP = 0f;
        GM.NowPlayerData.PickaxeLV = 1;
        GM.NowPlayerData.Inventory.Clear();
        GM.NowPlayerData.Position.SetVector(new Vector3(0f, 4.5f, 0f));
        GM.NowPlayerData.Rotation.SetVector(new Vector3(0f, 0f, 0f));
    }

    private void SetUserData()
    {
        for (int i = 0; i < 3; i++)
        {
            GM.NowUserData.CanGatherings.Add(false);
        }
    }
}