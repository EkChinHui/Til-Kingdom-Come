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
            skillCooldown = 10f;
        }


        public override void Cast(PlayerController player, PlayerController opponent)
        {
            if (!CanCast()) return;

            StartCoroutine(AnimDelay(player));
            int pushDirection = opponent.transform.position.x - player.transform.position.x > 0 ? -1 : 1;
            opponent.KnockBack(pushDirection * pushDistance);
            StartCoroutine(Stun(opponent));
            EndCast();
            
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
