using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine;

public class RoundStartPanelController : MonoBehaviour
{
    public TextMeshProUGUI roundNumberText;
    private float roundNumber = 1;
    private RectTransform rectTransform;
    private float speed = 500f;
    private float startYAxis;
    private float endYAxis = 0;
    private bool lowering = false;
    private bool raising = false;
    private bool playSound = false;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startYAxis = rectTransform.anchoredPosition.y;
        lowering = true;
        playSound = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (lowering) {
            if (playSound) {
                AudioManager.instance.Play("Round Start");
                playSound = false;
            }
            Lower();
        } else if (raising) {
            StartCoroutine(Raise());
        }
    }

    public void Lower() {
        if(rectTransform.anchoredPosition.y > endYAxis) {
            rectTransform.anchoredPosition -= new Vector2(0, Time.deltaTime * speed);
        } else {
            lowering = false;
            raising = true;
        }
    }

    public IEnumerator Raise() {
        yield return new WaitForSeconds(1);
        if(rectTransform.anchoredPosition.y < startYAxis) {
            rectTransform.anchoredPosition += new Vector2(0, Time.deltaTime * speed);
        } else {
            raising = false;
        }
    }

    private void UpdateRoundNumber() {
        roundNumber++;
        roundNumberText.text = "Round " + roundNumber;
    }

    public void nextRound() {
        UpdateRoundNumber();
        lowering = true;
        playSound = true;
    }
}
