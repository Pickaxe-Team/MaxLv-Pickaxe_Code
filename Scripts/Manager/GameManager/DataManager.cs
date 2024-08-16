using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

#region DataClass

public class ItemData
{
    public ItemID ID;
    public string Name;
    public ItemType Type;
    public Sprite Image;
    public string Description;
    public PoolObject Prefab;
    public bool CanStack;
    public int MaxStackAmount;
}

public class ConsumableItemData : ItemData
{
    public float HealPercentage;
}

public class ItemInfoData
{
    public ItemID ID;
    public int Amount;
}

public class DropItemInfoData : ItemInfoData
{
    public float Probability;
}

public class EntityData
{
    public float HP;
    public float EXP;
    public List<DropItemInfoData> DropItems;
}

public class EnemyData : EntityData
{
    public EntityID ID;
    public string Name;
    public float ATK;
    public float AttackRange;
    public float PlayerChasingRange;
    public float Speed;
}

public class InteractableObjectData : EntityData
{
    public EntityID ID;
    public InteractableObjectType Type;
    public int LimitLV;
}

public class PickaxeData
{
    public PickaxeID ID;
    public string Name;
    public float ATK;
    public Sprite Image;
    public Grade Grade;
    public string Description;
    public List<ItemInfoData> Resources;
    public bool CanPickaxeAura;
    public bool CanPenetrationPickaxeAura;
    public int PickaxeAuraAmount;
    public float PickaxeAuraSize;
    public float PickaxeAuraSpeed;
    public float PickaxeAuraRange;
    public Sprite PickaxeAuraImage;
}
#endregion

public class DataManager : MonoBehaviour
{
    public PlayerSO PlayerData;
    public List<Color> GradeColor;
    public List<Color> HPColor;

    public void Initializer()
    {
        ContainConsumableItemData();
        ContainItemData();
        ContainPickaxeData();
        ContainInteractableObjectData();
        ContainEnemyData();
    }

    #region ItemData

    public Dictionary<ItemID, ItemData> ItemDatas = new Dictionary<ItemID, ItemData>();

    public void ContainItemData()
    {
        List<Dictionary<string, string>> resourceItemDataList = CSVReader.Read(ResourcesPath.ResourceItemCSV);

        foreach (var datas in resourceItemDataList)
        {
            ItemData resourceItemData = new ItemData();
            resourceItemData.ID = (ItemID)int.Parse(datas[Data.ID]);
            resourceItemData.Name = datas[Data.Name];
            resourceItemData.Type = (ItemType)int.Parse(datas[Data.Type]);
            resourceItemData.Image = Resources.Load<SpriteAtlas>(ResourcesPath.CSVSprites).GetSprite(datas[Data.Image]);
            resourceItemData.Description = datas[Data.Description];
            resourceItemData.Prefab = Resources.Load<PoolObject>(datas[Data.Prefab]);
            resourceItemData.CanStack = (int.Parse(datas[Data.CanStack]) == 1);
            resourceItemData.MaxStackAmount = int.Parse(datas[Data.MaxStackAmount]);
            ItemDatas.Add(resourceItemData.ID, resourceItemData);
        }
    }

    public void ContainConsumableItemData()
    {
        List<Dictionary<string, string>> consumableItemDataList = CSVReader.Read(ResourcesPath.ConsumableItemCSV);

        foreach (var datas in consumableItemDataList)
        {
            ConsumableItemData consumableItemData = new ConsumableItemData();
            consumableItemData.ID = (ItemID)int.Parse(datas[Data.ID]);
            consumableItemData.Name = datas[Data.Name];
            consumableItemData.Type = (ItemType)int.Parse(datas[Data.Type]);
            consumableItemData.Image = Resources.Load<SpriteAtlas>(ResourcesPath.CSVSprites).GetSprite(datas[Data.Image]);
            consumableItemData.Description = datas[Data.Description];
            consumableItemData.Prefab = Resources.Load<PoolObject>(datas[Data.Prefab]);
            consumableItemData.CanStack = (int.Parse(datas[Data.CanStack]) == 1);
            consumableItemData.MaxStackAmount = int.Parse(datas[Data.MaxStackAmount]);
            consumableItemData.HealPercentage = float.Parse(datas[Data.HealPercentage]);
            ItemDatas.Add(consumableItemData.ID, consumableItemData);
        }
    }

    #endregion

    #region EntityData

    public Dictionary<EntityID, EntityData> EntityDatas = new Dictionary<EntityID, EntityData>();

    public void ContainInteractableObjectData()
    {
        List<Dictionary<string, string>> interactableObjectDataList = CSVReader.Read(ResourcesPath.InteractableObjectCSV);

        foreach (var datas in interactableObjectDataList)
        {
            InteractableObjectData interactableObjectData = new InteractableObjectData();
            interactableObjectData.ID = (EntityID)int.Parse(datas[Data.ID]);
            interactableObjectData.HP = float.Parse(datas[Data.HP]);
            interactableObjectData.EXP = float.Parse(datas[Data.EXP]);
            interactableObjectData.DropItems = SplitDropItemDatas(datas[Data.DropItems]);
            interactableObjectData.Type = (InteractableObjectType)int.Parse(datas[Data.Type]);
            interactableObjectData.LimitLV = int.Parse(datas[Data.LimitLV]);
            EntityDatas.Add(interactableObjectData.ID, interactableObjectData);
        }
    }

    public void ContainEnemyData()
    {
        List<Dictionary<string, string>> enemyDataList = CSVReader.Read(ResourcesPath.EnemyCSV);

        foreach (var datas in enemyDataList)
        {
            EnemyData enemyData = new EnemyData();
            enemyData.ID = (EntityID)int.Parse(datas[Data.ID]);
            enemyData.Name = datas[Data.Name];
            enemyData.HP = float.Parse(datas[Data.HP]);
            enemyData.ATK = float.Parse(datas[Data.ATK]);
            enemyData.AttackRange = float.Parse(datas[Data.AttackRange]);
            enemyData.PlayerChasingRange = float.Parse(datas[Data.PlayerChasingRange]);
            enemyData.Speed = float.Parse(datas[Data.Speed]);
            enemyData.EXP = float.Parse(datas[Data.EXP]);
            enemyData.DropItems = SplitDropItemDatas(datas[Data.DropItems]);
            EntityDatas.Add(enemyData.ID, enemyData);
        }
    }

    #endregion

    #region PickaxeData

    public Dictionary<PickaxeID, PickaxeData> PickaxeDatas = new Dictionary<PickaxeID, PickaxeData>();

    public void ContainPickaxeData()
    {
        List<Dictionary<string, string>> pickaxeDataList = CSVReader.Read(ResourcesPath.PickaxeCSV);

        foreach (var datas in pickaxeDataList)
        {
            PickaxeData pickaxeData = new PickaxeData();
            pickaxeData.ID = (PickaxeID)int.Parse(datas[Data.ID]);
            pickaxeData.Name = datas[Data.Name];
            pickaxeData.ATK = float.Parse(datas[Data.ATK]);
            pickaxeData.Image = Resources.Load<SpriteAtlas>(ResourcesPath.CSVSprites).GetSprite(datas[Data.Image]);
            pickaxeData.Grade = (Grade)int.Parse(datas[Data.Grade]);
            pickaxeData.Description = datas[Data.Description];
            pickaxeData.Resources = SplitItemDatas(datas[Data.Resources]);
            pickaxeData.CanPickaxeAura = (int.Parse(datas[Data.CanPickaxeAura]) == 1);
            pickaxeData.CanPenetrationPickaxeAura = (int.Parse(datas[Data.CanPenetrationPickaxeAura]) == 1);
            pickaxeData.PickaxeAuraAmount = int.Parse(datas[Data.PickaxeAuraAmount]);
            pickaxeData.PickaxeAuraSize = float.Parse(datas[Data.PickaxeAuraSize]);
            pickaxeData.PickaxeAuraSpeed = float.Parse(datas[Data.PickaxeAuraSpeed]);
            pickaxeData.PickaxeAuraRange = float.Parse(datas[Data.PickaxeAuraRange]);
            pickaxeData.PickaxeAuraImage = Resources.Load<SpriteAtlas>(ResourcesPath.CSVSprites).GetSprite(datas[Data.PickaxeAuraImage]);
            PickaxeDatas.Add(pickaxeData.ID, pickaxeData);
        }
    }

    #endregion

    public List<DropItemInfoData> SplitDropItemDatas(string data)
    {
        if(data == "") return null;

        List<DropItemInfoData> itemDatalist = new List<DropItemInfoData>();

        string[] Items = data.Split('|');
        foreach (string Item in Items)
        {
            string[] itemInfo = Item.Split(':');
            DropItemInfoData itemData = new DropItemInfoData();
            itemData.ID = (ItemID)int.Parse(itemInfo[0]);
            itemData.Amount = int.Parse(itemInfo[1]);
            itemData.Probability = float.Parse(itemInfo[2]);
            itemDatalist.Add(itemData);
        }

        return itemDatalist;
    }

    public List<ItemInfoData> SplitItemDatas(string data)
    {
        if (data == "") return null;

        List<ItemInfoData> itemDatalist = new List<ItemInfoData>();

        string[] Items = data.Split('|');
        foreach (string Item in Items)
        {
            string[] itemInfo = Item.Split(':');
            ItemInfoData itemData = new ItemInfoData();
            itemData.ID = (ItemID)int.Parse(itemInfo[0]);
            itemData.Amount = int.Parse(itemInfo[1]);
            itemDatalist.Add(itemData);
        }

        return itemDatalist;
    }
}
