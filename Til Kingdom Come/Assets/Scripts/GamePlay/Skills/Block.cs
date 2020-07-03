using System.Collections;
using GamePlay.Player;
using UI.GameUI.Cooldown;
using UnityEngine;

namespace GamePlay.Skills
{
    public class Block : Skill
    {
        private int maxCharges = 2;
        private float chargeTime = 4f;
        public Charges charges;
        private void Start()
        {
            player = gameObject.GetComponentInParent<PlayerController>();
            skillName = "Block";
            skillInfo = "Blocks various attacks";
            charges = gameObject.AddComponent<Charges>();
            charges.SetCharges(maxCharges, chargeTime);
        }
        
        private void Update()
        {
            player.cooldownUiController.blockIcon.gameObject.GetComponent<DisplayCharges>().UpdateCharges(charges.CurrentCharge);
            if (charges.isCharging && charges.CurrentCharge < maxCharges)
            {
                charges.isCharging = false;
                Debug.Log("Block Charging");
                StartCoroutine(player.cooldownUiController.blockIcon.ChangesFillAmount(chargeTime));
            }
        }
        
        public override void Cast(PlayerController opponent)
        {
            if (!CanCast()) return;
            if (charges.CurrentCharge <= 0) return;

            // Trigger cooldown UI
            if (charges.CurrentCharge == maxCharges)
            {
                Debug.Log("Block Charging");
                charges.isCharging = true;
            }

            charges.CurrentCharge--;
            StartCoroutine(BlockAnimDelay(player));
        }
        
        private IEnumerator BlockAnimDelay(PlayerController player)
        {
            player.combatState = PlayerController.CombatState.Blocking;
            player.playerInput.Toggle();
            player.anim.SetTrigger("Block");
            yield return new WaitForSeconds(AnimationTimes.instance.BlockAnim);
            player.combatState = PlayerController.CombatState.NonCombat;
            player.playerInput.Toggle();
        }
    }
}