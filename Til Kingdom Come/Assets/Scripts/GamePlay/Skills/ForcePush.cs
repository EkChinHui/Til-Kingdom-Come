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
            skillDescription = "Pushes the enemy away from you";
            skillCooldown = 1f;
        }


        public override void Cast(PlayerController player, PlayerController opponent)
        {
            if (!CanCast()) return;
            int pushDirection = opponent.transform.position.x - player.transform.position.x > 0 ? -1 : 1;
            opponent.KnockBack(pushDirection * pushDistance);
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
