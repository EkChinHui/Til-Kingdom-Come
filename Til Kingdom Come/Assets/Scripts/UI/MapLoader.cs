﻿using UnityEngine;

namespace UI
{
    public class MapLoader : MonoBehaviour
    {
        public static string mapToLoad = "Map1";

        //[RuntimeInitializeOnLoadMethod]
        private void Start()
        {
            foreach (Transform child in transform)
            {
                if(child.gameObject.name == mapToLoad)
                {
                    child.gameObject.SetActive(true);
                }
                else
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }
}