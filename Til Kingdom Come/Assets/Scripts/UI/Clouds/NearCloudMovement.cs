using UnityEngine;

namespace UI
{
    public class NearCloudMovement : MonoBehaviour
    {
        private RectTransform rectTransform;
        private float speed = -40f;

        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            //update the position
            if (rectTransform.anchoredPosition.x <= -1920) {
                rectTransform.anchoredPosition = new Vector2(1920, rectTransform.anchoredPosition.y);
            }

            rectTransform.anchoredPosition += new Vector2(Time.deltaTime * speed, 0);
        }
    }
}
