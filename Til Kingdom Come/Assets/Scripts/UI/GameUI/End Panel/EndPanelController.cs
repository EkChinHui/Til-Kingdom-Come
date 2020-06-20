using GamePlay;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace UI
{
    public class EndPanelController : MonoBehaviour
    {
        public GameObject board;
        public RedVictoryScreenController redVictoryScreen;
        public BlueVictoryScreenController blueVictoryScreen;

        private void Start()
        {
            ScoreKeeper.onGameEnd += EndGame;
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
