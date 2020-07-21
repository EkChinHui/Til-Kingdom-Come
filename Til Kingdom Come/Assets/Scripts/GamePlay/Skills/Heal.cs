using System;
using System.Collections;
using GamePlay.Player;
using UnityEngine;

namespace GamePlay.Skills
{
    public class Heal : Skill
    {
        public GameObject glow;
        public GameObject healParticles;
        public int healAmount = 40;
        public float healDuration = 5f;
        private bool isHealing;

        void Start()
        {
            skillName = "Heal";
            skillInfo = "Heals the player";
            skillCooldown = 10f;
            skillNumber = 3;
        }

        private void Update()
        {
            if (isHealing)
            {
                var rateIncrease = healAmount / healDuration;
                player.currentHealth += rateIncrease * Time.deltaTime;
                player.currentHealth = Math.Min(player.currentHealth, 100);
                player.healthBarController.SetHealth(player.currentHealth);
            }
        }

        public override void Cast(PlayerController opponent)
        {
            if (!CanCast()) return;
            StartCoroutine(AnimDelay());
            StartCoroutine(HealOverTime());
            StartCoroutine(player.cooldownUiController.skillIcon.ChangesFillAmount(skillCooldown));
        }

        private IEnumerator AnimDelay()
        {
            player.combatState = PlayerController.CombatState.Skill;
            var animDelay = AnimationTimes.instance.HealAnim;
            player.anim.SetTrigger(skillName);
            yield return new WaitForSeconds(animDelay);
            player.combatState = PlayerController.CombatState.NonCombat;
            yield return null;
        }

        private IEnumerator HealOverTime()
        {
            isHealing = true;
            var heightOffset = new Vector3(0, 4f, 0);
            var glowOffset = new Vector3(0, 0.5f, 0);
            var glowParticle = Instantiate(glow, player.transform.position + glowOffset,
                Quaternion.identity);
            var healParticle = Instantiate(healParticles, player.transform.position + heightOffset, 
                Quaternion.identity);
            glowParticle.transform.parent = player.transform;
            healParticle.transform.parent = player.transform;
            yield return new WaitForSeconds(healDuration);
            isHealing = false;
            yield return null;
        }
        
    }
}
