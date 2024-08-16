using System.Collections;
using UnityEngine;

public class BossSummon : PoolObject
{
    private GameManager GM => GameManager.Instance;
    private SpriteRenderer _spriteRenderer;
    private bool _isAttacking = false;
    private Animator _animator;

    private float _moveSpeed = 4f;
    private Coroutine _dieCoroutine;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        EnableMovement();
        _dieCoroutine = StartCoroutine(DieAfterTime(10f));
    }

    private void OnDisable()
    {
        if (_dieCoroutine != null)
        {
            StopCoroutine(_dieCoroutine);
            _dieCoroutine = null;
        }
    }

    private void Update()
    {
        if (!_isAttacking)
        {
            FollowPlayer();
        }
    }

    private void FollowPlayer()
    {
        if (GM.Player != null)
        {
            Vector3 direction = GM.Player.transform.position - transform.position;
            transform.position = Vector3.MoveTowards(transform.position, GM.Player.transform.position, _moveSpeed * Time.deltaTime);

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tag.Player))
        {
            GM.Player.TakeDamage(GM.Player.MaxHP * 0.1f);
            gameObject.SetActive(false);
        }
    }

    private IEnumerator DieAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        _animator.SetTrigger("Die");
        DisableMovement();
    }
    public void Delete()
    {
        gameObject.SetActive(false);
    }
    #region 움직임 제어
    public void EnableMovement()
    {
        _isAttacking = false;
    }
    public void DisableMovement()
    {
        _isAttacking = true;
    }
    
    #endregion
}
