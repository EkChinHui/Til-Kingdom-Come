using UnityEngine;

namespace GamePlay.Skills
{
    public abstract class Skill : MonoBehaviour
    {
        [SerializeField] protected string skillName;
        [SerializeField] protected string skillInfo;
        public Sprite icon;
        protected float skillCooldown;
        private float nextAvailTime;

        public float SkillCooldown => skillCooldown;

        // getter
        public string SkillName => skillName;
        
        protected bool CanCast()
        {
            if (Time.time < nextAvailTime) return false;
            nextAvailTime = skillCooldown + Time.time;
            return true;

        }

        public float TimeLeft()
        {
            return nextAvailTime - Time.time;
        }

        public abstract void Cast(PlayerController player, PlayerController opponent);

    }
}
