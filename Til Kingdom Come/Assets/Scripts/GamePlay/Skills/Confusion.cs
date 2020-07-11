using System;
using System.Collections;
using GamePlay.Player;
using UnityEngine;

namespace GamePlay.Skills
{
    public class Confusion : Skill
    {
        private float pushDistance = 6f;
        private float confusionDuration = 4f;

        private void Start()
        { 
            skillName = "Confusion";
            skillInfo = "Confuse the enemy, reversing the controls of the enemy";
            skillNumber = 1;
            skillCooldown = 8f;
        }

        public override void Cast(PlayerController opponent)
        {
            if (!CanCast()) return;

            

            StartCoroutine(AnimDelay());
            Debug.Log($"Player {player.playerNo} used {skillName}");
            AudioManager.instance.PlaySoundEffect("Force Pull");
            if (isFacingOppponent(player, opponent))
            {
                opponent.KnockBack(pushDistance);
                StartCoroutine(Confuse(opponent));
            }
            StartCoroutine(player.cooldownUiController.skillIcon.ChangesFillAmount(skillCooldown));
        }

        private bool isFacingOppponent(PlayerController player, PlayerController opponent)
        {
            float playerDirection = player.transform.rotation.eulerAngles.y; // either 0 or 180
            float playerPositionX = player.transform.position.x;
            float opponentPositionX = opponent.transform.position.x;
            return (playerPositionX > opponentPositionX && Math.Abs(playerDirection - 180) < Mathf.Epsilon) ||
                   (playerPositionX < opponentPositionX && Math.Abs(playerDirection) < Mathf.Epsilon);

        }

        private IEnumerator AnimDelay()
        {
            player.combatState = PlayerController.CombatState.Skill;
            var animDelay = AnimationTimes.instance.ConfusionAnim;
            player.anim.SetTrigger(skillName);
            yield return new WaitForSeconds(animDelay);
            player.combatState = PlayerController.CombatState.NonCombat;
            yield return null;
        }

        private IEnumerator Confuse(PlayerController opponent)
        {
            opponent.playerInput.InvertKeys();
            var heightOffset = new Vector3(0, 4f, 0);
            ParticleSystem confusion = opponent.confusion.GetComponent<ParticleSystem>();
            var confusionParticle = Instantiate(opponent.confusion, 
                opponent.transform.position + heightOffset, Quaternion.identity);
            confusionParticle.transform.parent = opponent.transform;
            yield return new WaitForSeconds(confusionDuration);
            Destroy(confusionParticle.gameObject); 
            opponent.playerInput.InvertKeys();
            yield return null;
        }
    }
}
