using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearCloudMovement1 : MonoBehaviour
{
    RectTransform rectTransform;
    float speed = -100f;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        //update the position
        if (rectTransform.anchoredPosition.x <= -1920) {
            rectTransform.anchoredPosition = new Vector2(1920, 0);
        }

        rectTransform.anchoredPosition += new Vector2(Time.deltaTime * speed, 0);
    }
}
