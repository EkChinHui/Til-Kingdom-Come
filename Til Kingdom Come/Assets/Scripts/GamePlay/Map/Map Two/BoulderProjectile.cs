using UnityEngine;

namespace GamePlay.Map.Map_Two
{
    public class BoulderProjectile : MonoBehaviour
    {
        public int damage = 1;
        public GameObject collideEffect;
        public float heightOffset;
        public GameObject parent;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // destroys projectile if it touches a wall
            if (collision.CompareTag("Wall")) Impact();
            
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
                case PlayerController.CombatState.Dead:
                    return;
                default:
                    Debug.Log(damagedPlayer.combatState);
                    damagedPlayer.TakeDamage(damage);
                    Debug.Log("Projectile Hits Target");
                    Impact();
                    return;
            }
        }
        private void Impact()
        {
            AudioManager.instance.PlaySoundEffect("Boulder Hit");
            var offSet = new Vector3(0, heightOffset, 0);
            Instantiate(collideEffect, transform.position + offSet, Quaternion.identity);
            Destroy(parent);
        }
    }
}
