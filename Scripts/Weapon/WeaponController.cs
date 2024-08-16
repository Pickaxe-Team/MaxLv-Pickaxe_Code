using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private PickAxeController _pickAxeController;

    void Awake()
    {
        _pickAxeController = GetComponentInChildren<PickAxeController>();
    }

    void Start()
    {

        SetFlipDirection();
    }

    void OnEnable()
    {
        if (_pickAxeController != null)
        {
            _pickAxeController.ResetRotationZ();
            SetFlipDirection();
        }
    }

    void OnDisable()
    {
        if (_pickAxeController != null)
        {
            _pickAxeController.ResetRotationZ();
        }
    }

    private void FixedUpdate()
    {
        SetFlipDirection();
    }


    public void SetFlipDirection()
    {
        if (_pickAxeController != null)
        {
            if (_pickAxeController.SpriteRenderer.flipX ==false)
            {
                this.transform.localPosition = new Vector3(0.3f, -0.3f, 0f);
            }
            else
            {
                this.transform.localPosition = new Vector3(0.1f, -0.3f, 0f); // 왼쪽 방향 설정
            }
        }
    }
}
