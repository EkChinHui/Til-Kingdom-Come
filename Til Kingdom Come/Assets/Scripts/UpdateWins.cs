using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateWins : MonoBehaviour
{
    public TextMeshProUGUI text;
    public static int wins = 1;
    public string message = "No. of Wins: ";

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
}
