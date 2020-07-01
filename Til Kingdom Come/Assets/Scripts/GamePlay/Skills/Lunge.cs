using System;
using System.Collections;
using GamePlay.Player;
using UnityEngine;

namespace GamePlay.Skills
{
    public class Lunge : Skill
    {
        public Transform attackOrigin;
        public float lungeRange = 20f;
        private LayerMask playerLayer;
        public float channelTime = 0.3f;
        private bool isLunging;
        private float lungeDistance = 30f;
        public int attackDistance = 10;
        private float knockDistAttacking = 8f;
        private float knockDistBlocking = 4f;
        public int damage = 50;
        private Vector2 originOffset = new Vector2(1.5f, 2f);
        
        
        private void Start()
        {
            playerLayer = 1 << 8;
            skillName = "Lunge";
            skillInfo = "Channels an attack and charges at the enemy";
            skillCooldown = 10f;
            skillNumber = 4;
            attackOrigin = player.transform;
        }

        private void Update()
        {
            var transform2D = new Vector2(player.transform.position.x, player.transform.position.y);
            var direction = Math.Abs(player.transform.localRotation.eulerAngles.y - 180) < Mathf.Epsilon
                ? new Vector2(-1, 0)
                : new Vector2(1, 0);
            var tempOffset= Math.Abs(player.transform.localRotation.eulerAngles.y - 180) < Mathf.Epsilon
                ? new Vector2(-originOffset.x, originOffset.y)
                : new Vector2(originOffset.x, originOffset.y);
            Debug.DrawRay(transform2D + tempOffset, direction * attackDistance, Color.red,0);
            
        }

        public override void Cast(PlayerController opponent)
        {
            if (!CanCast()) return;
            StartCoroutine(AnimDelay());
            StartCoroutine(LungeMove());
            StartCoroutine(player.cooldownUiController.skillIcon.ChangesFillAmount(skillCooldown));
        }

        // raycast
        private void LungeCast()
        {
            var transform2D = new Vector2(player.transform.position.x, player.transform.position.y);
            var direction = Math.Abs(player.transform.localRotation.eulerAngles.y - 180) < Mathf.Epsilon
                ? new Vector2(-1, 0)
                : new Vector2(1, 0);
            var tempOffset= Math.Abs(player.transform.localRotation.eulerAngles.y - 180) < Mathf.Epsilon
                ? new Vector2(-originOffset.x, originOffset.y)
                : new Vector2(originOffset.x, originOffset.y);
            // Debug.DrawRay(transform2D + originOffset, direction * attackDistance, Color.red,3);

            RaycastHit2D rayCast = Physics2D.Raycast(transform2D + tempOffset, direction,
                attackDistance, playerLayer);

            if (rayCast)
            {
                PlayerController opponent = rayCast.collider.GetComponent<PlayerController>();
                Debug.Log(player.name);
                
                float enemyDirection = opponent.transform.rotation.eulerAngles.y;
                float myDirection = player.transform.rotation.eulerAngles.y;
                if (opponent.combatState == PlayerController.CombatState.Blocking 
                    && Math.Abs(enemyDirection - myDirection) > 1f - Mathf.Epsilon)
                {
                    // enemy successfully defends against attack
                    AudioManager.instance.PlaySoundEffect("Swords Collide");
                    // trigger successful block event
                    opponent.onSuccessfulBlock?.Invoke();
                    opponent.KnockBack(knockDistBlocking);
                }
                else if (opponent.combatState == PlayerController.CombatState.Rolling)
                {
                    // enemy is rolling and is invulnerable
                    AudioManager.instance.PlaySoundEffect("Sword Swing");
                }
                else if (opponent.combatState == PlayerController.CombatState.Dead)
                {
                    // enemy is dead
                    AudioManager.instance.PlaySoundEffect("Sword Swing");
                }
                else
                {
                    AudioManager.instance.PlaySoundEffect("Decapitation");
                    opponent.TakeDamage(damage);
                    Debug.Log("successful lunge hit");
                }
            }
        }
        
        private IEnumerator LungeMove()
        {
            yield return new WaitForSeconds(channelTime);
            LungeCast();
            /*var velocity = player.rb.velocity;
            player.rb.velocity = Math.Abs(player.transform.localRotation.eulerAngles.y - 180) < Mathf.Epsilon
                ? new Vector2(-lungeDistance, velocity.y)
                : new Vector2(lungeDistance, velocity.y);*/
            if (Math.Abs(player.transform.rotation.y) > Mathf.Epsilon)
            {
                player.rb.AddForce(player.transform.right * lungeDistance, ForceMode2D.Impulse);
            }
            else if (Math.Abs(player.transform.rotation.y) < Mathf.Epsilon)
            {
                player.rb.AddForce(player.transform.right * lungeDistance, ForceMode2D.Impulse);
            }
            yield return null;
        }
        
        private IEnumerator AnimDelay()
        {
            player.anim.SetTrigger(skillName);
            var animTime = AnimationTimes.instance.LungeAnim;
            player.combatState = PlayerController.CombatState.Skill;
            yield return new WaitForSeconds(animTime);
            player.combatState = PlayerController.CombatState.NonCombat;
            yield return null;
        }
        
        // draws wireframe for attack point to make it easier to set attack range
        private void OnDrawGizmosSelected()
        {
            var offset = new Vector2(10, 3);
            if (attackOrigin == null)
            {
                return;
            }
            Gizmos.DrawWireSphere((Vector2) attackOrigin.position + offset , 
                 lungeRange);
        }
    }
}