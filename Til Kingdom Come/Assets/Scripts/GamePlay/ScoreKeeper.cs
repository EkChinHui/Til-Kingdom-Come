using TMPro;
using UI;
using UnityEngine;

namespace GamePlay
{
    public class ScoreKeeper : MonoBehaviour
    {
        public TextMeshProUGUI playerOne;
        public TextMeshProUGUI playerTwo;
        public PlayerController player1;
        public PlayerController player2;
        public EndPanelController endPanel;
        [SerializeField] public static int winsToGame = 1;


        public void Start()
        {
        }

        public void UpdateWins(int player)
        {
            if (player == 1)
            {
                player2.score++;
                playerTwo.text = "Player 2 wins: " + player2.score;
            } else if (player == 2)
            {
                player1.score++;
                playerOne.text = "Player 1 wins: " + player1.score;
            }
            // delay after game ends to let death animation play out
            Invoke(nameof(TerminateGame), 3);
        }

        void TerminateGame()
        {
            if (player1.score >= winsToGame)
            {
                // player one wins
                endPanel.EndGame(1);
            } else if (player2.score >= winsToGame)
            {
                // player two wins
                endPanel.EndGame(2);
            } else
            {
                ResetLevel();
            }
            // load option to either restart or go back to menu
        }

        void ResetLevel()
        {
            player2.ResetPlayer();
            player1.ResetPlayer();
        }


    }
}
