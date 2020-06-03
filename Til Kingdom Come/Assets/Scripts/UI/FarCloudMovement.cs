﻿using UnityEngine;

namespace UI
{
    public class FarCloudMovement : MonoBehaviour
    {
        RectTransform rectTransform;
        float speed = -25f;
        void Start()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        void Update()
        {
            //update the position
            if (rectTransform.anchoredPosition.x <= -1920) {
                rectTransform.anchoredPosition = new Vector2(1920, rectTransform.anchoredPosition.y);
            }

            rectTransform.anchoredPosition += new Vector2(Time.deltaTime * speed, 0);
        }
    }
}
