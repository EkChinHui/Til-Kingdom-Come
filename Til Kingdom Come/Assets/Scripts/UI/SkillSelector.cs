using GamePlay;
using UnityEngine;

namespace UI
{
    public class SkillSelector : MonoBehaviour
    {
        private int playerOneSkill = 0;
        private int playerTwoSkill = 0;

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
            // resets the total players to 0
            PlayerController.totalPlayers = 0;
        }
        
    }
}