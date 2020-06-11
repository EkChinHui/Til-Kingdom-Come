using System.Linq.Expressions;
using UnityEngine;

namespace UI.Skill_selection
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "New Skill Cell")]
    public class SkillCell : ScriptableObject
    {
        public string skillName;
        public Sprite skillIcon;
        

    }
}
