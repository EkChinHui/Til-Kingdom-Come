using System.Collections.Generic;
using GamePlay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Skill_selection
{
    public class SkillSelector : MonoBehaviour
    {
        public List<SkillCell> allCells = new List<SkillCell>();
        public GameObject skillCellPrefab;
        private int playerOneSkill = 0;
        private int playerTwoSkill = 0;

        private void Start()
        {
            SkillSelectionManager.instance.assignedPlayerSkills.Clear();
            foreach (var skill in allCells)
            {
                SpawnSkillCell(skill);
            }

            CursorDetection.onSkillSelect += SelectSkills;
        }
        
        private void SpawnSkillCell(SkillCell skill)
        {
            GameObject skillCell = Instantiate(skillCellPrefab, transform);

            skillCell.name = skill.skillName;

            Image skillIcon = skillCell.transform.Find("Skill Icon").GetComponent<Image>();
            TextMeshProUGUI skillName = skillCell.transform.Find("Skill name").GetComponent<TextMeshProUGUI>();

            skillIcon.sprite = skill.skillIcon;
            skillName.text = skill.skillName;
            SkillNumber skillNumber = skillCell.GetComponent<SkillNumber>();
            skillNumber.skillNumber = skill.skillNumber;
        }
        
        private void SelectSkills(int playerNo, int skillNo)
        {
            switch (playerNo)
            {
                case 1:
                    playerOneSkill = skillNo;
                    break;
                case 2:
                    playerTwoSkill = skillNo;
                    break;
            }
        }

        public void PassSkills()
        {
            SkillSelectionManager.instance.assignedPlayerSkills.Add(playerOneSkill);
            SkillSelectionManager.instance.assignedPlayerSkills.Add(playerTwoSkill);
            // resets the total players to 0
            PlayerController.totalPlayers = 0;
        }

        private void OnDestroy()
        {
            CursorDetection.onSkillSelect -= SelectSkills;
        }
    }
}