using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UpdateWins : MonoBehaviour
{
    public TextMeshProUGUI text;
    public int wins = 1;
    public static int player1Wins = 0;
    public static int player2Wins = 0;
    public string message = "No. of wins: ";

    private void Start()
    {
    }
    public void addWins()
    {
        wins++;
        text.text = message + wins;
    }

    public void minusWins()
    {
        wins--;
        if (wins <= 1)
        {
            wins = 1;
        }
        text.text = message + wins;
    }

    public void passWins()
    {
        ScoreKeeper.winsToGame = wins;
    }
}
