using UnityEngine;

public abstract class BossSpawner : MonoBehaviour
{
    protected ObjectPool ObjectPool => BossSceneManager.Instance.ObjectPool;

    [Header("Ghost")]
    [SerializeField] protected PoolObject Ghost;
    protected int _maxGhostAmount = 10;

    [Header("Bullet")]
    [SerializeField] protected PoolObject Bullet;
    protected int _maxBulletAmount = 20;

    public abstract void AddObjectPool();
}