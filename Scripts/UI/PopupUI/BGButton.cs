using UnityEngine;

public class BGButton : MonoBehaviour
{
    public void Exit()
    {
        transform.parent.gameObject.SetActive(false);
        GameManager.Instance.PlaySFX(SFX.Click);
    }
}
