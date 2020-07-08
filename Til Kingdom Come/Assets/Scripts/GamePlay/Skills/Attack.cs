using System;
using System.Collections;
using Cinemachine;
using GamePlay.Map.Map_Two;
using GamePlay.Player;
using UI.GameUI.Cooldown;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.Skills
{
    public class Attack : Skill
    {
        public Transform attackPoint;
        public GameObject comboEffect;
        private LayerMask playerLayer;
        public Sprite attackTwo;
        public Sprite attackThree;
        
        [Header("Variables")]
        private const int AttackDamage = 20;
        private const int FinalComboDamage = 40;
        private float attRange = 1.85f;
        private float knockDistAttacking = 10f;
        private float knockDistBlocking = 5f;
        private float attackCooldown = 0f;
        private float reactionDelay = 0.2f;
        public Charges charges;
        public int maxCharges = 3;
        private float chargeTime = 4f;
        public Combo combo;
        private CooldownUI attackIcon;
        private Image darkMask;
        private float moveDistance = 0;//12f;

        public static Action onMissedAttack;
        public static Action onSuccessfulAttack;
        
        private void Start()
        {
            player = gameObject.GetComponentInParent<PlayerController>();
            skillName = "Attack";
            skillInfo = "basic attack";
            skillCooldown = attackCooldown;
            playerLayer = 1 << LayerMask.NameToLayer("Player");
            charges = gameObject.AddComponent<Charges>();
            charges.SetCharges(maxCharges, chargeTime);
            combo = new Combo();
            attackIcon = player.cooldownUiController.attackIcon;
            darkMask = attackIcon.darkMask.GetComponent<Image>();
        }

        private void Update()
        {
               player.cooldownUiController.attackIcon.gameObject.GetComponent<DisplayCharges>().UpdateCharges(charges.CurrentCharge);
               if (charges.isCharging && charges.CurrentCharge < maxCharges)
               {
                   charges.isCharging = false;
                   Debug.Log("Attack Charging");
                   StartCoroutine(player.cooldownUiController.attackIcon.ChangesFillAmount(chargeTime));
               }
            
               combo.UpdateDecay();
               // Swap skill icon based on current combo
               switch (combo.CurrentCombo)
               {
                   case Combo.ComboNumber.One:
                       attackIcon.image.sprite = icon;
                       darkMask.sprite = icon;
                       break;
                   case Combo.ComboNumber.Two:
                       attackIcon.image.sprite = attackTwo;
                       darkMask.sprite = attackTwo;
                       break;
                   case Combo.ComboNumber.Three:
                       attackIcon.image.sprite = attackThree;
                       darkMask.sprite = attackThree;
                       break;
               }
        }

        public override void Cast(PlayerController opponent)
        {
            Debug.Log("player cast attack");
            if (AttackPriority(opponent) || !CanCast()) return;
            if (charges.CurrentCharge <= 0) return;

            // Trigger cooldown UI
            if (charges.CurrentCharge == maxCharges)
            {
                Debug.Log("Attack Charging");
                charges.isCharging = true;
            }

            charges.CurrentCharge -= 1;
            // Disable input
            player.playerInput.Toggle();

            // Combo will be upgraded to next combo if it is executed within the decay time
            combo.SetDecay();
            switch (combo.CurrentCombo)
            {
                case (Combo.ComboNumber.One):
                    StartCoroutine(ComboOneAnimDelay());
                    break;
                case (Combo.ComboNumber.Two):
                    StartCoroutine(ComboTwoAnimDelay());
                    break;
                case (Combo.ComboNumber.Three):
                    StartCoroutine(ComboThreeAnimDelay());
                    break;
            }

            Debug.Log("Attack Charges Left: " + charges.CurrentCharge);
            Debug.Log("Attack Executed: " + combo.CurrentCombo);
        }
        
        private void AttackCast()
        {
            BreakBoulders();
            // Detect enemies in range of attack
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attRange, playerLayer);
            // Maximum distance between both players for attack to be successful
            if (hitEnemies.Length == 0) {
                AudioManager.instance.PlaySoundEffect("Sword Swing");
                onMissedAttack?.Invoke();
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
                        var damage = combo.CurrentCombo == Combo.ComboNumber.Three
                            ? FinalComboDamage
                            : AttackDamage;
                        AudioManager.instance.PlaySoundEffect("Decapitation");
                        player.otherPlayer.TakeDamage(damage);
                        onSuccessfulAttack?.Invoke();
                        print("Combo: " + combo.CurrentCombo);
                    }
                }
            }
            combo.UpdateCombo();
        }

        private void BreakBoulders()
        {
            Collider2D[] boulders = Physics2D.OverlapCircleAll(attackPoint.position, attRange + 0.5f,
                1 << LayerMask.NameToLayer("Interactables"));
            print(boulders);
            foreach (var boulder in boulders)
            {
                var boulderProjectile = boulder.GetComponent<BoulderProjectile>();
                boulderProjectile.Impact();
            }
        }
        
        private bool AttackPriority(PlayerController opponent)
        // 5.0125 value is the maximum distance between 2 players for the attack to be successful
        // currently obtained manually by trial and error but we should find a way to calculate it`
        {
            var opponentAttackedFirst = opponent.combatState == PlayerController.CombatState.Attacking 
                                        && Mathf.Abs(opponent.transform.position.x - player.transform.position.x) <= 5.0125f;
            return opponentAttackedFirst;
        }
        
        private IEnumerator ComboOneAnimDelay()
        {
            player.combatState = PlayerController.CombatState.Attacking;
            player.anim.SetTrigger("Attack");
            // reaction delay to allow opponent to react
            yield return new WaitForSeconds(reactionDelay);
            AttackCast();
            yield return new WaitForSeconds(AnimationTimes.instance.AttackAnim - reactionDelay);
            player.combatState = PlayerController.CombatState.NonCombat;
            player.playerInput.Toggle();
        }

        private IEnumerator ComboTwoAnimDelay()
        {
            player.combatState = PlayerController.CombatState.Attacking;
            player.anim.SetTrigger("Attack 2");
            var velocity = player.rb.velocity;
            player.rb.velocity = Mathf.Abs(transform.localRotation.eulerAngles.y - 180) < Mathf.Epsilon
                ? new Vector2(-moveDistance, velocity.y)
                : new Vector2(moveDistance, velocity.y);
            // reaction delay to allow opponent to react
            yield return new WaitForSeconds(reactionDelay);
            AttackCast();
            yield return new WaitForSeconds(AnimationTimes.instance.AttackAnim - reactionDelay);
            player.combatState = PlayerController.CombatState.NonCombat;
            player.playerInput.Toggle();
        }

        private IEnumerator ComboThreeAnimDelay()
        {
            player.combatState = PlayerController.CombatState.Attacking;
            player.anim.SetTrigger("Attack 3");
            var velocity = player.rb.velocity;
            // move forward
            player.rb.velocity = Mathf.Abs(transform.localRotation.eulerAngles.y - 180) < Mathf.Epsilon
                ? new Vector2(-moveDistance, velocity.y)
                : new Vector2(moveDistance, velocity.y);
            // reaction delay to allow opponent to react
            yield return new WaitForSeconds(reactionDelay);
            AttackCast();
            yield return new WaitForSeconds(AnimationTimes.instance.AttackAnim - reactionDelay);
            AudioManager.instance.PlaySoundEffect("Sword Smash");
            player.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
            player.combatState = PlayerController.CombatState.NonCombat;

            #region Particles
            var distanceOffset = Math.Abs(transform.localRotation.eulerAngles.y) < Mathf.Epsilon
                ? 4f
                : -4f;
            var offset = new Vector3(distanceOffset, 0, 0);

            Instantiate(comboEffect, transform.position + offset, Quaternion.identity);
            
            #endregion
            
            player.playerInput.Toggle();
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