using UnityEngine;

namespace GamePlay.Skills
{
    public abstract class Skill : MonoBehaviour
    {
        [SerializeField] protected string skillName;
        [SerializeField] protected string skillInfo;
        public Sprite icon;
        [SerializeField] protected float skillCooldown;
        protected float nextAvailTime;
        [SerializeField] protected int skillNumber;

        #region getters
        public string SkillName => skillName;
        public string SkillInfo => skillInfo;
        public float SkillCooldown => skillCooldown;
        public int SkillNumber => skillNumber;
        #endregion

        
        protected virtual bool CanCast()
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
