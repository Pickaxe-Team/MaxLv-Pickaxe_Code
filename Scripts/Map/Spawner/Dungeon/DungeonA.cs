using UnityEngine;

public class DungeonA : DungeonSpawner
{
    [Header("Ore")]
    [SerializeField] private PoolObject CopperOre;
    [SerializeField] private PoolObject SilverOre;
    [SerializeField] private PoolObject GoldOre;

    [Header("Enemy")]
    [SerializeField] private PoolObject BlueSlime;
    [SerializeField] private PoolObject Orbinaut;
    [SerializeField] private PoolObject LavaOrbinaut;

    public override void AddObjectPool()
    {
        // Ore
        ObjectPool.AddObjectPool(SpawnObject.CopperOre, CopperOre, _maxOreAmount);
        ObjectPool.AddObjectPool(SpawnObject.SilverOre, SilverOre, _maxOreAmount);
        ObjectPool.AddObjectPool(SpawnObject.GoldOre, GoldOre, _maxOreAmount);

        // Item
        ObjectPool.AddObjectPool(SpawnObject.Copper, GameManager.Instance.GetItemData(ItemID.Copper).Prefab, _maxItemAmount);
        ObjectPool.AddObjectPool(SpawnObject.Silver, GameManager.Instance.GetItemData(ItemID.Silver).Prefab, _maxItemAmount);
        ObjectPool.AddObjectPool(SpawnObject.Gold, GameManager.Instance.GetItemData(ItemID.Gold).Prefab, _maxItemAmount);
        ObjectPool.AddObjectPool(SpawnObject.CopperScroll, GameManager.Instance.GetItemData(ItemID.CopperScroll).Prefab, _maxItemAmount);
        ObjectPool.AddObjectPool(SpawnObject.SilverScroll, GameManager.Instance.GetItemData(ItemID.SilverScroll).Prefab, _maxItemAmount);
        ObjectPool.AddObjectPool(SpawnObject.GoldScroll, GameManager.Instance.GetItemData(ItemID.GoldScroll).Prefab, _maxItemAmount);

        // Enemy
        ObjectPool.AddObjectPool(SpawnObject.BlueSlime, BlueSlime, _maxEnemyAmount);
        ObjectPool.AddObjectPool(SpawnObject.Orbinaut, Orbinaut, _maxEnemyAmount);
        ObjectPool.AddObjectPool(SpawnObject.LavaOrbinaut, LavaOrbinaut, _maxEnemyAmount);

        // Ghost
        ObjectPool.AddObjectPool(Tag.Ghost, Ghost, _maxGhostAmount);

        // Bullet
        ObjectPool.AddObjectPool(Tag.Bullet, Bullet, _maxBulletAmount);
    }

    protected override string OreNameByRatio()
    {
        int randomValue = Random.Range(0, 101);

        if (randomValue <= 40)
        {
            return SpawnObject.CopperOre;
        }
        else if (randomValue <= 70)
        {
            return SpawnObject.SilverOre;
        }
        else
        {
            return SpawnObject.GoldOre;
        }
    }

    protected override string EnemyNameByRatio()
    {
        int randomValue = Random.Range(0, 101);

        if (randomValue <= 40)
        {
            return SpawnObject.BlueSlime;
        }
        else if (randomValue <= 70)
        {
            return SpawnObject.Orbinaut;
        }
        else
        {
            return SpawnObject.LavaOrbinaut;
        }
    }
}