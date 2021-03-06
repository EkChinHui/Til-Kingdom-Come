﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameUI.Cooldown
{
    public class CooldownUI : MonoBehaviour
    {
        public Image image;
        public Image darkMask;

        public void Start()
        {
            darkMask.fillAmount = 0;
        }

        public IEnumerator ChangesFillAmount(float duration)
        {
            float timer = 0;
 
            while (timer < duration)
            {
                timer += Time.deltaTime;
                darkMask.fillAmount = Mathf.Lerp(1, 0, timer / duration);
                yield return null;
            }
        }
        
    }
}