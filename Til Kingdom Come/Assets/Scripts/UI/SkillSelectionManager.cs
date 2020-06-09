using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay.Skills;
using UnityEngine;

namespace UI
{
    public class SkillSelectionManager : MonoBehaviour
    {
        private List<GameObject> Skills = new List<GameObject>();
        public List<int> assignedPlayerSkills; // add in order of player number
        public int totalPlayers = 2;

        #region Events
        public static Action<int, GameObject> passPlayerSkills;
        #endregion

        #region All Skills
        public GameObject forcePull;
        public GameObject forcePush;
        public GameObject throwKnives;
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
            Skills.Add(forcePull);
            Skills.Add(forcePush);
            Skills.Add(throwKnives);
        }

        public void AssignSkills()
        {
            for (var i = 0; i < totalPlayers; i++)
            {
                //Debug.Log(allSkills[assignedPlayerSkills[i] - 1].SkillName);
                passPlayerSkills(i + 1, Skills[assignedPlayerSkills[i] - 1]);
            }
        }
    }
    
    
}
    