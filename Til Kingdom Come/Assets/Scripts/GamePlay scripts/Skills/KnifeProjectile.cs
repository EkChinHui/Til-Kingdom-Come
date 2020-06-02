using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeProjectile : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 15f;
    public int damage = 1;
    public float lifeTime = 3;
    public LayerMask collide;

    private void Start()
    {
        Invoke("DestroyProjectile", lifeTime);

    }

    private void Update()
    {
        rb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            PlayerController damagedPlayer = collision.GetComponent<PlayerController>();
            if (damagedPlayer != null) 
            {
                if (damagedPlayer.isShieldUp())
                {
                    DestroyProjectile();
                } else
                {
                    damagedPlayer.TakeDamage(damage);
                    DestroyProjectile();
                }
                
            }
        }
    }

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
