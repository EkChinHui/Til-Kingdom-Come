using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapOneController : MonoBehaviour
{
    void Start()
    {
        AudioManager.instance.FadeOutCurrentMusic();
        AudioManager.instance.PlayMusic("Battle Theme 1");
    }
}
