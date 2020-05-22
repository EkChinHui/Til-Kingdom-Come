using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapChanger : MonoBehaviour
{
    [Header("Sprite to change")]
    public SpriteRenderer spriteRenderer;

    [Header("Sprites to cycle through")]
    public List<Sprite> sprites = new List<Sprite>();
    public int current = 0;

    public void NextOption()
    {
        current++;
        if (current >= sprites.Count)
        {
            current = 0;
        }
        spriteRenderer.sprite = sprites[current];
    }

    public void PreviousOption()
    {
        current--;
        if(current < 0)
        {
            current = sprites.Count - 1;
        }
        spriteRenderer.sprite = sprites[current];
    }


    
}
