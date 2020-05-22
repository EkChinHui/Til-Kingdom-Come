using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreKeeper : MonoBehaviour
{

    public int winsToGame = 1;
    public TextMeshProUGUI playerOne;
    public TextMeshProUGUI playerTwo;
    public TextMeshProUGUI winningMessage;
    public Canvas canvas;
    public PlayerController player1;
    public PlayerController player2;


    // Start is called before the first frame update
    void Start()
    {
        setWins();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateWins(int player)
    {
        if (player == 1)
        {
            Debug.Log("player 1 wins");
            playerOne.text = "Player 1 Wins: " + player2.losses;
        } else if (player == 2)
        {
            Debug.Log("player 2 wins");
            playerTwo.text = "Player 2 Wins: " + player1.losses;
        }
        TerminateGame();
    }

    void TerminateGame()
    {
        if (player2.losses >= winsToGame)
        {
            // player one wins
            winningMessage.text = "Player one won!!!";
        } else if (player1.losses >= winsToGame)
        {
            // player two wins
            winningMessage.text = "Player two won!!!";
        } else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        // load option to either restart or go back to menu
    }


    public void setWins()
    {
        winsToGame = UpdateWins.wins;
    }

}
