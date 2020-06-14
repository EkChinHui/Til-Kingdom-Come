using System.Collections;
using UnityEngine;

namespace GamePlay.Skills
{
    public class ForcePush : Skill
    {
        [SerializeField] private float pushDistance = 6f;
        [SerializeField] private float stunDuration = 0.2f;

        private void Start()
        {
            skillName = "Force Push";
            skillInfo = "Pushes the enemy away from you";
            skillNumber = 0;
            skillCooldown = 10f;
        }


        public override void Cast(PlayerController player, PlayerController opponent)
        {
            if (!CanCast()) return;
            Debug.Log($"Player {player.playerNo} used {skillName}");
            StartCoroutine(AnimDelay(player));
            opponent.KnockBack(pushDistance);
            StartCoroutine(Stun(opponent));
            StartCoroutine(player.cooldownUiController.skillIcon.ChangesFillAmount(skillCooldown));
            
        }
        private IEnumerator AnimDelay(PlayerController player)
        {
            player.combatState = PlayerController.CombatState.Skill;
            var forcePushAnim= AnimationTimes.instance.ForcePushAnim;
            player.anim.SetTrigger(skillName);
            yield return new WaitForSeconds(forcePushAnim);
            player.combatState = PlayerController.CombatState.NonCombatState;
        }
        
        
        private IEnumerator Stun(PlayerController opponent)
        {
            yield return new WaitForSeconds(stunDuration);
        }
    }
}
