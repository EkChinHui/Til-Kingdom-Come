using System.Collections;
using GamePlay.Player;
using UnityEngine;

namespace GamePlay.Skills
{
    public class Heal : Skill
    {
        void Start()
        {
            skillName = "Heal";
            skillInfo = "Heals the player";
            skillCooldown = 10f;
            skillNumber = 3;
        }

        public override void Cast(PlayerController opponent)
        {
            if (!CanCast()) return;
        }

        private IEnumerator AnimDelay()
        {
            player.combatState = PlayerController.CombatState.Skill;
            //var animDelay = AnimationTimes.instance.ConfusionAnim;
            player.anim.SetTrigger(skillName);
            //yield return new WaitForSeconds(animDelay);
            player.combatState = PlayerController.CombatState.NonCombatState;
            yield return null;
        }
    }
}
