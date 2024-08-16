using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;
    private PlayerData NowPlayerData => GameManager.Instance.NowPlayerData;
    private PickaxeData NowPickaxeData => GameManager.Instance.GetPickaxeData((PickaxeID)GameManager.Instance.NowPlayerData.PickaxeLV);
    private PickaxeData NextPickaxeData => GameManager.Instance.GetPickaxeData((PickaxeID)(GameManager.Instance.NowPlayerData.PickaxeLV + 1));


    [SerializeField] private List<RequireResourceSlot> Slots;
    private List<ItemInfoData> _requireResources;
    private bool _enoughResource;

    [SerializeField] private TextMeshProUGUI NowPickaxeNameText;
    [SerializeField] private TextMeshProUGUI NextPickaxeNameText;

    [SerializeField] private Image PickaxeImage;
    [SerializeField] private Outline PickaxeOutline;
    [SerializeField] private TextMeshProUGUI PickaxeAtkText;

    private void OnEnable()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        _enoughResource = true;
        NowPickaxeNameText.text = NowPickaxeData.Name;
        PickaxeImage.sprite = NowPickaxeData.Image;
        PickaxeOutline.effectColor = GM.GetGradeColor(NowPickaxeData.Grade);

        if (NowPlayerData.PickaxeLV + 1 <= (int)PickaxeID.End - 1)
        {
            PickaxeAtkText.text = $"공격력: {NowPickaxeData.ATK} <color=green>+ {NextPickaxeData.ATK}</color>";
            _requireResources = NextPickaxeData.Resources;
            NextPickaxeNameText.text = NextPickaxeData.Name;
        }
        else
        {
            PickaxeAtkText.text = $"공격력: {NowPickaxeData.ATK}";
            NextPickaxeNameText.text = "<color=red>Max Level</color>";
            for (int i = 0; i < Slots.Count; i++)
            {
                Slots[i].gameObject.SetActive(false);
            }
            return;
        }

        for (int i = 0; i < Slots.Count; i++)
        {
            if(i < _requireResources.Count)
            {
                Slots[i].gameObject.SetActive(true);
                SetResourceSlot(Slots[i], _requireResources[i], NowPlayerData.Inventory);
            }
            else
            {
                Slots[i].gameObject.SetActive(false);
            }
        }
    }

    public void SetResourceSlot(RequireResourceSlot slot, ItemInfoData requireResource, Dictionary<ItemID, InventoryItemInfoData> inventory)
    {
        ItemData item = GM.GetItemData(requireResource.ID);

        slot.Icon.sprite = item.Image;

        int hasItemAmonut;
        if(!inventory.ContainsKey(requireResource.ID))
        {
            hasItemAmonut = 0;
        }
        else
        {
            hasItemAmonut = inventory[requireResource.ID].Amount;
        }

        string color;
        if(hasItemAmonut < requireResource.Amount)
        {
            color = "red";
            _enoughResource = false;
        }
        else
        {
            color = "white";
        }

        slot.Text.text = $"{item.Name} : <color={color}>{hasItemAmonut}</color> / {requireResource.Amount}";
    }

    public void UpgradeButton()
    {
        if(NowPlayerData.PickaxeLV + 1 <= (int)PickaxeID.End - 1)
        {
            if (_enoughResource)
            {
                foreach (var requireResource in _requireResources)
                {
                    NowPlayerData.Inventory[requireResource.ID].Amount -= requireResource.Amount;
                }

                GM.PlaySFX(SFX.Upgrade);
                NowPlayerData.PickaxeLV++;

                PickAxeController pickAxeController = GM.Player.GetComponentInChildren<PickAxeController>();
                pickAxeController.UpdateSpriteByLevel();

                UpdateUI();

                GM.SaveAllData(GM.Player.transform.position, GM.Player.transform.rotation);
            }
            else
            {
                GM.ShowAlert("재료가 부족합니다");
            }
        }
        else
        {
            GM.ShowAlert("이미 최고 레벨입니다.");
        }
    }
}
