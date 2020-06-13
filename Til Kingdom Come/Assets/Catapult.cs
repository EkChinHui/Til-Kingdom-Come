using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GamePlay.Skills
{
    public class Catapult : Skill
    {
        public GameObject boulder;
        // Start is called before the first frame update
        void Start()
        {
            skillName = "Catapult";
            skillInfo = "Calls in a catapult strike on the opponent";
            skillCooldown = 5f;
        }
        public override void Cast(PlayerController player, PlayerController opponent)
        {
            if (!CanCast()) return;

            StartCoroutine(AnimDelay(player, opponent));
        }
        private IEnumerator AnimDelay(PlayerController player, PlayerController opponent)
        {
            var animDelay = AnimationTimes.instance.CatapultAnim;
            player.combatState = PlayerController.CombatState.Skill;
            player.anim.SetTrigger(skillName);
            yield return new WaitForSeconds(animDelay);

            var enemyPosition = opponent.transform.position;
            var boulderSpawnPosition = enemyPosition + new Vector3(0, 10, 0);
            Instantiate(boulder, boulderSpawnPosition, Quaternion.identity);
            player.combatState = PlayerController.CombatState.NonCombatState;
        }
    }
}

