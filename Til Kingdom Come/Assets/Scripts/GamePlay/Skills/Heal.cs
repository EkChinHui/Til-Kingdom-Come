using System;
using System.Collections;
using GamePlay.Player;
using UnityEngine;

namespace GamePlay.Skills
{
    public class Heal : Skill
    {
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
                print(rateIncrease * Time.deltaTime);
                player.currentHealth += rateIncrease * Time.deltaTime;
                print("Player health: " + player.currentHealth);
                player.currentHealth = Math.Min(player.currentHealth, 100);
            }
        }

        public override void Cast(PlayerController opponent)
        {
            if (!CanCast()) return;
            StartCoroutine(AnimDelay());
            StartCoroutine(HealOverTime());
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
            yield return new WaitForSeconds(healDuration);
            isHealing = false;
            yield return null;
        }
        
    }
}
