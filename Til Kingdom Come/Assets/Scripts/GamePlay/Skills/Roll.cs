using System;
using System.Collections;
using UnityEngine;

namespace GamePlay.Skills
{
    public class Roll : Skill
    {
        private const float RollCooldown = 0.6f;
        private void Start()
        {
            skillName = "Roll";
            skillInfo = "Rolls a short distance, invulnerable while rolling";
            skillCooldown = RollCooldown;
        }

        public override void Cast(PlayerController player, PlayerController opponent)
        {
            if (!CanCast()) return;
            RollDirection(player);
            StartCoroutine(RollAnimDelay(player));
        }
        
        private void RollDirection(PlayerController player)
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

        private IEnumerator RollAnimDelay(PlayerController player)
        {
            player.canRoll = false;
            player.isActing = true;
            player.anim.SetTrigger("Roll");
            player.runSpeed = player.rollSpeed;
            player.Move();
            yield return new WaitForSeconds(AnimationTimes.instance.RollAnim);
            player.runSpeed = 4f; // FIX: replace this hardcoded value
            player.isSilenced = true;
            yield return new WaitForSeconds(RollCooldown);
            player.isSilenced = false;
            player.isActing = false;
            player.canRoll = true;
        }
    }
}