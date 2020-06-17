using UnityEngine;
using System;

public class PauseMenuController : MonoBehaviour
{
    public static Action PauseToggle;
    public bool canPause = true;
    private bool gameIsPaused = false;
    public GameObject pauseMenu;
    public GameObject blurEffect;
    void Update()
    {
        if (canPause)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                if(gameIsPaused)
                {
                    if (PauseToggle != null) PauseToggle();
                    Resume();
                }
                else
                {
                    if (PauseToggle != null) PauseToggle();
                    Pause();
                }
            }
        }
    }

    public void Resume()
    {
        Debug.Log("Resuming Game");
        AudioManager.instance.PlayCurrentMusic();
        pauseMenu.SetActive(false);
        blurEffect.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
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
