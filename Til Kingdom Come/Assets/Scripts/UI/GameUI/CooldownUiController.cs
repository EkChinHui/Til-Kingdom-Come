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
            var darkmask = skillIcon.GetComponentInChildren<Image>();
            Debug.Log(darkmask.name);
            darkmask.sprite = icon;
            skillIcon.darkMask.sprite = darkmask.sprite;
        }

        private void OnDestroy()
        {
            SkillSelectionManager.passPlayerSkills -= SetSkillIcon;
        }
    }
    
}
