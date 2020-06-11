using System;
using System.Collections.Generic;
using DG.Tweening;
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

        public KeyCode select;
        public KeyCode cancel;

        #region Events

        public static Action<int, int> onSkillSelect;

        #endregion

        // Start is called before the first frame update
        void Start()
        {
            gr = GetComponentInParent<GraphicRaycaster>();
            SetColor();
            border = "Border" + playerNo;
        }

        // Update is called once per frame
        void Update()
        {

            pointerEventData.position = Camera.main.WorldToScreenPoint(transform.position);
            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(pointerEventData, results);

            if (results.Count > 0)
            {
                Transform raycastSkill = results[0].gameObject.transform;
                if (Input.GetKeyDown(select))
                {
                    if (currentSkill == null)
                    {
                        currentSkill = raycastSkill;
                        currentSkill.Find(border).GetComponent<Image>().color = color;
                        var skillNo = currentSkill.GetComponent<SkillNumber>().skillNumber;
                        onSkillSelect?.Invoke(playerNo, skillNo);
                        Debug.Log(currentSkill.name);
                    }
                }
            }
            if (currentSkill != null)
            {
                if (Input.GetKeyDown(cancel))
                {
                    currentSkill.Find(border).GetComponent<Image>().color = Color.clear;
                    currentSkill = null;
                }
            }
        }

        private void SelectSkill(Transform t)
        {
            if (t != null)
            {
                t.Find(border).GetComponent<Image>().color = Color.white;
                t.Find(border).GetComponent<Image>().DOColor(color, 0.7f).SetLoops(-1);
            }

            currentSkill = t;
            if (t != null)
            {
                t.Find(border).GetComponent<Image>().color = color;
            }
            else
            {
                //t.Find("Border").GetComponent<Image>().color = Color.clear;
            }

        }

        private void SetColor()
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
