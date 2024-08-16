using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossPopup : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;

    [Header("UI")]
    [SerializeField] private Image NowBossImg;
    [SerializeField] private List<Sprite> BossImages;
    [SerializeField] private DungeonPopup DungeonPopup;

    private Vector3 _posOffset = new Vector3(0, -2f, 0);

    private void OnEnable()
    {
        NowBossImg.sprite = BossImages[GM.StageNum];
    }

    public void OnClickOK()
    {
        GM.SaveAllData(GM.Player.transform.position + _posOffset, GM.Player.transform.rotation);

        GM.PlaySFX(SFX.Click);

        SceneLoadManager.LoadScene(4);
    }

    public void OnClickCancle()
    {
        GM.StageNum = 0;
        DungeonPopup.UpdateIcon(GM.StageNum);
        gameObject.SetActive(false);

        GM.PlaySFX(SFX.Click);
    }
}
