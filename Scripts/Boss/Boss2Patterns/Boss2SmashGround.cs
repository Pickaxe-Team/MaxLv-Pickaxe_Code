using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2SmashGround : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tag.Player))
        {
            GM.Player.TakeDamage(GM.Player.MaxHP * 0.7f);
        }
    }
}
