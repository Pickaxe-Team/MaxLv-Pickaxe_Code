using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Boss2Patterns : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private Transform FirePoint;
    [SerializeField] private GameObject Smash;
    [SerializeField] private GameObject[] FloatingBullet;
    [SerializeField] private Light2D GlobalLight;
    private ObjectPool _objectPool;
    private bool _isAttacking = false;
    private bool _hasSpawnedBullet = false;
    private Boss _boss;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boss = GetComponent<Boss>();
    }

    private void Start()
    {
        _objectPool = BossSceneManager.Instance.ObjectPool;
        StartCoroutine(AttackRoutine());

        StartCoroutine(RandomlyDimMap());
    }

    private void Update()
    {
        if (!_isAttacking)
        {
            FollowPlayer();
        }
        SpawnBullet();
    }
    private IEnumerator RandomlyDimMap()
    {
        while (true)
        {
            float randomDelay = Random.Range(10f, 15f);
            yield return new WaitForSeconds(randomDelay);

            float dimDuration = 2f;
            DOTween.To(() => GlobalLight.intensity, x => GlobalLight.intensity = x, 0f, dimDuration)
                .SetEase(Ease.InOutQuad);

            yield return new WaitForSeconds(20f);

            float brightenDuration = 2f;
            DOTween.To(() => GlobalLight.intensity, x => GlobalLight.intensity = x, 1f, brightenDuration)
                .SetEase(Ease.InOutQuad);

            yield return new WaitForSeconds(brightenDuration);
        }
    }
    private void SpawnBullet()
    {
        if (!_hasSpawnedBullet && _boss.BossCurHP < _boss.EnemyData.HP / 2)
        {
            _animator.SetTrigger("Idle");
            _hasSpawnedBullet = true;
        }
    }
    #region 범위 체크
    private bool IsPlayerInRange()
    {
        if (GM.Player != null)
        {
            float distanceToPlayerSq = (GM.Player.transform.position - transform.position).sqrMagnitude;
            float attackRangeSq = _boss.EnemyData.AttackRange * _boss.EnemyData.AttackRange;
            return distanceToPlayerSq <= attackRangeSq;
        }
        return false;
    }
    #endregion

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1f, 3f));

            if (!_isAttacking)
            {
                _isAttacking = true;
                if (IsPlayerInRange())
                {
                    _animator.SetTrigger("Attack1");
                }
                else
                {
                    _animator.SetTrigger("Attack2");
                }
            }
        }
    }

    #region 보스 움직임 제어
    public void EnableMovement()
    {
        _isAttacking = false;
        _animator.SetTrigger("Walk");
    }
    public void DisableMovement()
    {
        _isAttacking = true;
    }
    #endregion

    #region 플레이어 따라가기
    private void FollowPlayer()
    {
        if (GM.Player != null)
        {
            Vector3 direction = GM.Player.transform.position - transform.position;
            transform.position = Vector3.MoveTowards(transform.position, GM.Player.transform.position, _boss.EnemyData.Speed * Time.deltaTime);

            if (direction.x > 0)
            {
                _spriteRenderer.flipX = false;
            }
            else if (direction.x < 0)
            {
                _spriteRenderer.flipX = true;
            }
        }
    }
    #endregion

    #region 땅치는 패턴
    public void SmashGround()
    {
        Smash.SetActive(true);
    }
    public void DelteSmashGround()
    {
        Smash.SetActive(false);
    }
    #endregion

    #region 튕기는 총알 소환 패턴
    public void SpawnFloatingBullet()
    {
        for (int i = 0; i < FloatingBullet.Length; i++)
        {
            FloatingBullet[i].SetActive(true);
        }
       
    }
    public void DeleteFloatingBullet()
    {
        for (int i = 0; i < FloatingBullet.Length; i++)
        {
            FloatingBullet[i].SetActive(false);
        }
    }
    #endregion

    #region 땅치면 총알 나가는 패턴
    public void Shootbullet()
    {
        int patternChoice = Random.Range(0, 2);
        if (patternChoice == 0)
        {
            ShootBullets360();
        }
        else if (patternChoice == 1)
        {
            ShootBulletsAtPlayer();
        }
    }

    private void ShootBullets360()
    {
        Vector2 firePointPosition = this.FirePoint.position;

        int numberOfBullets = 12;
        float spreadAngle = 360f;

        float angleStep = spreadAngle / numberOfBullets;
        float startAngle = 0f;

        for (int i = 0; i < numberOfBullets; i++)
        {
            float currentAngle = startAngle + (angleStep * i);
            Vector2 direction = (Quaternion.Euler(0, 0, currentAngle) * Vector2.right).normalized;

            CreateBullet(Tag.BossBullet, firePointPosition, direction, Tag.Boss);
        }
    }

    private void ShootBulletsAtPlayer()
    {
        if (GM.Player != null)
        {
            Vector2 firePointPosition = this.FirePoint.position;
            Vector2 playerPosition = GM.Player.transform.position;
            Vector2 directionToPlayer = (playerPosition - firePointPosition).normalized;

            int numberOfBullets = 3;
            float spreadAngle = 15f;

            float angleStep = spreadAngle / (numberOfBullets - 1);
            float startAngle = -spreadAngle / 2;

            for (int i = 0; i < numberOfBullets; i++)
            {
                float currentAngle = startAngle + (angleStep * i);
                Vector2 direction = (Quaternion.Euler(0, 0, currentAngle) * directionToPlayer).normalized;

                CreateBullet(Tag.BossBullet, firePointPosition, direction, Tag.Boss);
            }
        }
    }

    private void CreateBullet(string tag, Vector2 position, Vector2 direction, string ownerTag)
    {
        PoolObject bullet = _objectPool.SpawnFromPool(tag);
        bullet.ReturnMyComponent<Bullet>().Initialize(position, direction, ownerTag);
    }
    #endregion

    #region 소리모음

    public void PlayGolemRoarSound()
    {
        GM.PlaySFX(SFX.GolemRoar);
    }
    public void PlayGolemStompSound()
    {
        GM.PlaySFX(SFX.GolemStomp);
    }
    public void PlayGolemWalkingSound()
    {
        GM.PlaySFX(SFX.GolemWalking);
    }

    #endregion
}
