﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueVictoryScreenController : MonoBehaviour
{
    RectTransform rectTransform;
    float speed = 500f;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        print(rectTransform.anchoredPosition);
    }
    
    void Update() {
        lower();
    }
    public void lower() {
        if(rectTransform.anchoredPosition.y > 0) {
            rectTransform.anchoredPosition -= new Vector2(0, Time.deltaTime * speed);
        }
    }
}
