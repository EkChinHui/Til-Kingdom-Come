using System;
using System.Collections.Generic;
using UnityEngine;

namespace GamePlay.Information
{
    public class SkillSelectionManager : MonoBehaviour
    {
        public List<GameObject> skillPrefabs = new List<GameObject>();
        public List<int> assignedPlayerSkills; // add in order of player number
        public int playerOneAISkill;
        public int playerTwoAISkill;
        public bool testingAI;

        #region Events
        public static Action<int, GameObject> passPlayerSkills;
        #endregion

        #region All Skills
        public GameObject throwKnives;
        public GameObject confusion;
        public GameObject heal;
        public GameObject lunge;
        public GameObject fireball;
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
            
            /*skillPrefabs.Add(throwKnives); // 1
            skillPrefabs.Add(confusion); // 2
            skillPrefabs.Add(heal); // 3
            skillPrefabs.Add(lunge); // 4
            skillPrefabs.Add(fireball); // 5*/
            if (testingAI)
            {
                assignedPlayerSkills.Add(playerOneAISkill);
                assignedPlayerSkills.Add(playerTwoAISkill);
            }
        }
        private void Start()
        {

        }

        public void AssignSkills(int playerNo)
        {
            Debug.Log("Assigning skills to player " + playerNo);
            passPlayerSkills(playerNo, skillPrefabs[assignedPlayerSkills[playerNo - 1]]);
        }
    }
    
    
}
    