using UnityEngine;

namespace UI.Skill_selection
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "New Skill Cell")]
    public class SkillCell : ScriptableObject
    {
        public string skillName;
        public string skillInfo;
        public string skillCooldown;
        public Sprite skillIcon;
        public int skillNumber;
    }
}
