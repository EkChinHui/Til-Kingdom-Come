using UnityEngine;

namespace UI
{
    public class MapLoader : MonoBehaviour
    {
        public static string mapToLoad = "Map1";
        
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
