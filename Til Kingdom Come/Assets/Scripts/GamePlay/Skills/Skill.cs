using UnityEngine;

namespace GamePlay.Skills
{
    public abstract class Skill : MonoBehaviour
    {
        protected string skillName;
        protected string skillInfo;
        protected float skillCooldown;
        private float nextAvailTime;
        

        // getter
        public string SkillName => skillName;
        
        protected bool CanCast()
        {
            return Time.time > nextAvailTime;
        }

        protected void EndCast()
        {
            nextAvailTime = skillCooldown + Time.time;
        }

        public abstract void Cast(PlayerController player, PlayerController opponent);

    }
}
