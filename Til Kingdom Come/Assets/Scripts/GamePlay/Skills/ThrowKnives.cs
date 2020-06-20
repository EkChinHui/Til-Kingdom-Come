using System;
using System.Collections;
using UI.GameUI;
using UnityEngine;

namespace GamePlay.Skills
{
    public class ThrowKnives : Skill
    {
        public Transform rangePoint;
        public GameObject knife;
        private float knifeDelay = 0.6f;
        public float heightOffset;
        public float distanceOffset;
        public Charges charges;
        public int maxCharges = 2;
        public float chargeTime = 5f;
        
        void Start()
        {
            skillName = "Throw Knives"; 
            skillInfo = "Throws knives at opponent";
            skillNumber = 2;
            skillCooldown = 0f;
            charges = gameObject.AddComponent<Charges>();
            charges.SetCharges(maxCharges, chargeTime);
        }
        
        private void Update()
        {
            player.cooldownUiController.skillIcon.gameObject.GetComponent<DisplayCharges>().UpdateCharges(charges.CurrentCharge);
            if (charges.isCharging && charges.CurrentCharge < maxCharges)
            {
                charges.isCharging = false;
                Debug.Log("Throw Knives Charging");
                StartCoroutine(player.cooldownUiController.skillIcon.ChangesFillAmount(chargeTime));
            }
        }

        public override void Cast(PlayerController opponent)
        {
            if (!CanCast()) return;
            if (charges.CurrentCharge <= 0) {
                print("0 charges left");
                return;
            }

            // Trigger cooldown UI
            if (charges.CurrentCharge == maxCharges)
            {
                Debug.Log("Block Charging");
                charges.isCharging = true;
            }

            charges.CurrentCharge -= 1;
            rangePoint = player.transform;
            Debug.Log($"Player {player.playerNo} used {skillName}");
            AudioManager.instance.PlaySoundEffect("Throw Knife");
            StartCoroutine(AnimDelay());
        }

        private IEnumerator AnimDelay()
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
