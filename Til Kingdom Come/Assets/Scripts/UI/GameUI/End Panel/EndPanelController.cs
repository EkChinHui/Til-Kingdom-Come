using GamePlay.Information;
using GamePlay.Player;
using JetBrains.Annotations;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.GameUI.End_Panel
{
    public class EndPanelController : MonoBehaviour
    {
        public GameObject board;
        public RedVictoryScreenController redVictoryScreen;
        public BlueVictoryScreenController blueVictoryScreen;
        public PhotonView photonView;

        private void Start()
        {
            ScoreKeeper.onGameEnd += EndGame;
            photonView = GetComponent<PhotonView>();
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void EndGame(int player)
        {
            AudioManager.instance.FadeOutCurrentMusic();
            AudioManager.instance.PlaySoundEffect("Victory");
            board.SetActive(true);
            switch (player)
            { 
                case 1:
                    redVictoryScreen.gameObject.SetActive(true);
                    break;
                case 2:
                    blueVictoryScreen.gameObject.SetActive(true);
                    break;
            }
        }

        // called with the restart button
        [UsedImplicitly]
        public void ReloadGame()
        {
            Debug.Log("Reloading Game");
            PlayerController.totalPlayers = 0;
            if (SceneManager.GetActiveScene().name == "Game")
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            else
                photonView.RPC("RPCReloadGame", RpcTarget.All);
        }

        public void RPCQuit()
        {
            photonView.RPC("RPCHome", RpcTarget.All);
            photonView.RPC("RPCMusic", RpcTarget.All);
        }

        [PunRPC]
        public void RPCHome()
        {
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene("Multiplayer Lobby");
        }


        [PunRPC]
        public void RPCReloadGame()
        {
            PhotonNetwork.LoadLevel("MultiplayerArena");
        }

        [PunRPC]
        public void RPCMusic()
        {
            AudioManager.instance.FadeOutCurrentMusic();
            AudioManager.instance.PlayMusic("Main Theme");
        }

        public void Home()
        {
            Debug.Log("Back to Main Menu");
            AudioManager.instance.PlayMusic("Main Theme");
        }

        private void OnDestroy()
        {
            ScoreKeeper.onGameEnd -= EndGame;
        }
    }
}
