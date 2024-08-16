using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class QuickSlotUI : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;
    private Player Player => GM.Player;
    private Dictionary<ItemID, InventoryItemInfoData> Inventory => GameManager.Instance.NowPlayerData.Inventory;
    private HealthSystem HealthSystem => GameManager.Instance.Player.HealthSystem;

    [Header("QuickSlotItem")]
    public ConsumableItemData ConsumableItemData;
    public Image ItemImage;
    public TextMeshProUGUI AmountText;

    private void Start()
    {
        UpdateQuickSlot();
        GM.Player.Input.OnConsumableInput.AddListener(OnClickButton);
    }


    #region 퀵슬롯 업데이트

    public void UpdateQuickSlot()
    {
        UpdateData();
        UpdateUI();
    }

    #endregion

    #region 퀵슬롯 데이터 업데이트

    private void UpdateData()
    {
        // 저장된 퀵슬롯 아이템 데이터가 있는 경우
        if (GM.NowPlayerData.HasQuickSlotItemData)
        {
            // 아이템 ID로 아이템 데이터 가져오기 
            ConsumableItemData = GM.GetItemData(GM.NowPlayerData.QuickSlotItemData) as ConsumableItemData;
        }
        else
        {
            ConsumableItemData = null;
        }
    }

    #endregion

    #region 퀵슬롯 UI 업데이트

    public void UpdateUI()
    {
        // 저장된 퀵슬롯 아이템 데이터가 있는 경우
        if (GM.NowPlayerData.HasQuickSlotItemData)
        {
            ItemImage.sprite = ConsumableItemData.Image;
            ItemImage.gameObject.SetActive(true);
            AmountText.text = Inventory.ContainsKey(GM.NowPlayerData.QuickSlotItemData) ? Inventory[GM.NowPlayerData.QuickSlotItemData].Amount.ToString() : "0";
        }
        else
        {
            ItemImage.gameObject.SetActive(GM.NowPlayerData.HasQuickSlotItemData);
            AmountText.text = string.Empty;
        }
    }

    #endregion

    #region 퀵슬롯 버튼 클릭 시

    public void OnClickButton()
    {
        // 저장된 퀵슬롯 아이템 데이터가 있고,
        if (GM.NowPlayerData.HasQuickSlotItemData)
        {
            // 인벤토리에 해당 아이템이 있는 경우
            if (GM.NowPlayerData.Inventory.ContainsKey(GM.NowPlayerData.QuickSlotItemData))
            {
                if(Player.Heal(Player.MaxHP * ConsumableItemData.HealPercentage))
                {
                    // 인벤토리 업데이트
                    GM.UIInventory.ReduceItem(GM.NowPlayerData.QuickSlotItemData);

                    // 퀵슬롯 UI 업데이트
                    UpdateUI();
                }                
            }
            else
            {
                // 인벤토리에 해당 아이템이 없는 경우
                GM.ShowAlert("사용할 아이템이 부족합니다!");
            }
        }
        else
        {
            // 저장된 퀵슬롯 아이템 데이터가 없는 경우
            GM.ShowAlert("사용할 아이템을 등록해주세요!");
        }
    }
    
    #endregion
}
