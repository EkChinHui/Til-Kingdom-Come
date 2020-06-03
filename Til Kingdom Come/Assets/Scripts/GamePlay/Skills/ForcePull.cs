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
            skillCooldown = 1f;
        }

        public override void Cast(PlayerController player, PlayerController opponent)
        {
            if (!CanCast()) return;

            StartCoroutine(AnimDelay(player));

            int pullDirection = opponent.transform.position.x - player.transform.position.x > 0 ? 1 : -1;
            opponent.KnockBack(pullDirection * pullDistance);
            StartCoroutine(Stun(opponent));
       
        }

        private IEnumerator AnimDelay(PlayerController player)
        {
            var animDelay = AnimationTimes.instance.ForcePullAnim;
            player.anim.SetTrigger(skillName);
            player.isActing = true;
            yield return new WaitForSeconds(animDelay);
            player.isActing = false;
        }

        private IEnumerator Stun(PlayerController opponent)
        {
            opponent.isActing = true;
            yield return new WaitForSeconds(stunDuration);
            opponent.isActing = false;
            yield return null;
        }
    }
}
