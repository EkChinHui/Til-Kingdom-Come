using System;
using GamePlay;
using UnityEngine;

namespace UI
{
    public class SkillSelector : MonoBehaviour
    {
        private int playerOneSkill;
        private int playerTwoSkill;

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
            ScoreKeeper.playerOneSkill = playerOneSkill;
            ScoreKeeper.playerTwoSkill = playerTwoSkill;
            PlayerController.totalPlayers = 0;
        }
    }
}