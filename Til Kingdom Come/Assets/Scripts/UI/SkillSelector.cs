using System;
using GamePlay;
using GamePlay.Skills;
using UnityEngine;

namespace UI
{
    public class SkillSelector : MonoBehaviour
    {
        private int playerOneSkill;
        private int playerTwoSkill;

        private void Start()
        {
            SkillSelectionManager.assignedPlayerSkills.Clear();
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
            SkillSelectionManager.assignedPlayerSkills.Add(playerOneSkill);
            SkillSelectionManager.assignedPlayerSkills.Add(playerTwoSkill);
            Debug.Log(SkillSelectionManager.assignedPlayerSkills);
            PlayerController.totalPlayers = 0;
        }
        
    }
}