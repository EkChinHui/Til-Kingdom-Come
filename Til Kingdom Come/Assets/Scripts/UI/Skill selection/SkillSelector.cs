using System.Collections.Generic;
using GamePlay.Information;
using GamePlay.Player;
using GamePlay.Skills;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Skill_selection
{
    public class SkillSelector : MonoBehaviour
    {
        public List<GameObject> skillPrefabs;
        public GameObject skillCellTemplate;
        public List<GameObject> skillCells;
        private int playerOneSkill;
        private int playerTwoSkill;
        private int maxSkills;
        private PhotonView photonView;

        #region Keycodes

        public KeyCode playerOneLeft;
        public KeyCode playerOneRight;
        public KeyCode playerTwoLeft;
        public KeyCode playerTwoRight;

        #endregion


        private void Start()
        {
            SkillSelectionManager.instance.assignedPlayerSkills.Clear();
            photonView = GetComponent<PhotonView>();
            maxSkills = skillPrefabs.Count - 1;

            foreach (var skills in skillPrefabs)
            {
                SpawnSkillCell(skills);
            }

            // set initial border
            playerOneSkill = 0;
            playerTwoSkill = 0;
            SetBorder(playerOneSkill,1);
            SetBorder(playerTwoSkill, 2);
        }

        [PunRPC]
        private void IncreasePlayerOneSkill()
        {
            ClearBorder(playerOneSkill, 1);
            playerOneSkill = playerOneSkill == 0 ? maxSkills: playerOneSkill + 1;
            SetBorder(playerOneSkill, 1);
        }

        [PunRPC]
        private void DecreasePlayerOneSkill()
        {
            ClearBorder(playerOneSkill, 1);
            playerOneSkill = playerOneSkill == 0 ? maxSkills: playerOneSkill - 1;
            SetBorder(playerOneSkill, 1);
        }
        
        [PunRPC]
        private void IncreasePlayerTwoSkill()
        {
            ClearBorder(playerTwoSkill, 2);
            playerTwoSkill = playerTwoSkill == 0 ? maxSkills: playerTwoSkill + 1;
            SetBorder(playerTwoSkill, 2);
        }

        [PunRPC]
        private void DecreasePlayerTwoSkill()
        {
            ClearBorder(playerTwoSkill, 2);
            playerTwoSkill = playerTwoSkill == 0 ? maxSkills: playerTwoSkill - 1;
            SetBorder(playerTwoSkill, 2);
        }
        

        
        private void Update()
        {
            /*
             * On keypress, clears current border of color, then sets the border of
             * selected skill.
             * Ternary operator allows the selected border to loop to the first or last
             * skill.
             */

            if (PhotonNetwork.IsMasterClient)
            {
                // Change Player One skill
                if (Input.GetKeyDown(playerOneLeft))
                {
                    /*ClearBorder(playerOneSkill, 1);
                    playerOneSkill = playerOneSkill == 0 ? maxSkills: playerOneSkill - 1;
                    SetBorder(playerOneSkill, 1);*/
                    photonView.RPC("DecreasePlayerOneSkill", RpcTarget.All);

                } else if (Input.GetKeyDown(playerOneRight))
                {
                    /*ClearBorder(playerOneSkill, 1);
                    playerOneSkill = playerOneSkill == maxSkills ? 0 : playerOneSkill + 1;
                    SetBorder(playerOneSkill, 1);*/
                    photonView.RPC("IncreasePlayerOneSkill", RpcTarget.All);
                }
            }
            else
            {
                if (Input.GetKeyDown(playerOneLeft))
                {
                    /*ClearBorder(playerOneSkill, 1);
                    playerOneSkill = playerOneSkill == 0 ? maxSkills: playerOneSkill - 1;
                    SetBorder(playerOneSkill, 1);*/
                    photonView.RPC("DecreasePlayerTwoSkill", RpcTarget.All);

                } else if (Input.GetKeyDown(playerOneRight))
                {
                    /*ClearBorder(playerOneSkill, 1);
                    playerOneSkill = playerOneSkill == maxSkills ? 0 : playerOneSkill + 1;
                    SetBorder(playerOneSkill, 1);*/
                    photonView.RPC("IncreasePlayerTwoSkill", RpcTarget.All);
                }
            }


            
            /*// Change Player Two skill
            if (Input.GetKeyDown(playerTwoLeft))
            {
                /*ClearBorder(playerTwoSkill, 2);
                playerTwoSkill = playerTwoSkill == 0 ? maxSkills : playerTwoSkill - 1;
                SetBorder(playerTwoSkill, 2);#1#
                photonView.RPC("DecreasePlayerTwoSkill", RpcTarget.All);
            } else if (Input.GetKeyDown(playerTwoRight))
            {
                /*ClearBorder(playerTwoSkill, 2);
                playerTwoSkill = playerTwoSkill == maxSkills ? 0 : playerTwoSkill + 1;
                SetBorder(playerTwoSkill, 2);#1#
                photonView.RPC("IncreasePlayerTwoSkill", RpcTarget.All);
            }*/
            

        }

        private void SetBorder(int skill, int playerNo)
        {
            var border = "Border" + playerNo;
            var image = GetBorderByName(border, skillCells[skill]);
            
            // Sets corresponding color based on player number
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

        // Clears the color of existing border
        private void ClearBorder(int skill, int playerNo)
        {
            var border = "Border" + playerNo;
            var image = GetBorderByName(border, skillCells[skill]);
            image.color = Color.clear;
        }

        private Image GetBorderByName(string borderName, GameObject go)
        {
            foreach (var border in go.GetComponentsInChildren<Image>())
            {
                if (border.gameObject.name == borderName)
                {
                    return border;
                }
            }
            return null;
        }
        
        /*
         * Instantiates each skill cell using the skillCellTemplate,
         * replacing values in the skillCellTemplate with info of the
         * skills found in each skillPrefab.
         * Skill Cells are then aligned using a horizontal layout group.
         */
        private void SpawnSkillCell(GameObject go)
        {
            GameObject skillCell = Instantiate(skillCellTemplate, transform);
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
        
        public void PassSkills()
        {
            SkillSelectionManager.instance.assignedPlayerSkills.Add(playerOneSkill);
            SkillSelectionManager.instance.assignedPlayerSkills.Add(playerTwoSkill);
            // resets the total players to 0
            PlayerController.totalPlayers = 0;
        }
        
    }
}