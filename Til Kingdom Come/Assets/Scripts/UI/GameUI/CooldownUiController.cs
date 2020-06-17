using GamePlay;
using GamePlay.Skills;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameUI
{
    public class CooldownUiController : MonoBehaviour
    {
        public CooldownUi attackIcon;
        public CooldownUi blockIcon;
        public CooldownUi rollIcon;
        public CooldownUi skillIcon;
        public int playerNo;

        private void Awake()
        {
            SkillSelectionManager.passPlayerSkills += SetSkillIcon;
        }

        private void SetSkillIcon(int player, GameObject skill)
        {
            if (player != playerNo) return;
            Skill chosenSkill = skill.GetComponent<Skill>();
            Sprite icon = chosenSkill.icon;
            skillIcon.image.sprite = icon;
            var darkMask = skillIcon.GetComponentInChildren<Image>();
            darkMask.sprite = icon;
            skillIcon.darkMask.sprite = darkMask.sprite;
        }

        private void OnDestroy()
        {
            SkillSelectionManager.passPlayerSkills -= SetSkillIcon;
        }
    }
    
}
