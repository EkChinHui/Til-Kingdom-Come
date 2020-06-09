using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CooldownUi : MonoBehaviour
    {
        public Image image;
        public Image darkMask;

        public void Start()
        {
            //darkMask.enabled = false;
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