using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerTwoKeyBindController : MonoBehaviour
{
    private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();
    public TextMeshProUGUI left, right, roll, attack, block, skill;
    private GameObject currentKey;
    void Start()
    {
        keys.Add("Left", (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P2Left", "LeftArrow")));
        keys.Add("Right", (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P2Right", "RightArrow")));
        keys.Add("Roll", (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P2Roll", "DownArrow")));
        keys.Add("Attack", (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P2Attack", "Slash")));
        keys.Add("Block", (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P2Block", "Period")));
        keys.Add("Skill", (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P2Skill", "Comma")));


        left.text = keys["Left"].ToString();
        right.text = keys["Right"].ToString();
        roll.text = keys["Roll"].ToString();
        attack.text = keys["Attack"].ToString();
        block.text = keys["Block"].ToString();
        skill.text = keys["Skill"].ToString();
    }
    void OnGUI()
    {
        if (currentKey != null)
        {
            Event e = Event.current;
            if (e.isKey)
            {
                keys[currentKey.name] = e.keyCode;
                currentKey.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = e.keyCode.ToString();
                currentKey = null;
            }
        }
    }
    public void ChangeKey(GameObject clicked)
    {
        Debug.Log("Player 2 changing key binding for " + clicked.name + ".");
        currentKey = clicked;
        currentKey.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Choose Key";
    }
    public void SaveKeys()
    {
        foreach (var key in keys)
        {
            PlayerPrefs.SetString("P2" + key.Key, key.Value.ToString());
        }

        PlayerPrefs.Save();
    }
}
