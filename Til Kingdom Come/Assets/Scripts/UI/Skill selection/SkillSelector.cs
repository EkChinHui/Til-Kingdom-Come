using System.Collections.Generic;
using GamePlay;
using TMPro;
using UI.Skill_selection;
using UnityEngine;
using UnityEngine.UI;

namespace UI
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
        }

        private void SpawnSkillCell(SkillCell skill)
        {
            GameObject skillCell = Instantiate(skillCellPrefab, transform);

            Image skillIcon = skillCell.transform.Find("Skill Icon").GetComponent<Image>();
            TextMeshProUGUI skillName = skillCell.transform.Find("Skill name").GetComponent<TextMeshProUGUI>();

            skillIcon.sprite = skill.skillIcon;
            skillName.text = skill.skillName;
        }

        public void SelectSkillPlayerOne(int skillNo)
        {
            playerOneSkill = skillNo;
        }

        public void SelectSkillPlayerTwo(int skillNo)
        {
            playerTwoSkill = skillNo;
        }

        public void PassSkills()
        {
            SkillSelectionManager.instance.assignedPlayerSkills.Add(playerOneSkill);
            SkillSelectionManager.instance.assignedPlayerSkills.Add(playerTwoSkill);
            // resets the total players to 0
            PlayerController.totalPlayers = 0;
        }
        
    }
}