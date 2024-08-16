using UnityEngine;

public class DungeonB : DungeonSpawner
{
    [Header("Ore")]
    [SerializeField] private PoolObject SilverOre;
    [SerializeField] private PoolObject GoldOre;
    [SerializeField] private PoolObject DiamondOre;

    [Header("Enemy")]
    [SerializeField] private PoolObject Orbinaut;
    [SerializeField] private PoolObject LavaOrbinaut;
    [SerializeField] private PoolObject GreenSlime;

    public override void AddObjectPool()
    {
        // Ore
        ObjectPool.AddObjectPool(SpawnObject.SilverOre, SilverOre, _maxOreAmount);
        ObjectPool.AddObjectPool(SpawnObject.GoldOre, GoldOre, _maxOreAmount);
        ObjectPool.AddObjectPool(SpawnObject.DiamondOre, DiamondOre, _maxOreAmount);

        // Item
        ObjectPool.AddObjectPool(SpawnObject.Silver, GameManager.Instance.GetItemData(ItemID.Silver).Prefab, _maxItemAmount);
        ObjectPool.AddObjectPool(SpawnObject.Gold, GameManager.Instance.GetItemData(ItemID.Gold).Prefab, _maxItemAmount);
        ObjectPool.AddObjectPool(SpawnObject.Diamond, GameManager.Instance.GetItemData(ItemID.Diamond).Prefab, _maxItemAmount);
        ObjectPool.AddObjectPool(SpawnObject.SilverScroll, GameManager.Instance.GetItemData(ItemID.SilverScroll).Prefab, _maxItemAmount);
        ObjectPool.AddObjectPool(SpawnObject.GoldScroll, GameManager.Instance.GetItemData(ItemID.GoldScroll).Prefab, _maxItemAmount);
        ObjectPool.AddObjectPool(SpawnObject.DiamondScroll, GameManager.Instance.GetItemData(ItemID.DiamondScroll).Prefab, _maxItemAmount);

        // Enemy
        ObjectPool.AddObjectPool(SpawnObject.Orbinaut, Orbinaut, _maxEnemyAmount);
        ObjectPool.AddObjectPool(SpawnObject.LavaOrbinaut, LavaOrbinaut, _maxEnemyAmount);
        ObjectPool.AddObjectPool(SpawnObject.GreenSlime, GreenSlime, _maxEnemyAmount);

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
            return SpawnObject.SilverOre;
        }
        else if (randomValue <= 70)
        {
            return SpawnObject.GoldOre;
        }
        else
        {
            return SpawnObject.DiamondOre;
        }
    }

    protected override string EnemyNameByRatio()
    {
        int randomValue = Random.Range(0, 101);

        if (randomValue <= 40)
        {
            return SpawnObject.Orbinaut;
        }
        else if (randomValue <= 70)
        {
            return SpawnObject.LavaOrbinaut;
        }
        else
        {
            return SpawnObject.GreenSlime;
        }
    }
}