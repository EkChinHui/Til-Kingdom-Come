using UnityEngine;

namespace GamePlay.Skills
{
    public abstract class Skill : MonoBehaviour
    {
        [SerializeField] protected string skillName;
        [SerializeField] protected string skillInfo;
        protected float skillCooldown;
        private float nextAvailTime;

        public float SkillCooldown => skillCooldown;

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

        public float TimeLeft()
        {
            return nextAvailTime - Time.time;
        }

        public abstract void Cast(PlayerController player, PlayerController opponent);

    }
}
