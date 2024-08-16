using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using System;

[System.Serializable]
public class VectorElement
{
    public Vector3 GetVector()
    {
        return new Vector3 (x, y, z);
    }

    public void SetVector(Vector3 v)
    {
        x = v.x; 
        y = v.y;
        z = v.z;
    }

    public float x;
    public float y;
    public float z;
}

[System.Serializable]
public class PlayerData
{
    [Header("곡괭이")]
    public int PickaxeLV;

    [Header("플레이어 스탯")]
    public int LV;
    public float CurHP;
    public float CurEXP;

    [Header("플레이어 좌표")]
    public VectorElement Position = new VectorElement();
    public VectorElement Rotation = new VectorElement();// Quaternion.Euler -> Vector3

    [Header("인벤토리")]
    public Dictionary<ItemID, InventoryItemInfoData> Inventory = new Dictionary<ItemID, InventoryItemInfoData>();

    [Header("퀵슬롯")]
    public ItemID QuickSlotItemData;
    public bool HasQuickSlotItemData = false;
}

[System.Serializable]
public class InventoryItemInfoData
{
    public ItemID ID;
    public int Amount;
    public int OrderInInventory;
}

[System.Serializable]
public class MapData
{
    [Header("오브젝트")]
    public int Index = 0;
    public Dictionary<int, MapObject> InteractableObjects = new Dictionary<int, MapObject>();
}

[System.Serializable]
public class MapObject
{
    public VectorElement Position = new VectorElement();
    public string Tag;
}

[System.Serializable]
public class OptionData
{
    [Header("소리")]
    public float BGMVolume;
    public float SFXVolume;
}

[System.Serializable]
public class UserData
{
    [Header("자원별 마지막 수집 시간")]
    public Dictionary<ItemID, DateTime> LastCollectionTimes = new Dictionary<ItemID, DateTime>();

    [Header("클리어된 보스")]
    public HashSet<EntityID> ClearedBosses = new HashSet<EntityID>();

    [Header("자동 채집 해금 여부")]
    public List<bool> CanGatherings = new List<bool>();
}

public class SaveManager : MonoBehaviour
{
    public void SaveData<T>(T data)
    {
        string path = Path.Combine(Application.persistentDataPath, $"{typeof(T).Name}.json");
        string jsonData = JsonConvert.SerializeObject(data);
        File.WriteAllText(path, jsonData);
    }

    public bool TryLoadData<T>(out T data)
    {
        string path = Path.Combine(Application.persistentDataPath, $"{typeof(T).Name}.json");
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            data = JsonConvert.DeserializeObject<T>(jsonData);
            return true;
        }
        else
        {
            data = default(T);
            return false;
        }
    }
}
