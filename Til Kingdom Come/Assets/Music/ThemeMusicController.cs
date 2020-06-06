using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeMusicController : MonoBehaviour
{
    void Start()
    {
        AudioManager.instance.Play("Theme");
    }
}
