using System.Collections.Generic;
using UnityEngine;

public interface IItemDropperable
{
    EntityData ReturnMyEntity();
}

public interface IObjectPool
{
    ObjectPool ReturnMyObjectPool();
}

public class DropSystem : MonoBehaviour
{
    private List<DropItemInfoData> DropItems => _entityData.DropItems;

    private EntityData _entityData;
    private ObjectPool _objectPool;
    private HealthSystem _healthSystem;

    private void Start()
    {
        _entityData = GetComponent<IItemDropperable>().ReturnMyEntity();
        _objectPool = GetComponent<IObjectPool>().ReturnMyObjectPool();

        _healthSystem = GetComponent<HealthSystem>();
        _healthSystem.OnDeath += DropItem;
    }

    public void DropItem()
    {
        for (int i = 0; i < DropItems.Count; i++)
        {
            if (Random.value <= DropItems[i].Probability)
            {
                int amount = Random.Range(1, DropItems[i].Amount + 1);

                float minAngle = 0f;
                float referenceAngle = 360f / amount;
                float maxAngle = referenceAngle;
                for (int j = 0; j < amount; j++)
                {
                    Vector3 origin = new Vector3(Random.Range(1f, 2f), 0f, 0f);
                    Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(minAngle, maxAngle)); // z축을 기준으로 회전
                    origin = rotation * origin;

                    PoolObject obj = _objectPool.SpawnFromPool(DropItems[i].ID.ToString());
                    obj.gameObject.transform.position = transform.position;
                    obj.ReturnMyComponent<ItemObject>().DropEffect(transform.position + origin);

                    minAngle += referenceAngle;
                    maxAngle += referenceAngle;
                }
            }
        }
    }
}
