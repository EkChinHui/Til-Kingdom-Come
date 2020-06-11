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
        // Start is called before the first frame update
        void Start()
        {
            gr = GetComponent<GraphicRaycaster>();
        }

        // Update is called once per frame
        void Update()
        {
            pointerEventData.position = Camera.main.WorldToScreenPoint(transform.position);
            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(pointerEventData, results);

            if (results.Count > 0)
            {
                print(results[0].gameObject.name);
            }
        }
    }
}
