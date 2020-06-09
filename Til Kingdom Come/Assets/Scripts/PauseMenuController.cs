using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PauseMenuController : MonoBehaviour
{
    public static Action PauseToggle;
    private bool gameIsPaused = false;
    public GameObject pauseMenu;
    public GameObject blurEffect;
    void Update()
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

    public void Resume()
    {
        pauseMenu.SetActive(false);
        blurEffect.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }
    private void Pause()
    {
        pauseMenu.SetActive(true);
        blurEffect.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void UnfreezeTime()
    {
        Time.timeScale = 1f;
    }
}
