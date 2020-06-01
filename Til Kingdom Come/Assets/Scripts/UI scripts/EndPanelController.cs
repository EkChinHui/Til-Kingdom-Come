using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPanelController : MonoBehaviour
{
    public string mainMenu = "Main Menu";
    public EndPanelController endPanel;
    public RedVictoryScreenController redVictoryScreen;
    public BlueVictoryScreenController blueVictoryScreen;
    public void endGame(int player)
    {
        endPanel.gameObject.SetActive(true);
        if (player == 1) {
            redVictoryScreen.gameObject.SetActive(true);
        } else if (player == 2) {
            blueVictoryScreen.gameObject.SetActive(true);
        }
    }


    public void ReloadGame()
    {
        //endPanel.gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
