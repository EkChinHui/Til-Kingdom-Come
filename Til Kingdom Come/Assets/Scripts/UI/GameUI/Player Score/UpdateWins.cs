using GamePlay;
using GamePlay.Information;
using TMPro;
using UnityEngine;
using Photon.Pun;
using UI.Map;

namespace UI.GameUI.Player_Score
{
    public class UpdateWins : MonoBehaviour
    {
        public PhotonView photonView;
        public TextMeshProUGUI text;
        public static int wins = 1;
        public static int player1Wins = 0;
        public static int player2Wins = 0;
        public string message = "No. of wins: ";
        
        private void Awake()
        {
            photonView = GetComponent<PhotonView>();
        }

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
            MapLoader.mapToLoad = "Map " + (MapChanger.current + 1);
            ScoreKeeper.winsToGame = wins;
        }

        [PunRPC]
        private void MultiplayerPassWins(int mapNumber, int wins)
        {
            MapLoader.mapToLoad = "Map " + mapNumber;
            ScoreKeeper.winsToGame = wins;
        }
    }
}
