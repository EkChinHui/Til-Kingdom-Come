using UnityEngine;

namespace GamePlay.Skills
{
    public class KnifeProjectile : MonoBehaviour
    {
        public Rigidbody2D rb;
        public float speed = 15f;
        public int damage = 1;
        public float lifeTime = 3;

        private void Start()
        {
            Invoke(nameof(DestroyProjectile), lifeTime);

        }

        private void Update()
        {
            rb.velocity = transform.right * speed;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer != 10) return;
            PlayerController damagedPlayer = collision.GetComponent<PlayerController>();
            if (damagedPlayer == null) return;
            // Player can still be hit while rolling
            if (damagedPlayer.IsShieldUp())
            {
                DestroyProjectile();
            } else
            {
                damagedPlayer.TakeDamage(damage);
                DestroyProjectile();
            }
        }

        private void DestroyProjectile()
        {
            Destroy(gameObject);    
        }
    }
}
