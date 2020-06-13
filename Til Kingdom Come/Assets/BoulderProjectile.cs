using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamePlay.Skills
{
    public class BoulderProjectile : MonoBehaviour
    {
        public int damage = 1;
        // Start is called before the first frame update
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
                default:
                    damagedPlayer.TakeDamage(damage);
                    // DestroyProjectile();
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
