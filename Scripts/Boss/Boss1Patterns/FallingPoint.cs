using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPoint : PoolObject
{
    private GameManager GM => GameManager.Instance;
    private void GetCollider2D()
    {
        GetComponent<Collider2D>().enabled = true;
    }
    private void DeleteCollider2D()
    {
        GetComponent<Collider2D>().enabled = false;
    }

    private void DeleteObject()
    {
        gameObject.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tag.Player))
        {
            GM.Player.TakeDamage(GM.Player.MaxHP * 0.34f);
        }
    }
    public void PlayMeteorImpactSound()
    {
        GM.PlaySFX(SFX.MeteorImpact);
    }
    public void PlayMeteorShootSound()
    {
        GM.PlaySFX(SFX.MeteorShoot);
    }
}
