using GamePlay.Player;
using UnityEngine;

namespace GamePlay.Skills
{
    public abstract class Skill : MonoBehaviour
    {
        protected PlayerController player;
        [SerializeField] protected string skillName;
        [SerializeField] protected string skillInfo;
        public Sprite icon;
        [SerializeField] protected float skillCooldown;
        private float nextAvailTime;
        [SerializeField] protected int skillNumber;

        #region getters
        public string SkillName => skillName;
        public string SkillInfo => skillInfo;
        public float SkillCooldown => skillCooldown;
        public int SkillNumber => skillNumber;
        #endregion

        
        protected bool CanCast()
        {
            if (Time.time < nextAvailTime) return false;
            nextAvailTime = skillCooldown + Time.time;
            return true;
        }

        public abstract void Cast(PlayerController opponent);

        public void AssignPlayer(PlayerController player)
        {
            this.player = player;
        }
        
    }
}
