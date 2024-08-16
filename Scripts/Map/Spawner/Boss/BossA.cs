using UnityEngine;

public class BossA : BossSpawner
{
    [Header("BossLaser")]
    [SerializeField] private PoolObject BossLaser;
    private int _maxBossLaserAmount = 20;

    [Header("BossMeteor")]
    [SerializeField] protected PoolObject BossMeteor;
    protected int _maxBossMeteorAmount = 20;

    public override void AddObjectPool()
    {
        // Laser
        ObjectPool.AddObjectPool(Tag.BossLaser, BossLaser, _maxBossLaserAmount);
        
        // Meteor
        ObjectPool.AddObjectPool(Tag.BossMeteor, BossMeteor, _maxBossMeteorAmount);
        
        // Ghost
        ObjectPool.AddObjectPool(Tag.Ghost, Ghost, _maxGhostAmount);

        // Bullet
        ObjectPool.AddObjectPool(Tag.Bullet, Bullet, _maxBulletAmount);
    }
}