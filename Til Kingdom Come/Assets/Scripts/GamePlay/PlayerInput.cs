﻿using System;
using UnityEngine;

namespace GamePlay
{
    public class PlayerInput : MonoBehaviour
    {
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

        private void Update()
        {
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
    }
}