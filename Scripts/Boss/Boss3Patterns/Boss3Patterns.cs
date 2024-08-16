using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Boss3Patterns : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;
    private Animator _animator;
    [SerializeField] private List<GameObject> Effects;
    [SerializeField] private GameObject AttackAll;
    [SerializeField] private GameObject SafeZone;
    [SerializeField] private Image HPbar;
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private Transform FirePoint;

    private ObjectPool _objectPool;
    private bool _isAttacking = false;
    private Boss _boss;
    private bool _hasHealedOnce = false;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boss = GetComponent<Boss>();
    }
    void Start()
    {
        _objectPool = BossSceneManager.Instance.ObjectPool;
        StartCoroutine(PerformRandomAttack());
        HPbar.color = GM.GetHPColor(HPColor.Orange);
    }

    private void Update()
    {
        if (!_isAttacking)
        {
            FollowPlayer();
        }
        CheckAndHealBoss();
    }
    #region 애니메이션
    private IEnumerator PerformRandomAttack()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1f, 3f));
            int attackChoice = Random.Range(0, 10);
            if (attackChoice == 0)
            {
                _animator.SetTrigger("Attack2");
            }
            else if (attackChoice > 0 && attackChoice < 4)
            {
                _animator.SetTrigger("Idle");
            }
            else
            {
                int repeatCount = Random.Range(1, 5); 
                for (int i = 0; i < repeatCount; i++)
                {
                    _animator.SetTrigger("Attack1");
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
    }
    #endregion
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
                foreach (GameObject effect in Effects)
                {
                    if (effect != null)
                    {
                        Vector3 newPosition = effect.transform.localPosition;
                        newPosition.x = Mathf.Abs(newPosition.x); // x 좌표를 양수로
                        effect.transform.localPosition = newPosition;
                    }
                }
            }
            else if (direction.x < 0)
            {
                _spriteRenderer.flipX = true;
                foreach (GameObject effect in Effects)
                {
                    if (effect != null)
                    {
                        Vector3 newPosition = effect.transform.localPosition;
                        newPosition.x = -Mathf.Abs(newPosition.x); // x 좌표를 음수로
                        effect.transform.localPosition = newPosition;
                    }
                }
            }
        }
    }
    #endregion
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
    #region 이펙트 보여주기
    public void ShowEffect(int index)
    {
        if (index >= 0 && index < Effects.Count)
        {
            Effects[index].SetActive(true);
        }
    }

    public void DeleteEffect(int index)
    {
        if (index >= 0 && index < Effects.Count)
        {
            Effects[index].SetActive(false);
        }
    }
    #endregion
    #region 쫄따구소환
    public void Check()
    {
        PoolObject BossSummon = _objectPool.SpawnFromPool(Tag.BossSummon);
        if (BossSummon != null)
        {
            // 현재 위치에서 오브젝트 소환
            Vector2 spawnPosition = transform.position;
            BossSummon.transform.position = spawnPosition;
            BossSummon.gameObject.SetActive(true);
        }
    }
    #endregion
    #region 전체 범위 공격
    public void AttackAllArea()
    {
        AttackAll.SetActive(true);
    }
    public void StopAttackAllArea()
    {
        AttackAll.SetActive(false);
    }
    public void DealDamageAllArea()
    {
        if (SafeZone.GetComponent<Collider2D>().bounds.Contains(GM.Player.transform.position))
        {
            return;
        }
        else
        {
            GM.Player.TakeDamage(GM.Player.MaxHP * 0.9f);
        }
    }
    #endregion
    #region 총알 나가는 패턴
    private void ShootBullets360()
    {
        Vector2 firePointPosition = this.FirePoint.position;

        int numberOfBullets = 12;
        float spreadAngle = 360f;

        float angleStep = spreadAngle / numberOfBullets;
        float startAngle = Random.Range(0f, 360f); // 매번 랜덤 시작 각도 설정

        for (int i = 0; i < numberOfBullets; i++)
        {
            float currentAngle = startAngle + (angleStep * i);
            Vector2 direction = (Quaternion.Euler(0, 0, currentAngle) * Vector2.right).normalized;

            CreateBullet(Tag.BossBullet, firePointPosition, direction, Tag.Boss);
        }
    }

    private void CreateBullet(string tag, Vector2 position, Vector2 direction, string ownerTag)
    {
        PoolObject bullet = _objectPool?.SpawnFromPool(tag);

        bullet.ReturnMyComponent<Bullet>().Initialize(position, direction, ownerTag);

    }
    #endregion
    #region 보스 체력회복 패턴
    private void CheckAndHealBoss()
    {
        if (_boss.BossCurHP <= _boss.EnemyData.HP * 0.1f && !_hasHealedOnce)
        {
            _hasHealedOnce = true;
            _animator.SetTrigger("Time");
        }
    }

    public void HealBoss()
    {
        float myFloat = _boss.BossCurHP;
        float duration = 1f;
        DOTween.To(() => myFloat, x => myFloat = x, _boss.EnemyData.HP, duration).SetEase(Ease.Linear).OnUpdate(() =>
        {
            _boss.HealthSystem.Heal(myFloat - _boss.BossCurHP);
        });
        HPbar.DOColor(GM.GetHPColor(HPColor.Red), duration).SetEase(Ease.Linear);
    }
    #endregion
    #region 소리모음
    public void PlayGolemWalkingSound()
    {
        GM.PlaySFX(SFX.GolemWalking);
    }
    public void PlayGolemChargedSound()
    {
        GM.PlaySFX(SFX.GolemCharged);
    }

    public void PlayGolemPunchSound()
    {
        GM.PlaySFX(SFX.GolemPunch);
    }
    public void PlayGolmeHealSound()
    {
        GM.PlaySFX(SFX.GolmeHeal);
    }
    public void PlayGolemStompSound()
    {
        GM.PlaySFX(SFX.GolemStomp);
    }
    #endregion
}
