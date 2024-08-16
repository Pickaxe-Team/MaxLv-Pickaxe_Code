using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AutomationNPC : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;
    private Dictionary<ItemID, InventoryItemInfoData> Inventory => GameManager.Instance.NowPlayerData.Inventory;
    private UserData NowUserData => GameManager.Instance.NowUserData;

    [Header("Data")]
    [SerializeField] private ItemID WoodItemID;
    [SerializeField] private ItemID StoneItemID;
    [SerializeField] private ItemID MineralItemID;
    private int _first = 0;
    private int _second = 1;
    private int _third = 2;

    [Header("Image")]
    [SerializeField] private Image WoodCharacter;
    [SerializeField] private Image StoneCharacter;
    [SerializeField] private Image MineralCharacter;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI CollectedWood;
    [SerializeField] private TextMeshProUGUI CollectedStones;
    [SerializeField] private TextMeshProUGUI CollectedMineral;

    public GameObject AutomationPanel;

    private float _treeResourcesCount = 0.1f;
    private float _rockResourcesCount = 0.1f;
    private float _mineralResourcesCount = 0.1f;

    private int _previousPickaxeLevel;

    private void Start()
    {
        AutomationPanel.SetActive(true);
        _previousPickaxeLevel = GameManager.Instance.NowPlayerData.PickaxeLV;
        // 이미지와 텍스트 초기화
        InitializeCharacterImagesAndTexts();
        // 보스 클리어 상태 초기화
        CheckBossClearedStatus();
        AutomationPanel.SetActive(false);
    }

    private void InitializeCharacterImagesAndTexts()
    {
        InitializeResourceImageAndText(WoodCharacter, CollectedWood, NowUserData.CanGatherings[_first], "은곡괭이 제작시");
        InitializeResourceImageAndText(StoneCharacter, CollectedStones, NowUserData.CanGatherings[_second], "첫번째 보스 처치시");
        InitializeResourceImageAndText(MineralCharacter, CollectedMineral, NowUserData.CanGatherings[_third], "두번째 보스 처치시");
    }

    private void InitializeResourceImageAndText(Image characterImage, TextMeshProUGUI collectedText, bool isConditionMet, string unlockCondition)
    {
        if (!isConditionMet)
        {
            characterImage.color = Color.black;
            collectedText.text = $"{unlockCondition} \n해금";
        }
    }
    private void Update()
    {
        CheckLevelUp();
        UpdateCollectedResourcesText();
        CheckBossClearedStatus();
    }

    private void CheckLevelUp()
    {
        int currentPickaxeLevel = GameManager.Instance.NowPlayerData.PickaxeLV;
        if (currentPickaxeLevel > _previousPickaxeLevel)
        {
            HandleLevelUp(_previousPickaxeLevel);
            _previousPickaxeLevel = currentPickaxeLevel;
        }
    }

    private void HandleLevelUp(int previousLevel)
    {
        ItemID previousOreItemID = GetOreItemIDForLevel(previousLevel);
        ReceiveResource(previousOreItemID, _mineralResourcesCount, NowUserData.CanGatherings[_third]);

        int currentPickaxeLevel = GameManager.Instance.NowPlayerData.PickaxeLV;
        ItemID currentOreItemID = GetOreItemIDForLevel(currentPickaxeLevel);
        NowUserData.LastCollectionTimes[currentOreItemID] = DateTime.Now;
        UpdateCollectedResourcesText();
    }

    private void CheckBossClearedStatus()
    {
        if (!NowUserData.CanGatherings[_first] && GM.NowPlayerData.PickaxeLV >= 4)
        {
            SetPlayerLevel();
        }
        if (!NowUserData.CanGatherings[_second] && NowUserData.ClearedBosses.Contains(EntityID.Golem))
        {
            SetFirstBossCleared();
        }
        if (!NowUserData.CanGatherings[_third] && NowUserData.ClearedBosses.Contains(EntityID.Golem2))
        {
            SetSecondBossCleared();
        }
    }

    private int CalculateResource(ItemID itemID, float collectionRate, bool collectionCondition)
    {
        if (!collectionCondition)
        {
            return 0;
        }

        int collected = Mathf.Max(0, Mathf.FloorToInt(GM.GetElapsedTime(itemID) * collectionRate));

        return collected;
    }

    private void UpdateCollectedResourcesText()
    {
        UpdateResourceText(WoodItemID, _treeResourcesCount, NowUserData.CanGatherings[_first], CollectedWood, "나무");
        UpdateResourceText(StoneItemID, _rockResourcesCount, NowUserData.CanGatherings[_second], CollectedStones, "돌");
        UpdateResourceText(GetCurrentOreItemID(), _mineralResourcesCount, NowUserData.CanGatherings[_third], CollectedMineral, "광석");
    }

    private void UpdateResourceText(ItemID itemID, float collectionRate, bool isBossCleared, TextMeshProUGUI collectedText, string resourceName)
    {
        if (isBossCleared)
        {
            int collected = CalculateResource(itemID, collectionRate, isBossCleared);
            collectedText.text = $"{resourceName} <color=green>{collected}</color>개 수집";
        }
    }

    public void ShowAutomationPanel()
    {
        if (AutomationPanel.activeInHierarchy)
        {
            AutomationPanel.SetActive(false);
            GM.PlaySFX(SFX.Click);
        }
        else
        {
            AutomationPanel.SetActive(true);
            CheckBossClearedStatus();
            UpdateCollectedResourcesText();
            GM.PlaySFX(SFX.Click);
        }
    }

    private void ReceiveResource(ItemID itemID, float collectionRate, bool collectionCondition)
    {
        int collected = CalculateResource(itemID, collectionRate, collectionCondition);
        if (collected > 0)
        {
            AddInventory(itemID, collected);
            NowUserData.LastCollectionTimes[itemID] = DateTime.Now;
            UpdateCollectedResourcesText();
        }
    }

    public void ReceiveWood()
    {
        if (!NowUserData.CanGatherings[_first])
        {
            GM.ShowAlert("은곡괭이를 만드십시오");
        }
        else
        {
            ReceiveResource(WoodItemID, _treeResourcesCount, NowUserData.CanGatherings[_first]);
        }
    }

    public void ReceiveStone()
    {
        if (!NowUserData.CanGatherings[_second])
        {
            GM.ShowAlert("보스를 처치하십시오");
        }
        else
        {
            ReceiveResource(StoneItemID, _rockResourcesCount, NowUserData.CanGatherings[_second]);
        }
    }

    public void ReceiveOre()
    {
        if (!NowUserData.CanGatherings[_third])
        {
            GM.ShowAlert("보스를 처치하십시오");
        }
        else
        {
            ItemID currentOreItemID = GetCurrentOreItemID();
            ReceiveResource(currentOreItemID, _mineralResourcesCount, NowUserData.CanGatherings[_third]);
        }
    }

    private ItemID GetCurrentOreItemID()
    {
        int pickaxeLevel = GameManager.Instance.NowPlayerData.PickaxeLV;
        return GetOreItemIDForLevel(pickaxeLevel);
    }

    private ItemID GetOreItemIDForLevel(int level)
    {
        int baseMineralID = (int)MineralItemID;
        int increment = (level - 1) / 3;
        ItemID oreItemID = (ItemID)(baseMineralID + increment);
        return oreItemID;
    }

    private void AddInventory(ItemID itemID, int amount)
    {
        if (Inventory.ContainsKey(itemID))
        {
            Inventory[itemID].Amount = Mathf.Max(0, Inventory[itemID].Amount + amount);
        }
        else
        {
            if (Inventory.Count == GM.UIInventory.Slots.Length)
            {
                GM.ShowAlert("인벤토리가 가득 찼습니다!");
                return;
            }
            else
            {
                InventoryItemInfoData data = new InventoryItemInfoData
                {
                    ID = itemID,
                    Amount = amount,
                    OrderInInventory = Inventory.Count
                };
                Inventory.Add(itemID, data);
            }
        }
    }

    private void SetPlayerLevel()
    {
        NowUserData.CanGatherings[_first] = true;
        UpdateCharacterImageAndText(WoodCharacter, CollectedWood);
        GM.NowUserData.LastCollectionTimes.Add(WoodItemID, DateTime.Now);
    }

    private void SetFirstBossCleared()
    {
        NowUserData.CanGatherings[_second] = true;
        UpdateCharacterImageAndText(StoneCharacter, CollectedStones);
        GM.NowUserData.LastCollectionTimes.Add(StoneItemID, DateTime.Now);
    }

    private void SetSecondBossCleared()
    {
        NowUserData.CanGatherings[_third] = true;
        UpdateCharacterImageAndText(MineralCharacter, CollectedMineral);
        GM.NowUserData.LastCollectionTimes.Add(MineralItemID, DateTime.Now);
    }

    private void UpdateCharacterImageAndText(Image characterImage, TextMeshProUGUI collectedText)
    {
        characterImage.color = Color.white;
        UpdateCollectedResourcesText();
    }
}
