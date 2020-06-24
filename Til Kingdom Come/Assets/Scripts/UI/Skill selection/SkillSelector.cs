using System.Collections.Generic;
using GamePlay.Information;
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
        public List<GameObject> skillCells;
        private int playerOneSkill;
        private int playerTwoSkill;
        public KeyCode playerOneLeft;
        public KeyCode playerOneRight;
        public KeyCode playerTwoLeft;
        public KeyCode playerTwoRight;

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

            playerOneSkill = 0;
            playerTwoSkill = 0;
            SetBorder(playerOneSkill,1);
            SetBorder(playerTwoSkill, 2);
        }

        private void Update()
        {
            var maxSkills = skillPrefabs.Count - 1;
            
            if (Input.GetKeyDown(playerOneLeft))
            {
                ClearBorder(playerOneSkill, 1);
                playerOneSkill = playerOneSkill == 0 ? maxSkills: playerOneSkill - 1;
                SetBorder(playerOneSkill, 1);
            } else if (Input.GetKeyDown(playerOneRight))
            {
                print(maxSkills);
                ClearBorder(playerOneSkill, 1);
                playerOneSkill = playerOneSkill == maxSkills ? 0 : playerOneSkill + 1;
                print(playerOneSkill);
                SetBorder(playerOneSkill, 1);
            }

            if (Input.GetKeyDown(playerTwoLeft))
            {
                ClearBorder(playerTwoSkill, 2);
                playerTwoSkill = playerTwoSkill == 0 ? maxSkills : playerTwoSkill - 1;
                SetBorder(playerTwoSkill, 2);
            } else if (Input.GetKeyDown(playerTwoRight))
            {
                ClearBorder(playerTwoSkill, 2);
                playerTwoSkill = playerTwoSkill == maxSkills ? 0 : playerTwoSkill + 1;
                SetBorder(playerTwoSkill, 2);
            }
            

        }

        private void SetBorder(int skill, int playerNo)
        {
            var border = "Border" + playerNo;
            var image = GetBorderByName(border, skillCells[skill]);
            switch (playerNo)
            {
                case 1:
                    image.color = Color.red;
                    break;
                case 2:
                    image.color = Color.blue;
                    break;
            }
        }

        private void ClearBorder(int skill, int playerNo)
        {
            var border = "Border" + playerNo;
            var image = GetBorderByName(border, skillCells[skill]);
            image.color = Color.clear;
            
        }

        private Image GetBorderByName(string componentName, GameObject go)
        {
            foreach (var border in go.GetComponentsInChildren<Image>())
            {
                if (border.gameObject.name == componentName)
                {
                    return border;
                }
            }
            return null;
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
            skillCells.Add(skillCell);
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