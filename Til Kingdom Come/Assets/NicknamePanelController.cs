using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NicknamePanelController : MonoBehaviour
{
    public TextMeshProUGUI text;

    private void OnEnable()
    {
        text.SetText(PlayerPrefs.GetString("Nickname", ""));
    }
}
