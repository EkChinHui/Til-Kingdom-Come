using UnityEngine;

namespace UI
{
    public class BlueVictoryScreenController : MonoBehaviour
    {
        private RectTransform rectTransform;
        private float speed = 500f;

        private float endYAxis = 50f;
        // Start is called before the first frame update
        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        private void Update() {
            Lower();
        }
        public void Lower() {
            if(rectTransform.anchoredPosition.y > endYAxis) {
                rectTransform.anchoredPosition -= new Vector2(0, Time.deltaTime * speed);
            }
        }
    }
}
