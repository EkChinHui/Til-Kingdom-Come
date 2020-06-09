using System;
using System.Collections;
using UnityEngine;

namespace GamePlay.Skills
{
    public class Roll : Skill
    {
        public bool dashing;
        public float rollSpeed = 1.5f;
        private const float RollCooldown = 2f;
        private PlayerController playerController;
        private void Start()
        {
            skillName = "Roll";
            skillInfo = "Rolls a short distance, invulnerable while rolling";
            skillCooldown = RollCooldown;
        }

        private void FixedUpdate()
        {
            if (dashing)
            {
                RollMove(playerController);
            }

        }

        public override void Cast(PlayerController player, PlayerController opponent)
        {
            if (!CanCast()) return;
            playerController = player;
            AudioManager.instance.Play("Roll");
            FlipSprite(player);
            StartCoroutine(RollAnimDelay(player));
            StartCoroutine(player.cooldownUiController.rollIcon.ChangesFillAmount(skillCooldown));
        }

        private void RollMove(PlayerController player)
        {
            if (Math.Abs(transform.rotation.y) > Mathf.Epsilon)
            {
                player.rb.AddForce(transform.right * rollSpeed, ForceMode2D.Impulse);
            }
            else if (Math.Abs(transform.rotation.y) < Mathf.Epsilon)
            {
                player.rb.AddForce(transform.right * rollSpeed, ForceMode2D.Impulse);
            }
        }

        private IEnumerator RollAnimDelay(PlayerController player)
        {
            player.combatState = PlayerController.CombatState.Rolling;
            dashing = true;
            player.anim.SetTrigger("Roll");
            yield return new WaitForSeconds(AnimationTimes.instance.RollAnim);
            dashing = false;
            player.combatState = PlayerController.CombatState.NonCombatState;
            player.rb.velocity = Vector2.zero;
        }
        
        private void FlipSprite(PlayerController player)
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