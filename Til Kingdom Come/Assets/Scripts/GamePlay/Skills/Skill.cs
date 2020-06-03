using System;
using UnityEngine;

namespace GamePlay.Skills
{
    public abstract class Skill : MonoBehaviour
    {
        protected string skillName;
        protected string skillDescription;
        protected float skillCooldown;
        private float nextAvailTime;

        private void Start()
        {
            
        }

        // getter
        public string SkillName => skillName;
        
        protected bool CanCast()
        {
            if (!(Time.time > nextAvailTime)) return false;
            nextAvailTime = skillCooldown + Time.time;
            return true;

        }

        public abstract void Cast(PlayerController player, PlayerController opponent);

    }
}
