using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;
    private Player Player => GM.Player;
    private PlayerData NowPlayerData => GameManager.Instance.NowPlayerData;
    private PickaxeData NowPickaxeData => GameManager.Instance.GetPickaxeData((PickaxeID)GameManager.Instance.NowPlayerData.PickaxeLV);

    [Header("Item Slot")]
    public Transform SlotPanel;
    public ItemSlot[] Slots;

    [Header("Selected Item")]
    public ItemSlot SelectedItem;
    public TextMeshProUGUI SelectedItemName;
    public TextMeshProUGUI SelectedItemDescription;
    public GameObject UseButton;
    public GameObject QuickButton;
    public GameObject DropButton;

    [Header("Pickaxe")]
    public Image PickaxeImage;
    public TextMeshProUGUI PickaxeStatName;
    public TextMeshProUGUI PickaxeStatDescription;
    public TextMeshProUGUI PickaxeATK;
    public Outline PickaxeOutline;

    public QuickSlotUI QuickSlotUI;
    private HealthSystem _healthSystem;

    private void OnEnable()
    {
        SelectedItemName.gameObject.SetActive(false);
        SelectedItemDescription.gameObject.SetActive(false);

        // 인벤토리 UI 초기화
        UpdateUI();
        // 곡괭이 UI 초기화
        UpdatePickaxeUI();
        // 아이템 정보 UI 초기화
        ClearSelectedItemWindow();
    }

    #region 인벤토리 초기화
    public void InitInventory()
    {
        _healthSystem = Player.HealthSystem;
        Slots = new ItemSlot[SlotPanel.childCount];

        // 슬롯 초기화
        for (int i = 0; i < Slots.Length; i++)
        {
            Slots[i] = SlotPanel.GetChild(i).GetComponent<ItemSlot>();
            Slots[i].Init(this, i);
            Slots[i].Clear();
        }
    }
    #endregion

    #region 인벤토리 UI 업데이트
    public void UpdateUI()
    {
        // 슬롯 초기화
        for (int i = 0; i < Slots.Length; i++)
        {
            Slots[i].Clear();
        }

        // 각 슬롯에 인벤토리 데이터 할당
        foreach(KeyValuePair<ItemID, InventoryItemInfoData> data in NowPlayerData.Inventory)
        {
            Slots[data.Value.OrderInInventory].Set(data);
        }
    }
    #endregion

    #region 아이템 선택(클릭)
    public void SelectItem(int index)
    {
        if (Slots[index].ItemData == null) return;
        if (SelectedItem != null) SelectedItem.Pointer.SetActive(false);

        SelectedItem = Slots[index];
        SelectedItem.Pointer.SetActive(true);

        SelectedItemName.gameObject.SetActive(true);
        SelectedItemDescription.gameObject.SetActive(true);

        SelectedItemName.text = SelectedItem.ItemData.Name;
        SelectedItemDescription.text = SelectedItem.ItemData.Description;

        UseButton.SetActive(SelectedItem.ItemData.Type == ItemType.Consumable);
        QuickButton.SetActive(SelectedItem.ItemData.Type == ItemType.Consumable);
        DropButton.SetActive(true);
    }
    #endregion

    #region 하단 아이템 정보 패널 초기화
    private void ClearSelectedItemWindow()
    {
        SelectedItem = null;

        SelectedItemName.text = string.Empty;
        SelectedItemDescription.text = string.Empty;

        UseButton.SetActive(false);
        QuickButton.SetActive(false);
        DropButton.SetActive(false);
    }
    #endregion

    #region 인벤토리 버튼들
    public void OnUseButton()
    {
        ConsumableItemData consumableItemData = SelectedItem.ItemData as ConsumableItemData;

        if(Player.Heal(Player.MaxHP * consumableItemData.HealPercentage))
        {
            ReduceItem(SelectedItem.ItemData.ID);

            QuickSlotUI.UpdateUI();
        }
    }

    public void OnQuickButton()
    {
        NowPlayerData.QuickSlotItemData = SelectedItem.ItemData.ID;
        NowPlayerData.HasQuickSlotItemData = true;
        GM.PlaySFX(SFX.Click);

        QuickSlotUI.UpdateQuickSlot();
    }

    public void OnDropButton()
    {
        GM.PlaySFX(SFX.Click);
        RemoveItem();
    }
    #endregion

    #region 아이템 감소 및 제거
    // 아이템 감소
    public void ReduceItem(ItemID itemID)
    {
        // 인벤토리 데이터에서 아이템 수량 감소
        NowPlayerData.Inventory[itemID].Amount--;

        // 아이템 수량이 0 이하면 아이템 제거
        if (NowPlayerData.Inventory[itemID].Amount <= 0)
        {
            OrderSet(NowPlayerData.Inventory[itemID].OrderInInventory);
            NowPlayerData.Inventory.Remove(itemID);

            // 선택 아이템 및 아이템 정보 UI 업데이트
            if (SelectedItem != null) SelectedItem.Clear();
            ClearSelectedItemWindow();
        }

        // 인벤토리 UI 업데이트
        UpdateUI();
    }

    // 아이템 제거
    private void RemoveItem()
    {
        // 인벤토리 데이터에서 아이템 제거
        OrderSet(NowPlayerData.Inventory[SelectedItem.ItemData.ID].OrderInInventory);
        NowPlayerData.Inventory.Remove(SelectedItem.ItemData.ID);

        // 선택 아이템 및 아이템 정보 UI 업데이트
        SelectedItem.Clear();
        ClearSelectedItemWindow();

        // 인벤토리 UI 업데이트
        UpdateUI();
    }

    // 모든 아이템 제거
    public void RemoveAllItem()
    {
        NowPlayerData.Inventory.Clear();

        //데이터 처리 더 해야할 경우 사용
        //ItemSlot targetSlot = Slots[0];

        //while (targetSlot.ItemSO != null)
        //{
        //    SelectedItem = targetSlot;
        //    RemoveItem();
        //}
    }
    #endregion

    #region 인벤토리 데이터 정렬
    private void OrderSet(int removeOrder)
    {
        foreach(var item in GameManager.Instance.NowPlayerData.Inventory)
        {
            if(item.Value.OrderInInventory > removeOrder)
            {
                item.Value.OrderInInventory--;
            }
        }
    }
    #endregion

    #region 곡괭이 UI 업데이트
    private void UpdatePickaxeUI()
    {
            PickaxeStatName.text = NowPickaxeData.Name;
            PickaxeATK.text = "공격력: " + NowPickaxeData.ATK.ToString();
            PickaxeImage.sprite = NowPickaxeData.Image;
            PickaxeStatDescription.text = NowPickaxeData.Description;
            PickaxeOutline.effectColor = GM.GetGradeColor(NowPickaxeData.Grade);
    }
    #endregion

    #region 인벤토리 활성화 토글
    public void Toggle()
    {
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
            GM.PlaySFX(SFX.Click);
        }
        else
        {
            gameObject.SetActive(true);
            GM.PlaySFX(SFX.Click);
        }
    }
    #endregion
}
