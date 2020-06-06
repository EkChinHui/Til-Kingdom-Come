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
            if (collision.gameObject.layer != LayerMask.NameToLayer("Player")) return;
            PlayerController damagedPlayer = collision.GetComponent<PlayerController>();
            if (damagedPlayer == null) return;
            switch (damagedPlayer.combatState)
            {
                // Player cant be hit while rolling
                case PlayerController.CombatState.Rolling:
                    return;
                case PlayerController.CombatState.Blocking:
                    DestroyProjectile();
                    Debug.Log("here");
                    break;
                default:
                    damagedPlayer.TakeDamage(damage);
                    DestroyProjectile();
                    return;
            }
        }

        private void DestroyProjectile()
        {
            Destroy(gameObject);    
        }
    }
}
