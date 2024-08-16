using System.Collections;
using UnityEngine;

public class PickAxeController : MonoBehaviour
{
    private PickaxeData NowPickaxeData => GameManager.Instance.GetPickaxeData((PickaxeID)GameManager.Instance.NowPlayerData.PickaxeLV);

    private Animator _animator;
    private Transform _pickAxeTransform;
    public SpriteRenderer SpriteRenderer;
    private bool _isRight = false; // 플레이어의 방향을 나타내는 불리언 값


    void Awake()
    {
        _animator = GetComponent<Animator>();
        _pickAxeTransform = transform;
        SpriteRenderer = GetComponent<SpriteRenderer>();

    }

    private void OnEnable()
    {
        // 활성화될 때 isRight 값을 애니메이터에 반영
        UpdateAnimatorDirection();
        UpdateSpriteByLevel();
    }

    public void ResetRotationZ()
    {
        if (_pickAxeTransform != null) // null 체크 추가
        {
            Vector3 currentRotation = _pickAxeTransform.localEulerAngles;
            currentRotation.z = 0;
            _pickAxeTransform.localEulerAngles = currentRotation;
        }
    }

    public void SetFlipDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0)
        {
            _isRight = true;
            SpriteRenderer.flipX = false; // 오른쪽 방향
        }
        else if (moveInput.x < 0)
        {
            _isRight = false;
            SpriteRenderer.flipX = true; // 왼쪽 방향
        }

        // Animator에 isRight 값을 설정
        UpdateAnimatorDirection();
    }

  

    public void PlayAttackAnimation()
    {
        if (_animator != null)
        {
            if (_isRight)
            {
                _animator.SetTrigger("AttackRight");
            }
            else
            {
                _animator.SetTrigger("AttackLeft");
            }
        }
    }

    public void ResetAttackAnimation()
    {
        if (_animator != null)
        {
            _animator.ResetTrigger("AttackRight");
            _animator.ResetTrigger("AttackLeft");
        }
    }

    // 현재 방향을 기반으로 애니메이터 업데이트
    private void UpdateAnimatorDirection()
    {
        if (_animator != null)
        {
            _animator.SetBool("IsRight", _isRight);
        }
    }

    public void UpdateSpriteByLevel()
    {
        SpriteRenderer.sprite = NowPickaxeData.Image;
    }
}
