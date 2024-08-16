using UnityEngine;

public class DungeonC : DungeonSpawner
{
    [Header("Ore")]
    [SerializeField] private PoolObject GoldOre;
    [SerializeField] private PoolObject DiamondOre;
    [SerializeField] private PoolObject SapphireOre;

    [Header("Enemy")]
    [SerializeField] private PoolObject LavaOrbinaut;
    [SerializeField] private PoolObject GreenSlime;
    [SerializeField] private PoolObject BlueOrbinaut;

    public override void AddObjectPool()
    {
        // Ore
        ObjectPool.AddObjectPool(SpawnObject.GoldOre, GoldOre, _maxOreAmount);
        ObjectPool.AddObjectPool(SpawnObject.DiamondOre, DiamondOre, _maxOreAmount);
        ObjectPool.AddObjectPool(SpawnObject.SapphireOre, SapphireOre, _maxOreAmount);

        // Item
        ObjectPool.AddObjectPool(SpawnObject.Gold, GameManager.Instance.GetItemData(ItemID.Gold).Prefab, _maxItemAmount);
        ObjectPool.AddObjectPool(SpawnObject.Diamond, GameManager.Instance.GetItemData(ItemID.Diamond).Prefab, _maxItemAmount);
        ObjectPool.AddObjectPool(SpawnObject.Sapphire, GameManager.Instance.GetItemData(ItemID.Sapphire).Prefab, _maxItemAmount);
        ObjectPool.AddObjectPool(SpawnObject.GoldScroll, GameManager.Instance.GetItemData(ItemID.GoldScroll).Prefab, _maxItemAmount);
        ObjectPool.AddObjectPool(SpawnObject.DiamondScroll, GameManager.Instance.GetItemData(ItemID.DiamondScroll).Prefab, _maxItemAmount);
        ObjectPool.AddObjectPool(SpawnObject.SapphireScroll, GameManager.Instance.GetItemData(ItemID.SapphireScroll).Prefab, _maxItemAmount);

        // Enemy
        ObjectPool.AddObjectPool(SpawnObject.LavaOrbinaut, LavaOrbinaut, _maxEnemyAmount);
        ObjectPool.AddObjectPool(SpawnObject.GreenSlime, GreenSlime, _maxEnemyAmount);
        ObjectPool.AddObjectPool(SpawnObject.BlueOrbinaut, BlueOrbinaut, _maxEnemyAmount);

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
            return SpawnObject.GoldOre;
        }
        else if (randomValue <= 70)
        {
            return SpawnObject.DiamondOre;
        }
        else
        {
            return SpawnObject.SapphireOre;
        }
    }

    protected override string EnemyNameByRatio()
    {
        int randomValue = Random.Range(0, 101);

        if (randomValue <= 40)
        {
            return SpawnObject.LavaOrbinaut;
        }
        else if (randomValue <= 70)
        {
            return SpawnObject.GreenSlime;
        }
        else
        {
            return SpawnObject.BlueOrbinaut;
        }
    }
}