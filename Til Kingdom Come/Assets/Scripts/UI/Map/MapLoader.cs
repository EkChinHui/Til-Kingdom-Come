using UnityEngine;

namespace UI.Map
{
    public class MapLoader : MonoBehaviour
    {
        public static string mapToLoad = "Map 1";
        
        // Sets the Map to the selected map
        private void Start()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(child.gameObject.name == mapToLoad);
            }
        }
    }
}
