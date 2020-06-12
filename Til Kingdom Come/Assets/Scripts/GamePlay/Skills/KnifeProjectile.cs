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
            // Destroys the projectile after its lifetime has ended
            Invoke(nameof(DestroyProjectile), lifeTime);
        }

        private void Update()
        {
            rb.velocity = transform.right * speed;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // destroys projectile if it touches a wall
            if (collision.CompareTag("Wall")) DestroyProjectile();
            
            if (collision.gameObject.layer != LayerMask.NameToLayer("Player")) return;
            // projectile collides a player
            PlayerController damagedPlayer = collision.GetComponent<PlayerController>();
            if (damagedPlayer == null) return;
            switch (damagedPlayer.combatState)
            {
                // Player cant be hit while rolling
                case PlayerController.CombatState.Rolling:
                    Debug.Log("Successful dodge");
                    return;
                // Player will not be damaged while blocking (projectile is destroyed)
                case PlayerController.CombatState.Blocking:
                    DestroyProjectile();
                    Debug.Log("Successful block");
                    damagedPlayer.onSuccessfulBlock?.Invoke();
                    break;
                default:
                    damagedPlayer.TakeDamage(damage);
                    DestroyProjectile();
                    Debug.Log("Projectile Hits target");
                    return;
            }
        }

        private void DestroyProjectile()
        {
            Destroy(gameObject);    
        }
    }
}
