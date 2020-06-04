using System;
using System.Collections;
using UnityEngine;

namespace GamePlay.Skills
{
    public class Block : Skill
    {
        private const float BlockCooldown = 0.4f;
        private void Start()
        {
            skillName = "Block";
            skillInfo = "Blocks various attacks";
            skillCooldown = BlockCooldown;
        }

        public override void Cast(PlayerController player, PlayerController opponent)
        {
            if (!CanCast()) return;
            StartCoroutine(BlockAnimDelay(player));
        }
        
        private IEnumerator BlockAnimDelay(PlayerController player)
        {
            player.anim.SetTrigger("Block");
            player.isBlocking = true;
            player.isActing = true;
            yield return new WaitForSeconds(AnimationTimes.instance.BlockAnim);
            player.isSilenced = true;
            player.isBlocking = false;
            yield return new WaitForSeconds(BlockCooldown);
            player.isSilenced = false;
            player.isActing = false;
        }
    }
}