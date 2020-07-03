using System;
using System.Collections;
using GamePlay.Player;
using UnityEngine;

namespace GamePlay.Skills
{
    public class Roll : Skill
    {
        public bool dashing;
        private float rollSpeed = 1.5f;
        private const float rollCooldown = 5f;
        private void Start()
        {
            player = gameObject.GetComponentInParent<PlayerController>();
            skillName = "Roll";
            skillInfo = "Rolls a short distance, invulnerable while rolling";
            skillCooldown = rollCooldown;
        }

        private void FixedUpdate()
        {
            if (dashing)
            {
                RollMove();
            }

        }

        public override void Cast(PlayerController opponent)
        {
            if (!CanCast()) return;
            AudioManager.instance.PlaySoundEffect("Roll");
            FlipSprite();
            StartCoroutine(RollAnimDelay());
            StartCoroutine(player.cooldownUiController.rollIcon.ChangesFillAmount(skillCooldown));
        }

        private void RollMove()
        {
            if (Math.Abs(transform.rotation.y) > Mathf.Epsilon)
            {
                player.rb.AddForce(player.transform.right * rollSpeed, ForceMode2D.Impulse);
            }
            else if (Math.Abs(transform.rotation.y) < Mathf.Epsilon)
            {
                player.rb.AddForce(player.transform.right * rollSpeed, ForceMode2D.Impulse);
            }
        }

        private IEnumerator RollAnimDelay()
        {
            player.combatState = PlayerController.CombatState.Rolling;
            dashing = true;
            player.anim.SetTrigger("Roll");
            yield return new WaitForSeconds(AnimationTimes.instance.RollAnim);
            dashing = false;
            player.combatState = PlayerController.CombatState.NonCombat;
            player.rb.velocity = Vector2.zero;
        }
        
        private void FlipSprite()
        {
            if (player.playerInput.AttemptRight)
            {
                if (Math.Abs(transform.rotation.y) > Mathf.Epsilon)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
            else if (player.playerInput.AttemptLeft)
            {
                if (Math.Abs(transform.rotation.y) < Mathf.Epsilon)
                {
                    transform.rotation = Quaternion.Euler(0, 180f, 0);
                }
            }
        }
    }
}