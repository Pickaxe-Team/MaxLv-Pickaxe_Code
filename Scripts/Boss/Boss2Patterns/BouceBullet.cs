using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouceBullet : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private GameManager GM => GameManager.Instance;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        int terrainLayer = LayerMask.NameToLayer("Terrain");

        if (collision.gameObject.layer == terrainLayer)
        {
            GM.PlaySFX(SFX.FloatingBullet);
        }
    }

    private void OnEnable()
    {
        SetRandomDiagonalDirection();
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(Layer.FloatingBullet), LayerMask.NameToLayer(Layer.Player), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(Layer.FloatingBullet), LayerMask.NameToLayer(Layer.Boss), true);
    }
    private void SetRandomDiagonalDirection()
    {
        Vector2[] directions = {
            new Vector2(1, 1).normalized,
            new Vector2(1, -1).normalized,
            new Vector2(-1, 1).normalized,
            new Vector2(-1, -1).normalized
        };

        Vector2 randomDirection = directions[Random.Range(0, directions.Length)];
        float speed = 8f;
        _rigidbody.velocity = randomDirection * speed;
    }
}
