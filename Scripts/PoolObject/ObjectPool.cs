using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public Dictionary<string, List<PoolObject>> PoolDictionary;

    private void Awake()
    {
        PoolDictionary = new Dictionary<string, List<PoolObject>>();
    }

    public PoolObject SpawnFromPool(string tag)
    {
        if (PoolDictionary.TryGetValue(tag, out List<PoolObject> list))
        {
            foreach (PoolObject obj in list)
            {
                if (!obj.gameObject.activeInHierarchy)
                {
                    obj.gameObject.SetActive(true);
                    return obj;
                }
            }
        }

        return null;
    }

    public void AddObjectPool(string tag, PoolObject prefab, int size)
    {
        List<PoolObject> objectPool = new List<PoolObject>();

        for (int i = 0; i < size; i++)
        {
            PoolObject obj = Instantiate(prefab, gameObject.transform);
            obj.gameObject.SetActive(false);
            objectPool.Add(obj);
        }

        PoolDictionary.Add(tag, objectPool);
    }
}