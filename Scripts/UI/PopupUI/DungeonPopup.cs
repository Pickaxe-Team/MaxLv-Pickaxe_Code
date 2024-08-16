using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonPopup : MonoBehaviour
{
    [Serializable]
    public class IconImage
    {
        public List<Sprite> IconImages;
    }

    private GameManager GM => GameManager.Instance;

    [Header("UI")]
    [SerializeField] private GameObject BossPopup;
    [SerializeField] private List<Image> Icons;
    [SerializeField] private List<IconImage> DungeonIcons;

    private Vector3 _posOffset = new Vector3(0, -2f, 0);
    private readonly int _initNum = 0;

    private void OnEnable()
    {
        GM.StageNum = _initNum;
        UpdateIcon(_initNum);
    }

    public void OnClickDungeon(int stageNum)
    {
        GM.StageNum = stageNum;
        GM.PlaySFX(SFX.Click);

        UpdateIcon(stageNum);
    }

    public void UpdateIcon(int stageNum)
    {
        for (int i = 0; i < Icons.Count; i++)
        {
            Icons[i].sprite = DungeonIcons[stageNum].IconImages[i];
        }
    }

    public void OnClickBoss(int stageNum)
    {
        GM.StageNum = stageNum;
        GM.PlaySFX(SFX.Click);

        BossPopup.SetActive(true);
    }

    public void OnClickEnter()
    {
        GM.SaveAllData(GM.Player.transform.position + _posOffset, GM.Player.transform.rotation);

        GM.PlaySFX(SFX.Click);

        SceneLoadManager.LoadScene(3);
    }
}
