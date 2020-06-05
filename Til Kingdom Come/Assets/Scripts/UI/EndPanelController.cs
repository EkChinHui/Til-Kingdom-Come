﻿using System;
using GamePlay;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace UI
{
    public class EndPanelController : MonoBehaviour
    {
        public GameObject board;
        public RedVictoryScreenController redVictoryScreen;
        public BlueVictoryScreenController blueVictoryScreen;

        private void Start()
        {
            ScoreKeeper.OnGameEnd += EndGame;
        }

        private void EndGame(int player)
        {
            board.SetActive(true);
            switch (player)
            { 
                case 1:
                    redVictoryScreen.gameObject.SetActive(true);
                    break;
                case 2:
                    blueVictoryScreen.gameObject.SetActive(true);
                    break;
            }
        }

        // called with the restart button
        [UsedImplicitly]
        public void ReloadGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void OnDestroy()
        {
            ScoreKeeper.OnGameEnd -= EndGame;
        }
    }
}
