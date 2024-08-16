using UnityEngine;

public class DungeonExitPopup : MonoBehaviour
{
    private void OnEnable()
    {
        Time.timeScale = 0f;
    }

    public void OnClickOK()
    {
        GameManager.Instance.PlaySFX(SFX.Click);
        SceneLoadManager.LoadScene(2);
    }

    public void OnClickCancle()
    {
        GameManager.Instance.PlaySFX(SFX.Click);
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }
}
