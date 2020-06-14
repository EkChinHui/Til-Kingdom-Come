using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Skill_selection
{
    public class CursorDetection : MonoBehaviour
    {
        private GraphicRaycaster gr;
        private PointerEventData pointerEventData = new PointerEventData(null);
        private Transform currentSkill;
        public int playerNo;
        public Color color;
        public string border = "Border1";

        #region Events
        public static Action<int, int> onSkillSelect;
        #endregion
        
        void Start()
        {
            gr = GetComponentInParent<GraphicRaycaster>();
            SetColor();
            border = "Border" + playerNo;
        }
        
        void Update()
        {
            // Pointer returns a list of gameobjects the cursor is over
            pointerEventData.position = Camera.main.WorldToScreenPoint(transform.position);
            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(pointerEventData, results);

            /*
             * If items are found, results can only contain SkillCells. All other
             * graphics objects have their "raycast target" set to false
             */
            if (results.Count > 0)
            {
                Transform raycastSkill = results[0].gameObject.transform;
                if (currentSkill == null)
                {
                    currentSkill = raycastSkill;
                    currentSkill.Find(border).GetComponent<Image>().color = color;
                    var skillNo = currentSkill.GetComponent<SkillNumber>().skillNumber;
                    onSkillSelect?.Invoke(playerNo, skillNo);
                } 
                else if (currentSkill != raycastSkill)
                {
                    currentSkill.Find(border).GetComponent<Image>().color = Color.clear;
                    currentSkill = raycastSkill;
                }
                else if (currentSkill == raycastSkill)
                {
                    currentSkill.Find(border).GetComponent<Image>().color = color;
                    var skillNo = currentSkill.GetComponent<SkillNumber>().skillNumber;
                    onSkillSelect?.Invoke(playerNo, skillNo);
                }
                
            }
        }
        
        private void SetColor()
        // sets color based on player number , playerNo is set in Inspector
        {
            switch (playerNo)
            {
                case 1:
                    color = Color.red;
                    break;
                case 2:
                    color = Color.blue;
                    break;
            }
        }
        
    }
}
