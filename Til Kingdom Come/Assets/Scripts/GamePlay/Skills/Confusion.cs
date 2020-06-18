using System.Collections;
using UnityEngine;

namespace GamePlay.Skills
{
    public class Confusion : Skill
    {
        [SerializeField] private float pushDistance = 6f;
        [SerializeField] private float confusionDuration = 4f;


        private void Start()
        { 
            skillName = "Confusion";
            skillInfo = "Confuse the enemy, reversing the controls of the enemy";
            skillNumber = 1;
            skillCooldown = 10f;
        }

        public override void Cast(PlayerController opponent)
        {
            if (!CanCast()) return;
            StartCoroutine(AnimDelay());
            Debug.Log($"Player {player.playerNo} used {skillName}");
            AudioManager.instance.PlaySoundEffect("Force Pull");
            opponent.KnockBack(pushDistance);
            StartCoroutine(Confuse(opponent));
            StartCoroutine(player.cooldownUiController.skillIcon.ChangesFillAmount(skillCooldown));
       
        }

        private IEnumerator AnimDelay()
        {
            player.combatState = PlayerController.CombatState.Skill;
            var animDelay = AnimationTimes.instance.ConfusionAnim;
            player.anim.SetTrigger(skillName);
            yield return new WaitForSeconds(animDelay);
            player.combatState = PlayerController.CombatState.NonCombatState;
            yield return null;
        }

        private IEnumerator Confuse(PlayerController opponent)
        {
            opponent.playerInput.SwitchKeys();
            yield return new WaitForSeconds(confusionDuration);
            opponent.playerInput.SwitchKeys();
            yield return null;
        }
    }
}
