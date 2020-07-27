using GamePlay.Player;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.GameUI.Pause
{
    public class PauseMenuController : MonoBehaviour
    {
        public bool canPause = true;
        public PhotonView photonView;
        [SerializeField] bool gameIsPaused = false;
        public GameObject pauseMenu;
        public GameObject blurEffect;
        void Update()
        {
            if (canPause)
            {
                if(Input.GetKeyDown(KeyCode.Escape))
                {
                    if (SceneManager.GetActiveScene().name == "MultiplayerArena")
                    {
                        if (canPause)
                            photonView.RPC("OnPauseButtonClicked", RpcTarget.All);
                        Debug.Log("RPC sent");
                    }
                    else OnPauseButtonClicked();
                }
            }
        }

        [PunRPC]
        public void OnPauseButtonClicked()
        {
            if(gameIsPaused)
            {
                if (PlayerInput.onEnableInput != null) PlayerInput.onEnableInput();
                Debug.Log("Player enable input after pausing");
                Resume();
            }
            else
            {
                if (PlayerInput.onDisableInput != null) PlayerInput.onDisableInput();
                Pause();
            }
        }
        
        [PunRPC]
        public void Resume()
        {
            Debug.Log("Resuming Game");
            AudioManager.instance.PlayCurrentMusic();
            pauseMenu.SetActive(false);
            blurEffect.SetActive(false);
            Time.timeScale = 1f;
            gameIsPaused = false;
        }

        public void MultiplayerResume()
        {
            photonView.RPC("Resume", RpcTarget.All);
        }
        
        private void Pause()
        {
            Debug.Log("Pausing Game");
            AudioManager.instance.PauseCurrentMusic();
            pauseMenu.SetActive(true);
            blurEffect.SetActive(true);
            Time.timeScale = 0f;
            gameIsPaused = true;
        }

        public void Home()
        {
            Debug.Log("Back to Main Menu");
            AudioManager.instance.StopCurrentMusic();
            AudioManager.instance.PlayMusic("Main Theme");
            Time.timeScale = 1f;
        }

        public void Restart()
        {
            AudioManager.instance.StopCurrentMusic();
            Time.timeScale = 1f;
        }
    }
}
