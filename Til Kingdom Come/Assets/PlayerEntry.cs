using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerEntry : MonoBehaviour
{
    public TextMeshProUGUI playerName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetName(string name)
    {
        playerName.text = name;
    }
}
