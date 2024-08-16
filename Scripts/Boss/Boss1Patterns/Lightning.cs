using UnityEngine;

public class Lightning : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tag.Player))
        {
            GM.Player.TakeDamage(GM.Player.MaxHP * 0.1f);
        }
    }
}
