using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTwoMusicController : MonoBehaviour
{
    void Start()
    {
        AudioManager.instance.PlayMusic("Battle Theme 2");
    }
}
