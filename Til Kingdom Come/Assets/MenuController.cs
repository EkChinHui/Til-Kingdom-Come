using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject playMenu;
    private Vector3 activeScale;
    private Vector3 inactiveScale;
    private void Start()
    {
        activeScale = mainMenu.transform.localScale;
        inactiveScale = new Vector3(0,0,0);
    }
    public void ToPlayMenu()
    {
        playMenu.transform.localScale = activeScale;
        mainMenu.transform.localScale = inactiveScale;
    }
    
    public void ToMainMenu()
    {
        mainMenu.transform.localScale = activeScale;
        playMenu.transform.localScale = inactiveScale;
    }
}