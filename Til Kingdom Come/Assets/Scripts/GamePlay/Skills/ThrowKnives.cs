﻿using System.Collections;
using UnityEngine;

namespace GamePlay.Skills
{
    public class ThrowKnives : Skill
    {
        public Transform rangePoint;
        public GameObject knife;
        private float knifeDelay = 0.4f;

        // Start is called before the first frame update
        void Start()
        {
            rangePoint = gameObject.transform.GetChild(1);
            skillName = "Throw Knives"; 
            skillInfo = "Throws knives at opponent";
            skillCooldown = 10f;
        }



        public override void Cast(PlayerController player, PlayerController opponent)
        {
            if (!CanCast()) return;

            StartCoroutine(AnimDelay(player));



        }
        
        
        private IEnumerator AnimDelay(PlayerController player)
        {
            var animDelay = AnimationTimes.instance.ThrowKnivesAnim;
            player.anim.SetTrigger(skillName);
            player.isActing = true;
            yield return new WaitForSeconds(knifeDelay);
            Instantiate(knife, rangePoint.position, rangePoint.rotation);
            yield return new WaitForSeconds(animDelay - knifeDelay);
            player.isActing = false;

        }

    }
}
