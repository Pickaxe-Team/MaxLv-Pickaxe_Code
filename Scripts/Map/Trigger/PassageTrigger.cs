using UnityEngine;

public class PassageTrigger : MonoBehaviour
{
    [SerializeField] private GameObject PassagePopup;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tag.Player) && collision.TryGetComponent(out HealthSystem healthSystem))
        {
            if (healthSystem.IsDead) return;

            PassagePopup.SetActive(true);
        }
    }
}
