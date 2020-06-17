using UnityEngine;

namespace GamePlay.Skills
{
    public class Charges : MonoBehaviour
    {
        private float chargeTime;
        private float nextChargeTime;
        private int currentCharge;
        private int maxCharge;

        public int CurrentCharge
        {
            get => currentCharge;
            set => currentCharge = value;
        }

        public float ChargeTime
        {
            get => chargeTime;
            set => chargeTime = value;
        }

        void Update()
        {
            Charging();
        }

        public void SetCharges(int charges, float chargeTime)
        {
            maxCharge = charges;
            currentCharge = maxCharge;
            this.chargeTime = chargeTime;
        }
        
        private void Charging()
        {
            if (currentCharge >= maxCharge)
            {
                nextChargeTime = Time.time + chargeTime;
            }

            if (Time.time >= nextChargeTime)
            {
                currentCharge++;
                nextChargeTime = Time.time + chargeTime;
            }
        }
    }
}
