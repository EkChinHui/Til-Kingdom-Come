using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreKeeper : MonoBehaviour
{

    public TextMeshProUGUI playerOne;
    public TextMeshProUGUI playerTwo;
    public TextMeshProUGUI winningMessage;
    [SerializeField] public static int winsToGame;
    public int Onewins = 0;
    public int TwoWins = 0;

    public static ScoreKeeper scoreKeeper;


    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        // singleton
        if (!scoreKeeper)
        {
            scoreKeeper = this;
        } else
        {
            Destroy(gameObject);
        }
    }


    public void updateWins(int player)
    {
        if (player == 1)
        {
            ScoreKeeper.scoreKeeper.Onewins++;
            ScoreKeeper.scoreKeeper.playerOne.text = "Player 1 Wins: " + ScoreKeeper.scoreKeeper.Onewins;
        } else if (player == 2)
        {
            ScoreKeeper.scoreKeeper.TwoWins++;
            ScoreKeeper.scoreKeeper.playerTwo.text = "Player 2 Wins: " + ScoreKeeper.scoreKeeper.TwoWins;
        }
        // delay after game ends to let death animation play out
        Invoke("TerminateGame", 3);
    }

    void TerminateGame()
    {
        if (ScoreKeeper.scoreKeeper.Onewins >= winsToGame)
        {
            // player one wins
            ScoreKeeper.scoreKeeper.winningMessage.text = "Player one won!!!";
        } else if (ScoreKeeper.scoreKeeper.TwoWins >= winsToGame)
        {
            // player two wins
            ScoreKeeper.scoreKeeper.winningMessage.text = "Player two won!!!";
        } else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        // load option to either restart or go back to menu
    }




}
