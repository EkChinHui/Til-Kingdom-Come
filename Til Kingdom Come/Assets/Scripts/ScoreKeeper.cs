using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void updateWins(int player)
    {
        if (player == 1)
        {
            player1.Score++;
            playerOne.text = "Player 1 wins: " + player1.Score;
        } else if (player == 2)
        {
            player2.Score++;
            playerTwo.text = "Player 2 wins: " + player2.Score;
        }
        // delay after game ends to let death animation play out
        Invoke("TerminateGame", 3);
    }

    void TerminateGame()
    {
        if (player1.Score >= winsToGame)
        {
            // player one wins
            endPanel.endGame(1);
        } else if (player2.Score >= winsToGame)
        {
            // player two wins
            endPanel.endGame(2);
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
