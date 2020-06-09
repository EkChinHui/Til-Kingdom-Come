using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CooldownUiController : MonoBehaviour
    {
        public List<CooldownUi> allCooldownUis;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }

    [CreateAssetMenu(fileName = "new Cooldown UI", menuName = "Cooldown UI")]
    public class CooldownUi : ScriptableObject
    {
        public Image image;
        public Image darkMask;

        public void SetFillAmt(float cooldownPercentage)
        {
            darkMask.fillAmount = cooldownPercentage;
        }
    }
}
