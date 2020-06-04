using System;
using TMPro;
using UI;
using UnityEngine;

namespace GamePlay
{
    public class ScoreKeeper : MonoBehaviour
    {
        public TextMeshProUGUI playerOne;
        public TextMeshProUGUI playerTwo;
        private int playerOneScore;
        private int playerTwoScore;
        [SerializeField] public static int winsToGame = 1;

        public delegate void ResetDelegate();
        public static event ResetDelegate ResetPlayersEvent;

        public delegate void GameEndDelegate(int player);
        public static event GameEndDelegate OnGameEnd;


        public void Start()
        {
            PlayerController.DeathEvent += UpdateWins;
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
                OnGameEnd?.Invoke(1);
            } else if (playerTwoScore >= winsToGame)
            {
                // player two wins
                OnGameEnd?.Invoke(2);
            } else
            {
                if (ResetPlayersEvent != null)
                {
                    ResetPlayersEvent();
                }
            }
            // load option to either restart or go back to menu
        }

        private void OnDestroy()
        {
            PlayerController.DeathEvent -= UpdateWins;
        }
    }
}
