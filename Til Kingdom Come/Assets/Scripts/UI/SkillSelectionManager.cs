using System;
using System.Collections.Generic;
using GamePlay.Skills;
using UnityEngine;

namespace UI
{
    public class SkillSelectionManager : MonoBehaviour
    {
        private List<Skill> allSkills = new List<Skill>();
        public static List<int> assignedPlayerSkills; // add in order of player number
        public static int totalPlayers = 2;

        #region Events
        public static Action<int, Skill> passPlayerSkills;
        #endregion

        #region All Skills
        public ForcePull forcePull;
        public ForcePush forcePush;
        public ThrowKnives throwKnives;
        #endregion
        
        public static SkillSelectionManager instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
        }
        private void Start()
        {
            allSkills.Add(forcePull);
            allSkills.Add(forcePush);
            allSkills.Add(throwKnives);
            
            AssignSkills();
        }

        private void AssignSkills()
        {
            for (var i = 0; i < totalPlayers; i++)
            {
                passPlayerSkills(i + 1, allSkills[assignedPlayerSkills[i]]);
            }
        }
    }
    
    
}
    