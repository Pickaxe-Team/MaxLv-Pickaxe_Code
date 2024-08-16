using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boss1Patterns : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;

    [SerializeField] private Transform FirePoint;
    [SerializeField] private Transform RotatingObject;
    public float RotationSpeed = 50f;
    public float ShootingInterval = 2f;
    public float RandomFireDuration = 5f;
    public float RandomFireRate = 1f;
    public float MeteorFallSpeed = 5f;
    private ObjectPool _objectPool;
    private delegate void ShootPattern();
    private List<ShootPattern> _shootPatterns;
    private Animator _animator;
    private bool _isStopped = false;
    private bool _isLaserOnCooldown = false;
    private int _lastPatternIndex = -1;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _objectPool = BossSceneManager.Instance.ObjectPool;

        InvokeRepeating("ShootBullet", 2f, ShootingInterval);

        _shootPatterns = new List<ShootPattern>
        {
           ShootRotatingPattern,
           ShootLaserAtPlayerPattern,
           ShootMeteorPatternPattern
        };

        RotatingObject.gameObject.SetActive(false);
    }

    private void ShootBullet()
    {
        if (_isStopped) return;

        int randomIndex = Random.Range(0, _shootPatterns.Count);

        while (randomIndex == _lastPatternIndex && randomIndex == 1 && _isLaserOnCooldown)
        {
            randomIndex = Random.Range(0, _shootPatterns.Count);
        }

        _lastPatternIndex = randomIndex;
        TriggerAnimation(randomIndex);
        _shootPatterns[randomIndex]();
    }

    private void TriggerAnimation(int patternIndex)
    {
        string triggerName = patternIndex switch
        {
            0 => "Bullet",
            1 => "LaserOrLightning",
            2 => "Bullet",
            _ => "Default",
        };

        _animator.SetTrigger(triggerName);
    }
    #region 전깃줄 패턴
    private void ShootRotatingPattern()
    {
        if (!RotatingObject.gameObject.activeSelf)
        {
            StartCoroutine(RotatingCoroutine());
        }
    }

    private IEnumerator RotatingCoroutine()
    {
        GM.PlaySFX(SFX.Lightning);

        RotatingObject.gameObject.SetActive(true);
        float endTime = Time.time + 10f;
        while (Time.time < endTime)
        {
            RotatingObject.Rotate(Vector3.forward, RotationSpeed * Time.deltaTime);
            yield return null;
        }
        RotatingObject.gameObject.SetActive(false);
    }
    #endregion

    #region 레이저 패턴
    private void ShootLaserAtPlayerPattern()
    {
        if (!_isLaserOnCooldown)
        {
            StartCoroutine(SpawnLasersCoroutine());
        }
    }

    private IEnumerator SpawnLasersCoroutine()
    {
        _isLaserOnCooldown = true;

        for (int i = 0; i < 5; i++)
        {
            Vector2 playerPosition = GameManager.Instance.Player.transform.position;
            PoolObject laser = _objectPool.SpawnFromPool(Tag.BossLaser);
            if (laser != null)
            {
                laser.transform.position = playerPosition;
                float randomAngle = Random.Range(0f, 360f);
                laser.transform.rotation = Quaternion.Euler(0f, 0f, randomAngle);
            }
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(10f);
        _isLaserOnCooldown = false;
    }
    #endregion

    #region 메테오 패턴
    private void ShootMeteorPatternPattern()
    {
        StartCoroutine(ShootMeteorPatternCoroutine());
    }
    private IEnumerator ShootMeteorPatternCoroutine()
    {
        int meteorCount = 10;
        for (int i = 0; i < meteorCount; i++)
        {
            PoolObject BossMeteor = _objectPool.SpawnFromPool(Tag.BossMeteor);
            if (BossMeteor != null)
            {
                float randomX = Random.Range(-12f, 12f);
                float randomY = Random.Range(-6f, 6f);
                Vector2 spawnPosition = new Vector2(randomX, randomY);
                BossMeteor.transform.position = spawnPosition;
                BossMeteor.gameObject.SetActive(true);
            }
            yield return new WaitForSeconds(RandomFireRate);
        }
    }

    #endregion


    public void StopAllPatterns()
    {
        _isStopped = true;
        _animator.SetTrigger("Die");
    }

    #region 소리모음

    #endregion
}
