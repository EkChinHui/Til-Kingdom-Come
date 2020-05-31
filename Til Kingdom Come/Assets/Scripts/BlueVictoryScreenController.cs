using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        lower();
    }
    public void lower() {
        if(rectTransform.anchoredPosition.y > endPoint) {
            rectTransform.anchoredPosition -= new Vector2(0, Time.deltaTime * speed);
        }
    }
}
