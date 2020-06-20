using System;
using GamePlay.Player;
using UnityEngine;

namespace GamePlay.Skills
{
    public class KnifeProjectile : MonoBehaviour
    {
        public Rigidbody2D rb;
        public float speed = 15f;
        public int damage = 1;

        private void Awake()
        {
            ScoreKeeper.resetPlayersEvent += DestroyProjectile;
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
                    Debug.Log("Successfully Dodged Throwing Knife");
                    return;
                // Player will not be damaged while blocking (projectile is destroyed)
                case PlayerController.CombatState.Blocking:
                    DestroyProjectile();
                    AudioManager.instance.PlaySoundEffect("Swords Collide");
                    Debug.Log("Successfully Blocked Throwing Knife");
                    damagedPlayer.onSuccessfulBlock?.Invoke();
                    break;
                default:
                    damagedPlayer.TakeDamage(damage);
                    DestroyProjectile();
                    Debug.Log("Throwing Knife Hit");
                    return;
            }
        }

        private void DestroyProjectile()
        {
            Destroy(gameObject);    
        }

        private void OnDestroy()
        {
            ScoreKeeper.resetPlayersEvent -= DestroyProjectile;
        }
    }
}
