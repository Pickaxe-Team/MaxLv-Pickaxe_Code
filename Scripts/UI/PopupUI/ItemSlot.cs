using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public ItemData ItemData;

    public UIInventory Inventory;
    public Button Button;
    public Image Icon;
    public TextMeshProUGUI QuantityText;
    public GameObject Pointer;

    public int Index;

    public void Init(UIInventory uiInven,int idx)
    {
        Index = idx;
        Button = GetComponent<Button>();
        Button.onClick.AddListener(OnClickButton);

        Inventory = uiInven;
    }

    public void Set(KeyValuePair<ItemID, InventoryItemInfoData> data)
    {
        ItemData = GameManager.Instance.GetItemData(data.Key);
        Icon.gameObject.SetActive(true);
        Icon.sprite = ItemData.Image;
        QuantityText.text = data.Value.Amount > 1 ? data.Value.Amount.ToString() : string.Empty;
    }

    public void Clear()
    {
        ItemData = null;
        Icon.gameObject.SetActive(false);
        QuantityText.text = string.Empty;
        Pointer.SetActive(false);
    }

    public void OnClickButton()
    {
        //해당 슬롯을 눌렀을 때 인벤토리에 SelectItem()실행됨
        Inventory.SelectItem(Index);
        GameManager.Instance.PlaySFX(SFX.Click);
    }
}