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
            this.skillName = "Force Pull";
            this.skillDescription = "Pulls the enemy towards you";
            this.skillCooldown = 1f;
            skillName = "ForcePull";
        }

        public override void Cast(PlayerController player, PlayerController opponent)
        {
            int pullDirection = opponent.transform.position.x - player.transform.position.x > 0 ? 1 : -1;
            opponent.KnockBack(pullDirection * pullDistance);
            StartCoroutine(Stun(opponent));
       
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
