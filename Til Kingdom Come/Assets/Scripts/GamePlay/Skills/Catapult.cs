using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamePlay.Skills
{
    public class Catapult : Skill
    {
        public GameObject boulder;
        private int numberOfBoulders = 3;
        // Start is called before the first frame update
        void Start()
        {
            skillName = "Catapult";
            skillInfo = "Calls in a catapult strike on the opponent";
            skillNumber = 3;
        }
        public override void Cast(PlayerController player, PlayerController opponent)
        {
            if (!CanCast()) return;
            StartCoroutine(AnimDelay(player, opponent));
            StartCoroutine(player.cooldownUiController.skillIcon.ChangesFillAmount(skillCooldown));
        }
        private IEnumerator AnimDelay(PlayerController player, PlayerController opponent)
        {
            var animDelay = AnimationTimes.instance.CatapultAnim;
            player.combatState = PlayerController.CombatState.Skill;
            player.anim.SetTrigger(skillName);
            yield return new WaitForSeconds(animDelay);

            var enemyPosition = opponent.transform.position;
            for (int i = 0; i < numberOfBoulders; i++)
            {
                var x = Random.Range(-5f, 5f);
                var y = Random.Range(30f, 50f);
                var boulderSpawnPosition = enemyPosition + new Vector3(x, y, 0);
                Instantiate(boulder, boulderSpawnPosition, Quaternion.identity);
            }
            player.combatState = PlayerController.CombatState.NonCombatState;
        }
    }
}

