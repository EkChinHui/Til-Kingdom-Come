using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLoader : MonoBehaviour
{
    // Start is called before the first frame update
    public static string MapToLoad = "Map1";

    [RuntimeInitializeOnLoadMethod]
    void Start()
    {
        foreach (Transform child in transform)
        {
            if(child.gameObject.name == MapToLoad)
            {
                child.gameObject.SetActive(true);
            }
        }
    }

    private void Awake()
    {
        
    }


}
