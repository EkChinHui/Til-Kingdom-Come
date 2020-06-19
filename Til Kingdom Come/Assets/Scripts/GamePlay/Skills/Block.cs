using System.Collections;
using UI.GameUI;
using UnityEngine;

namespace GamePlay.Skills
{
    public class Block : Skill
    {
        private const float BlockCooldown = 0f;
        public Charges charges;
        private void Start()
        {
            player = gameObject.GetComponentInParent<PlayerController>();
            skillName = "Block";
            skillInfo = "Blocks various attacks";
            skillCooldown = BlockCooldown;
            charges = gameObject.AddComponent<Charges>();
            charges.SetCharges(3, 5f);
        }
        
        private void Update()
        {
            player.cooldownUiController.blockIcon.gameObject.GetComponent<DisplayCharges>()
                .UpdateCharges(charges.CurrentCharge);
        }
        
        public override void Cast(PlayerController opponent)
        {
            if (!CanCast()) return;
            if (charges.CurrentCharge <= 0) return;
            charges.CurrentCharge--;

            StartCoroutine(BlockAnimDelay(player));
            StartCoroutine(player.cooldownUiController.blockIcon.ChangesFillAmount(skillCooldown));
        }
        
        private IEnumerator BlockAnimDelay(PlayerController player)
        {
            player.combatState = PlayerController.CombatState.Blocking;
            player.playerInput.Toggle();
            player.anim.SetTrigger("Block");
            yield return new WaitForSeconds(AnimationTimes.instance.BlockAnim);
            player.combatState = PlayerController.CombatState.NonCombatState;
            player.playerInput.Toggle();
        }
    }
}