using TMPro;
using UnityEngine;

namespace UI.GameUI.Cooldown
{
    public class DisplayCharges : MonoBehaviour
    {
        public int charges;
        public TextMeshProUGUI display;

        private void Start()
        {
            if (charges == 0) return;
            display.text = charges.ToString();
        }

        public void UpdateCharges(int charge)
        {
            display.text = charge.ToString();
        }
    }
}