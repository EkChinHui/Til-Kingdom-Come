using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public RectTransform mainMenu;
    public RectTransform playMenu;
    private bool transiting = false;
    private float speed = 50f;
    private enum Menu { Main, Play }
    private Menu currentMenu;
    private float centre;
    private void Start()
    {
        currentMenu = Menu.Main;
        centre = mainMenu.anchoredPosition.x;
        Debug.Log(centre);
    }
    void Update()
    {
        if (transiting)
        {
            Transit();
        }
    }
    private void Transit()
    {
        if (currentMenu == Menu.Main)
        {
            if (playMenu.anchoredPosition.x > centre)
            {
                var displacement = new Vector2(Time.deltaTime * speed, 0);
                mainMenu.anchoredPosition -= displacement;
                playMenu.anchoredPosition -= displacement;
            }
            else
            {
                Debug.Log(playMenu.anchoredPosition.x);
                transiting = false;
                currentMenu = Menu.Play;
            }
        }
        else if (currentMenu == Menu.Play)
        {
            if (mainMenu.anchoredPosition.x < centre)
            {
                var displacement = new Vector2(Time.deltaTime * speed, 0);
                playMenu.anchoredPosition += displacement;
                mainMenu.anchoredPosition += displacement;

            }
            else
            {
                transiting = false;
                currentMenu = Menu.Main;
            }
        }
    }

    public void StartTransition()
    {
        transiting = true;
    }
}
