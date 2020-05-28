using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapChanger : MonoBehaviour
{
    [Header("Sprite to change")]
    public Image image;

    [Header("Sprites to cycle through")]
    public List<Sprite> sprites = new List<Sprite>();
    public static int current = 0;

    public void NextOption()
    {
        current++;
        if (current >= sprites.Count)
        {
            current = 0;
        }
        image.sprite = sprites[current];
    }

    public void PreviousOption()
    {
        current--;
        if(current < 0)
        {
            current = sprites.Count - 1;
        }
        image.sprite = sprites[current];
    }


    
}
