using UnityEngine;

public class DungeonD : DungeonSpawner
{
    [Header("Ore")]
    [SerializeField] private PoolObject DiamondOre;
    [SerializeField] private PoolObject SapphireOre;
    [SerializeField] private PoolObject AmethystOre;

    [Header("Enemy")]
    [SerializeField] private PoolObject GreenSlime;
    [SerializeField] private PoolObject BlueOrbinaut;
    [SerializeField] private PoolObject PinkSlime;

    public override void AddObjectPool()
    {
        // Ore
        ObjectPool.AddObjectPool(SpawnObject.DiamondOre, DiamondOre, _maxOreAmount);
        ObjectPool.AddObjectPool(SpawnObject.SapphireOre, SapphireOre, _maxOreAmount);
        ObjectPool.AddObjectPool(SpawnObject.AmethystOre, AmethystOre, _maxOreAmount);

        // Item
        ObjectPool.AddObjectPool(SpawnObject.Diamond, GameManager.Instance.GetItemData(ItemID.Diamond).Prefab, _maxItemAmount);
        ObjectPool.AddObjectPool(SpawnObject.Sapphire, GameManager.Instance.GetItemData(ItemID.Sapphire).Prefab, _maxItemAmount);
        ObjectPool.AddObjectPool(SpawnObject.Amethyst, GameManager.Instance.GetItemData(ItemID.Amethyst).Prefab, _maxItemAmount);
        ObjectPool.AddObjectPool(SpawnObject.DiamondScroll, GameManager.Instance.GetItemData(ItemID.DiamondScroll).Prefab, _maxItemAmount);
        ObjectPool.AddObjectPool(SpawnObject.SapphireScroll, GameManager.Instance.GetItemData(ItemID.SapphireScroll).Prefab, _maxItemAmount);
        ObjectPool.AddObjectPool(SpawnObject.AmethystScroll, GameManager.Instance.GetItemData(ItemID.AmethystScroll).Prefab, _maxItemAmount);

        // Enemy
        ObjectPool.AddObjectPool(SpawnObject.GreenSlime, GreenSlime, _maxEnemyAmount);
        ObjectPool.AddObjectPool(SpawnObject.BlueOrbinaut, BlueOrbinaut, _maxEnemyAmount);
        ObjectPool.AddObjectPool(SpawnObject.PinkSlime, PinkSlime, _maxEnemyAmount);

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
            return SpawnObject.DiamondOre;
        }
        else if (randomValue <= 70)
        {
            return SpawnObject.SapphireOre;
        }
        else
        {
            return SpawnObject.AmethystOre;
        }
    }

    protected override string EnemyNameByRatio()
    {
        int randomValue = Random.Range(0, 101);

        if (randomValue <= 40)
        {
            return SpawnObject.GreenSlime;
        }
        else if (randomValue <= 70)
        {
            return SpawnObject.BlueOrbinaut;
        }
        else
        {
            return SpawnObject.PinkSlime;
        }
    }
}