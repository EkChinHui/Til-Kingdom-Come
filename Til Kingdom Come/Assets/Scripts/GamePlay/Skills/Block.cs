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
            StartCoroutine(player.cooldownUiController.blockIcon.ChangesFillAmount(skillCooldown));
        }
        
        private IEnumerator BlockAnimDelay(PlayerController player)
        {
            player.anim.SetTrigger("Block");
            player.combatState = PlayerController.CombatState.Blocking;
            yield return new WaitForSeconds(AnimationTimes.instance.BlockAnim);
            player.combatState = PlayerController.CombatState.NonCombatState;
        }
    }
}