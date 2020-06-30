using System.Collections;
using GamePlay.Player;
using UnityEngine;

namespace GamePlay.Skills
{
    public class Fireball : Skill
    {
        // Start is called before the first frame update
        void Start()
        {
            skillName = "Fireball";
            skillInfo = "Throws a fireball burning the ground where the fireball lands";
            skillCooldown = 10f;
            skillNumber = 4;
        }

        public override void Cast(PlayerController opponent)
        {
            if (!CanCast()) return;
            StartCoroutine(AnimDelay());
        }

        private IEnumerator AnimDelay()
        {
            var animDelay = AnimationTimes.instance.FireBallAnim;
            player.combatState = PlayerController.CombatState.Skill;
            yield return new WaitForSeconds(animDelay);
            player.combatState = PlayerController.CombatState.NonCombat;
            yield return null;
        }
    }
}
