using UnityEngine;

namespace UI
{
    public class BlueVictoryScreenController : MonoBehaviour
    {
        RectTransform rectTransform;
        float speed = 500f;
        float endPoint = 50f;
        // Start is called before the first frame update
        void Start()
        {
            rectTransform = GetComponent<RectTransform>();
        }
    
        void Update() {
            Lower();
        }
        public void Lower() {
            if(rectTransform.anchoredPosition.y > endPoint) {
                rectTransform.anchoredPosition -= new Vector2(0, Time.deltaTime * speed);
            }
        }
    }
}
