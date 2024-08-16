using UnityEngine;

public class UpgradeTrigger : MonoBehaviour
{
    [SerializeField] private GameObject UpgradePopup;
    [SerializeField] private UpgradeManager UpgradeManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(Tag.Player))
        {
            UpgradePopup.SetActive(true);
        }
    }
}
