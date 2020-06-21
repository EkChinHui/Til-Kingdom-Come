using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public GameObject next;
    public GameObject back;


    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKey(KeyCode.Return))
        {
            next.GetComponent<Button>().onClick.Invoke();
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            back.GetComponent<Button>().onClick.Invoke();
        }
    }
}
