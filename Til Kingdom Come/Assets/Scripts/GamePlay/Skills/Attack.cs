using System;
using System.Collections;
using UnityEngine;

namespace GamePlay.Skills
{
    public class Attack : Skill
    {
        public Transform attackPoint;
        
        private LayerMask playerLayer;
        
        [Header("Variables")]
        private const int Damage = 1;
        public float attRange = 1.85f;
        public float knockDistAttacking = 8f;
        private float knockDistBlocking = 4f;
        private float attackCooldown = 0f;
        private float reactionDelay = 0.2f;
        public Charges charges;
        public int maxCharges = 3;
        public float chargeTime = 5f;
        public Combo combo;

        private void Start()
        {
            skillName = "Attack";
            skillInfo = "basic attack";
            skillCooldown = attackCooldown;
            playerLayer = 1 << LayerMask.NameToLayer("Player");
            charges = gameObject.AddComponent<Charges>();
            charges.SetCharges(maxCharges, chargeTime);
            combo = new Combo();
        }

        protected override bool CanCast()
        {
            if (Time.time < nextAvailTime) return false;
            nextAvailTime = skillCooldown + Time.time;
            return true;
        }

        public override void Cast(PlayerController player, PlayerController opponent)
        {
            if (AttackPriority(player, opponent) || !CanCast()) return;

            if (charges.CurrentCharge <= 0) {
                print("0 charges left");
                return;
            }

            combo.UpdateDecay();
            charges.CurrentCharge -= 1;
            print("charges: " + charges.CurrentCharge);
            print("combo: " + combo.CurrentCombo);
            // within the decay time combo will be upgraded to next combo;
            combo.SetDecay();
            StartCoroutine(player.cooldownUiController.attackIcon.ChangesFillAmount(skillCooldown));
            StartCoroutine(AttackAnimDelay(player));
            combo.UpdateCombo();
        }
        
        private void AttackCast(PlayerController player)
        {
            // Detect enemies in range of attack
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attRange, playerLayer);
            // maximum distance between both players for attack to be successful
            if (hitEnemies.Length == 0) {
                AudioManager.instance.PlaySoundEffect("Sword Swing");
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
                        // enemy successfully defends against attack
                        AudioManager.instance.PlaySoundEffect("Swords Collide");
                        // trigger successful block event
                        otherPlayer.onSuccessfulBlock?.Invoke();
                        player.KnockBack(knockDistAttacking);
                        otherPlayer.KnockBack(knockDistBlocking);
                    }
                    else if (otherPlayer.combatState == PlayerController.CombatState.Rolling)
                    {
                        // enemy is rolling and is invulnerable
                        AudioManager.instance.PlaySoundEffect("Sword Swing");
                        return;
                    }
                    else if (otherPlayer.combatState == PlayerController.CombatState.Dead)
                    {
                        // enemy is dead
                        AudioManager.instance.PlaySoundEffect("Sword Swing");
                        return;
                    }
                    else
                    {
                        AudioManager.instance.PlaySoundEffect("Decapitation");
                        player.otherPlayer.TakeDamage(Damage);
                    }
                }
            }
        }

        private bool AttackPriority(PlayerController player, PlayerController opponent)
        // 5.0125 value is the maximum distance between 2 players for the attack to be successful
        // currently obtained manually by trial and error but we should find a way to calculate it`
        {
            var opponentAttackedFirst = opponent.combatState == PlayerController.CombatState.Attacking 
                                        && Mathf.Abs(opponent.transform.position.x - player.transform.position.x) <= 5.0125f;
            return opponentAttackedFirst;
        }
        
        private IEnumerator AttackAnimDelay(PlayerController player)
        {
            player.combatState = PlayerController.CombatState.Attacking;
            player.anim.SetTrigger("Attack");
            print(player + " attacks");
            // reaction delay to allow opponent to react
            yield return new WaitForSeconds(reactionDelay);
            AttackCast(player);
            yield return new WaitForSeconds(AnimationTimes.instance.AttackAnim - reactionDelay);
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