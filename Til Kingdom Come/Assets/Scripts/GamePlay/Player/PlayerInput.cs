using System;
using UnityEngine;

namespace GamePlay.Player
{
    public class PlayerInput : MonoBehaviour
    {
        public static Action onEnableInput;
        public static Action onDisableInput;
        [SerializeField] public bool inputIsEnabled = true;
        [Header("Input")]
        public PlayerKeyInput playerKeyInput;
        // note to use getAxis for multi-player mode so user can change their input
        private KeyCode leftKey;
        private KeyCode rightKey;
        private KeyCode rollKey;
        private KeyCode attackKey;
        private KeyCode blockKey;
        private KeyCode skillKey;
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
            onEnableInput += EnableInput;
            onDisableInput += DisableInput;
            leftKey = playerKeyInput.GetLeftKey();
            rightKey = playerKeyInput.GetRightKey();
            rollKey = playerKeyInput.GetRollKey();
            attackKey = playerKeyInput.GetAttackKey();
            blockKey = playerKeyInput.GetBlockKey();
            skillKey = playerKeyInput.GetSkillKey();
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

        public void EnableInput()
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
            inputIsEnabled = true;
        }
        public void DisableInput()
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
            inputIsEnabled = true;
        }

        public void InvertKeys()
        {
            var tempLeftKey = leftKey;
            leftKey = rightKey;
            rightKey = tempLeftKey;
        }

        private void OnDestroy()
        {
            onEnableInput -= EnableInput;
            onDisableInput -= DisableInput;
        }
        
    }
}