﻿using GamePlay;
using UnityEngine;

namespace UI
{
    public class SkillSelector : MonoBehaviour
    {
        private int playerOneSkill = 1;
        private int playerTwoSkill = 1;

        private void Start()
        {
            SkillSelectionManager.instance.assignedPlayerSkills.Clear();
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
            PlayerController.totalPlayers = 0;
        }
        
    }
}