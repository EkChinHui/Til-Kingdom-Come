﻿using System;
using System.Collections.Generic;

using UnityEngine;

namespace UI
{
    public class SkillSelectionManager : MonoBehaviour
    {
        private List<GameObject> skillPrefabs = new List<GameObject>();
        public List<int> assignedPlayerSkills; // add in order of player number

        public int totalPlayers = 2;

        #region Events
        public static Action<int, GameObject> passPlayerSkills;
        #endregion

        #region All Skills
        public GameObject forcePush;
        public GameObject forcePull;
        public GameObject throwKnives;
        #endregion
        
        public static SkillSelectionManager instance;

        private void Awake()
        {
            // singleton
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
            skillPrefabs.Add(forcePush); // 0
            skillPrefabs.Add(forcePull); // 1
            skillPrefabs.Add(throwKnives); // 2
        }

        public void AssignSkills()
        {
            Debug.Log("Assigning skills to players");
            for (var i = 0; i < totalPlayers; i++)
            {
                //               player no, assigned skill
                passPlayerSkills(i + 1, skillPrefabs[assignedPlayerSkills[i]]);
            }
        }
    }
    
    
}
    