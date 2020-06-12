using System;
using System.Collections;
using UnityEngine;

namespace GamePlay.Skills
{
    public class Attack : Skill
    {
        
        private const int Damage = 1;
        public float attRange = 1.85f;
        public Transform attackPoint;
        private const float KnockDistAttacking = 8f;
        private const float KnockDistBlocking = 4f;
        private const float AttackCooldown = 0.4f;
        private const float ReactionDelay = 0.2f;
        public LayerMask playerLayer;

        private void Start()
        {
            skillName = "Attack";
            skillInfo = "basic attack";
            skillCooldown = AttackCooldown;
            playerLayer = 1 << LayerMask.NameToLayer("Player");
        }

        public override void Cast(PlayerController player, PlayerController opponent)
        {
            if (!CanCast()) return;
            StartCoroutine(player.cooldownUiController.attackIcon.ChangesFillAmount(skillCooldown));
            StartCoroutine(AttackAnimDelay(player));
        }
        
        private void AttackCast(PlayerController player)
        {
            // Detect enemies in range of attack
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attRange, playerLayer);
            // maximum distance between both players for attack to be successful
            float attackDistance = 1.934f + attRange;
            if (hitEnemies.Length == 0) {
                AudioManager.instance.Play("Sword Swing");
            }
            else
            {
                // Damage enemies
                foreach(Collider2D enemy in hitEnemies)
                {
                    if (enemy.GetComponent<PlayerController>() == null) continue;
                    PlayerController otherPlayer = enemy.GetComponent<PlayerController>();
                    float enemyDirection = otherPlayer.transform.rotation.eulerAngles.y;
                    float myDirection = player.transform.rotation.eulerAngles.y;
                    if (otherPlayer.combatState == PlayerController.CombatState.Blocking && Math.Abs(enemyDirection - myDirection) > 1f - Mathf.Epsilon)
                    {
                        // other player successfully defends against attack
                        AudioManager.instance.Play("Swords Collide");
                        // trigger successful block event
                        otherPlayer.onSuccessfulBlock?.Invoke();
                        player.KnockBack(KnockDistAttacking);
                        otherPlayer.KnockBack(KnockDistBlocking);
                    }
                    else if (otherPlayer.combatState == PlayerController.CombatState.Rolling)
                    {
                        // the enemy is rolling and is invulnerable
                        AudioManager.instance.Play("Sword Swing");
                        return;
                    }
                    else if (otherPlayer.combatState == PlayerController.CombatState.Attacking && 
                            Mathf.Abs(otherPlayer.transform.position.x - transform.position.x) <= attackDistance)
                    {
                        // the enemy attacked first and is in range
                        return;
                    }
                    else
                    {
                        AudioManager.instance.Play("Decapitation");
                        player.otherPlayer.TakeDamage(Damage);
                    }
                }
            }
        }
        
        private IEnumerator AttackAnimDelay(PlayerController player)
        {
            player.combatState = PlayerController.CombatState.Attacking;
            player.anim.SetTrigger("Attack");
            // reaction delay to allow opponent to react
            yield return new WaitForSeconds(ReactionDelay);
            AttackCast(player);
            yield return new WaitForSeconds(AnimationTimes.instance.AttackAnim - ReactionDelay);
            player.combatState = PlayerController.CombatState.NonCombatState;
        }
        
        
        // draws wireframe for attack point to make it easier to set attack range
        private void OnDrawGizmosSelected()
        {
            if (attackPoint == null)
            {
                return;
            }
            Gizmos.DrawWireSphere(attackPoint.position, attRange);
        }
    }
}