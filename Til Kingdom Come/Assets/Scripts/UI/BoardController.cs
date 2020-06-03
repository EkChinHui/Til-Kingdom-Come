using UnityEngine;

namespace UI
{
    public class BoardController : MonoBehaviour
    {
        // Start is called before the first frame update
        private RectTransform rectTransform;
        private const float Speed = 500f;
        private const float EndPoint = -50f;

        // Start is called before the first frame update
        void Start()
        {
            rectTransform = GetComponent<RectTransform>();
        }
    
        void Update() {
            Lower();
        }
        public void Lower() {
            if(rectTransform.anchoredPosition.y > EndPoint) {
                rectTransform.anchoredPosition -= new Vector2(0, Time.deltaTime * Speed);
            }
        }
    }
}
