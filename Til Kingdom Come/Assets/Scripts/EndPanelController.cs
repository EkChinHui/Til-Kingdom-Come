using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPanelController : MonoBehaviour
{
    public string mainMenu = "Menu";
    public EndPanelController endPanel;


    public void endGame()
    {
        endPanel.gameObject.SetActive(true);
    }

    public void backToMainMenu()
    {
        //endPanel.gameObject.SetActive(false);
        SceneManager.LoadScene(mainMenu);
    }


    public void ReloadGame()
    {
        //endPanel.gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
