using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NicknamePanelController : MonoBehaviour
{
    public TMP_InputField text;

    private void OnEnable()
    {
        text.text = PlayerPrefs.GetString("Nickname", "");
    }
}
