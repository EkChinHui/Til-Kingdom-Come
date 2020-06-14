using System;
using System.Collections;
using UnityEngine;

namespace GamePlay.Skills
{
    public class ThrowKnives : Skill
    {
        public Transform rangePoint;
        public GameObject knife;
        private float knifeDelay = 0.4f;
        public float heightOffset;
        public float distanceOffset;

        // Start is called before the first frame update
        void Start()
        {
            skillName = "Throw Knives"; 
            skillInfo = "Throws knives at opponent";
            skillNumber = 2;
            skillCooldown = 1f;
        }



        public override void Cast(PlayerController player, PlayerController opponent)
        {
            if (!CanCast()) return;
            rangePoint = player.transform;
            Debug.Log($"Player {player.playerNo} used {skillName}");
            AudioManager.instance.Play("Throw Knife");
            StartCoroutine(AnimDelay(player));
            StartCoroutine(player.cooldownUiController.skillIcon.ChangesFillAmount(skillCooldown));
        }
        
        
        private IEnumerator AnimDelay(PlayerController player)
        {
            var animDelay = AnimationTimes.instance.ThrowKnivesAnim;
            player.combatState = PlayerController.CombatState.Skill;
            player.anim.SetTrigger(skillName);
            yield return new WaitForSeconds(knifeDelay);
            var tempOffset = distanceOffset;
            if (Math.Abs(rangePoint.rotation.eulerAngles.y - 180) < Mathf.Epsilon)
            {
                tempOffset = -distanceOffset;
            }
            
            var pos = new Vector3(tempOffset, heightOffset, 0);
            Instantiate(knife, rangePoint.position + pos, rangePoint.rotation);
            yield return new WaitForSeconds(animDelay - knifeDelay);
            player.combatState = PlayerController.CombatState.NonCombatState;
        }

    }
}
