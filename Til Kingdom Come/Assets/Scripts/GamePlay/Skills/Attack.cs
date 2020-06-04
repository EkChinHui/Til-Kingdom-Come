using System;
using System.Collections;
using UnityEngine;

namespace GamePlay.Skills
{
    public class Attack : Skill
    {
        
        private const int Damage = 1;
        public float AttRange = 2.43f;
        public Transform attackPoint;
        private const float KnockDistAttacking = 8f;
        private const float KnockDistBlocking = 4f;
        private const float AttackCooldown = 0.4f;
        private const float ReactionDelay = 0.4f;
        public LayerMask playerLayer = 8;


        private void Start()
        {
            skillName = "Attack";
            skillInfo = "basic attack";
            skillCooldown = AttackCooldown;
        }

        public override void Cast(PlayerController player, PlayerController opponent)
        {
            if (!CanCast()) return;
            StartCoroutine(AttackAnimDelay(player));


        }
        
        private void AttackCast(PlayerController player)
        {
            // Detect enemies in range of attack
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, AttRange, playerLayer);
            // maximum distance between both players for attack to be successful
            float attackDistance = 1.934f + AttRange;
            // Damage enemies
            foreach(Collider2D enemy in hitEnemies)
            {
                if (enemy.GetComponent<PlayerController>() == null) continue;
                PlayerController otherPlayer = enemy.GetComponent<PlayerController>();
                float enemyDirection = otherPlayer.transform.rotation.eulerAngles.y;
                float myDirection = player.transform.rotation.eulerAngles.y;
                if (otherPlayer.isBlocking && Math.Abs(enemyDirection - myDirection) > 1f - Mathf.Epsilon)
                {
                    // other player successfully defends against attack
                    player.KnockBack(KnockDistAttacking);
                    otherPlayer.KnockBack(KnockDistBlocking);
                }
                else if (!otherPlayer.canRoll)
                {
                    // the enemy is rolling and is invulnerable
                }
                else if (otherPlayer.isAttacking && (Mathf.Abs(otherPlayer.transform.position.x - transform.position.x) <= attackDistance))
                {
                    // the enemy attacked first and is in range
                }
                else
                {
                    enemy.GetComponent<PlayerController>().TakeDamage(Damage);
                }
            }
            
        }
        
        private IEnumerator AttackAnimDelay(PlayerController player)
        {
            player.isActing = true;
            player.anim.SetTrigger("Attack");
            // reaction delay to allow opponent to react
            yield return new WaitForSeconds(ReactionDelay);
            player.isAttacking = true;
            AttackCast(player);
            yield return new WaitForSeconds(AnimationTimes.instance.AttackAnim - ReactionDelay);
            player.isSilenced = true;
            yield return new WaitForSeconds(AttackCooldown);
            player.isSilenced = false;
            player.isActing = false;
            player.isAttacking = false;
        }
        
        
        // draws wireframe for attack point to make it easier to set attack range
        private void OnDrawGizmosSelected()
        {
            if (attackPoint == null)
            {
                return;
            }
            Gizmos.DrawWireSphere(attackPoint.position, AttRange);
        }
    }
}