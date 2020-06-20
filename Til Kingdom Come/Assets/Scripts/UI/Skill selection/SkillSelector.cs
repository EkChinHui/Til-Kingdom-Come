using System.Collections.Generic;
using GamePlay;
using GamePlay.Player;
using GamePlay.Skills;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Skill_selection
{
    public class SkillSelector : MonoBehaviour
    {
        public List<GameObject> skillPrefabs;
        public GameObject skillCellPrefab;
        private int playerOneSkill = 0;
        private int playerTwoSkill = 0;

        private void Awake()
        {
            CursorDetection.onSkillSelect += SelectSkills;
        }

        private void Start()
        {
            SkillSelectionManager.instance.assignedPlayerSkills.Clear();

            foreach (var skills in skillPrefabs)
            {
                SpawnSkillCell(skills);
            }
        }

        private void SpawnSkillCell(GameObject go)
        {
            GameObject skillCell = Instantiate(skillCellPrefab, transform);
            var skill = go.GetComponent<Skill>();
            skillCell.name = skill.SkillName;
            
            TextMeshProUGUI skillName = skillCell.transform.Find("Skill Name").GetComponent<TextMeshProUGUI>();
            Image skillIcon = skillCell.transform.Find("Skill Icon").GetComponent<Image>();
            TextMeshProUGUI skillInfo = skillCell.transform.Find("Skill Info").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI skillCooldown = skillCell.transform.Find("Skill Cooldown").GetComponent<TextMeshProUGUI>();
            SkillNumber skillNumber = skillCell.GetComponent<SkillNumber>();
            
            skillName.text = skill.SkillName;
            skillIcon.sprite = skill.icon;
            skillInfo.text = skill.SkillInfo;
            skillCooldown.text = "Cooldown: " + skill.SkillCooldown + "s";
            skillNumber.skillNumber = skill.SkillNumber;
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