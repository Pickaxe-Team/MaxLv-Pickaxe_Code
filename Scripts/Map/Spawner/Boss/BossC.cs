using UnityEngine;

public class BossC : BossSpawner
{
    [Header("BossBullet")]
    [SerializeField] private PoolObject BossBullet;
    private int _maxBossBulletAmount = 100;

    [Header("BossSummon")]
    [SerializeField] private PoolObject BossSummon;
    private int _maxBossSummonAmount = 20;

    public override void AddObjectPool()
    {
        // BossBullet
        ObjectPool.AddObjectPool(Tag.BossBullet, BossBullet, _maxBossBulletAmount);

        // BossSummon
        ObjectPool.AddObjectPool(Tag.BossSummon, BossSummon, _maxBossSummonAmount);

        // Ghost
        ObjectPool.AddObjectPool(Tag.Ghost, Ghost, _maxGhostAmount);

        // Bullet
        ObjectPool.AddObjectPool(Tag.Bullet, Bullet, _maxBulletAmount);
    }
}