using System.Collections;
using UnityEngine;

namespace GamePlay.Skills
{
    public class ForcePull : Skill
    {
        [SerializeField] private float pullDistance = 6f;
        [SerializeField] private float stunDuration = 0.2f;


        private void Start()
        {
            skillName = "Force Pull";
            skillInfo = "Pulls the enemy towards you";
            skillCooldown = 10f;
        }

        public override void Cast(PlayerController player, PlayerController opponent)
        {
            if (!CanCast()) return;
            StartCoroutine(AnimDelay(player));
            Debug.Log($"Player {player.playerNo} used {skillName}");
            opponent.KnockBack(-1 * pullDistance);
            StartCoroutine(Stun(opponent));
            StartCoroutine(player.cooldownUiController.skillIcon.ChangesFillAmount(skillCooldown));
       
        }

        private IEnumerator AnimDelay(PlayerController player)
        {
            player.combatState = PlayerController.CombatState.Skill;
            var animDelay = AnimationTimes.instance.ForcePullAnim;
            player.anim.SetTrigger(skillName);
            yield return new WaitForSeconds(animDelay);
            player.combatState = PlayerController.CombatState.NonCombatState;
        }

        private IEnumerator Stun(PlayerController opponent)
        {
            yield return new WaitForSeconds(stunDuration);
            yield return null;
        }
    }
}
