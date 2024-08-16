using UnityEngine;

public class BossB : BossSpawner
{
    [Header("BossBullet")]
    [SerializeField] private PoolObject BossBullet;
    private int _maxBossBulletAmount = 100;

    public override void AddObjectPool()
    {
        // BossBullet
        ObjectPool.AddObjectPool(Tag.BossBullet, BossBullet, _maxBossBulletAmount);

        // Ghost
        ObjectPool.AddObjectPool(Tag.Ghost, Ghost, _maxGhostAmount);

        // Bullet
        ObjectPool.AddObjectPool(Tag.Bullet, Bullet, _maxBulletAmount);
    }
}