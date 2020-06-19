using UnityEngine;
using System;

namespace GamePlay
{
    public class PlayerInput : MonoBehaviour
    {
        public static Action OnToggleInput;
        private bool inputIsEnabled = true;
        [Header("Input")]
        // note to use getAxis for multi-player mode so user can change their input
        public KeyCode leftKey;
        public KeyCode rightKey;
        public KeyCode rollKey;
        public KeyCode blockKey;
        public KeyCode attackKey;
        public KeyCode skillKey;
        private bool attemptLeft;
        private bool attemptRight;
        private bool attemptRoll;
        private bool attemptAttack;
        private bool attemptBlock;
        private bool attemptSkill;

        public bool AttemptLeft => attemptLeft;
        public bool AttemptRight => attemptRight;
        public bool AttemptRoll => attemptRoll;
        public bool AttemptAttack => attemptAttack;
        public bool AttemptBlock => attemptBlock;
        public bool AttemptSkill => attemptSkill;

        private void Start()
        {
            OnToggleInput += Toggle;
        }
        
        private void Update()
        {
            if (!inputIsEnabled) return;
            InputManager();
        }
        
        private void InputManager()
        {
            attemptLeft = Input.GetKey(leftKey);
            attemptRight = Input.GetKey(rightKey);
            attemptRoll = Input.GetKeyDown(rollKey);
            attemptAttack = Input.GetKeyDown(attackKey);
            attemptBlock = Input.GetKeyDown(blockKey);
            attemptSkill = Input.GetKeyDown(skillKey);
        }

        public void Toggle()
        {
            if (inputIsEnabled)
            {
                attemptLeft = false;
                attemptRight = false;
                attemptRoll = false;
                attemptAttack = false;
                attemptBlock = false;
                attemptSkill = false;
            }
            inputIsEnabled = !inputIsEnabled;
            Debug.Log("Toggle Input");
        }

        public void SwitchKeys()
        {
            var tempLeftKey = leftKey;
            leftKey = rightKey;
            rightKey = tempLeftKey;
        }

        private void OnDestroy()
        {
            OnToggleInput -= Toggle;
        }
        
    }
}