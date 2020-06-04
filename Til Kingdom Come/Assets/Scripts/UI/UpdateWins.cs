using GamePlay;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UpdateWins : MonoBehaviour
    {
        public TextMeshProUGUI text;
        public int wins = 1;
        public static int player1Wins = 0;
        public static int player2Wins = 0;
        public string message = "No. of wins: ";
        
        public void AddWins()
        {
            wins++;
            text.text = message + wins;
        }

        public void MinusWins()
        {
            wins--;
            if (wins <= 1)
            {
                wins = 1;
            }
            text.text = message + wins;
        }

        public void PassWins()
        {
            MapLoader.mapToLoad = "Map" + (MapChanger.current + 1);
            ScoreKeeper.winsToGame = wins;
        }
    }
}
