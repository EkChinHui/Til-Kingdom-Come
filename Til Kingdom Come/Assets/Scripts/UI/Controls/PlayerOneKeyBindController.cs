using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerOneKeyBindController : MonoBehaviour
{
    private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();
    public TextMeshProUGUI left, right, roll, attack, block, skill;
    private GameObject currentKey;
    void Start()
    {
        keys.Add("Left", (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P1Left", "A")));
        keys.Add("Right", (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P1Right", "D")));
        keys.Add("Roll", (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P1Roll", "S")));
        keys.Add("Attack", (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P1Attack", "F")));
        keys.Add("Block", (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P1Block", "G")));
        keys.Add("Skill", (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P1Skill", "H")));


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
                Debug.Log(e);
                // check for duplicate keys
                bool isDuplicate = false;
                foreach (var key in keys)
                {
                    if (key.Key != currentKey.name && key.Value == e.keyCode)
                    {
                        isDuplicate = true;
                        break;
                    }
                }

                // check for KeyCode.None
                bool isNone = e.keyCode == KeyCode.None;

                if (!isDuplicate && !isNone)
                {
                    keys[currentKey.name] = e.keyCode;
                    currentKey.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = e.keyCode.ToString();
                    currentKey = null;
                }
                else
                {
                    Debug.Log("Duplicate key detected.");
                }
            }
        }
    }
    public void ChangeKey(GameObject clicked)
    {
        Debug.Log("Player 1 changing key binding for " + clicked.name + ".");
        currentKey = clicked;
        currentKey.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Choose Key";
    }
    public void SaveKeys()
    {
        foreach (var key in keys)
        {
            PlayerPrefs.SetString("P1" + key.Key, key.Value.ToString());
        }

        PlayerPrefs.Save();
    }
}
