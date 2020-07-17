using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject playMenu;

    public void Play()
    {
        playMenu.SetActive(true);
        mainMenu.SetActive(false);
    }
    
    public void Main()
    {
        mainMenu.SetActive(true);
        playMenu.SetActive(false);
    }
}