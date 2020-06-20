using System;
using GamePlay.Player;
using TMPro;
using UI.GameUI;
using UnityEngine;

namespace GamePlay
{
    public class ScoreKeeper : MonoBehaviour
    {
        public TextMeshProUGUI playerOne;
        public TextMeshProUGUI playerTwo;
        public PauseMenuController pauseMenuController;
        private int playerOneScore;
        private int playerTwoScore;
        public static int winsToGame = 1;
        public RoundStartPanelController roundStartPanel;

        #region Events
        public static Action resetPlayersEvent;
        public static Action<int> onGameEnd;
        #endregion

        public void Start()
        {
            PlayerController.onDeath += UpdateWins;
        }

        private void UpdateWins(int player)
        {
            switch (player)
            {
                case 1:
                    playerTwoScore++;
                    playerTwo.text = "Player 2 wins: " + playerTwoScore;
                    break;
                case 2:
                    playerOneScore++;
                    playerOne.text = "Player 1 wins: " + playerOneScore;
                    break;
            }

            // delay after game ends to let death animation play out
            Invoke(nameof(TerminateGame), 3);
        }

        void TerminateGame()
        {
            if (playerOneScore >= winsToGame)
            {
                // player one wins
                onGameEnd?.Invoke(1);
            } else if (playerTwoScore >= winsToGame)
            {
                // player two wins
                onGameEnd?.Invoke(2);
            } else
            {
                if (resetPlayersEvent != null)
                {
                    roundStartPanel.nextRound();
                    resetPlayersEvent();
                }
            }
            pauseMenuController.canPause = false;
            // load option to either restart or go back to menu
        }

        private void OnDestroy()
        {
            PlayerController.onDeath -= UpdateWins;
        }
    }
}
