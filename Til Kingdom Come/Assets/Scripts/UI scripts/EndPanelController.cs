using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPanelController : MonoBehaviour
{
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

    // called with the restart button
    public void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
