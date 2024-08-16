using System.Collections;
using UnityEngine;

public class Laser : PoolObject
{
    private GameManager GM => GameManager.Instance;

    public void StartCollider()
    {
        GetComponent<Collider2D>().enabled = true;
    }
    public void StopCollider()
    {
        GetComponent<Collider2D>().enabled = false;
    }
    public void DeleteLaser()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tag.Player))
        {
            GM.Player.TakeDamage(GM.Player.MaxHP * 0.1f);
        }
    }
    public void PlayLaserSound()
    {
        GM.PlaySFX(SFX.Laser);
    }
}
