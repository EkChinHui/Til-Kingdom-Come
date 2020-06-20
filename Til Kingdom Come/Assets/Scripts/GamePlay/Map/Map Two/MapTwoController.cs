using UnityEngine;

namespace GamePlay.Map.Map_Two
{
    public class MapTwoController : MonoBehaviour
    {
        private float frequencyOfBoulderSpawn = 0.005f;
        private float boulderSpawnYAxis = 25;
        public GameObject boulder;
        public GameObject endPanelBoard;
        // Start is called before the first frame update
        private void Start()
        {
            AudioManager.instance.PlayMusic("Battle Theme 2");
        }

        private void Update()
        {
            if (!endPanelBoard.activeSelf)
            {
                float randomNumber = Random.Range(0, 1f);
                if (randomNumber < frequencyOfBoulderSpawn)
                {
                    float boulderSpawnXAxis = Random.Range(-15, 15);
                    var boulderSpawnPosition = new Vector3(boulderSpawnXAxis, boulderSpawnYAxis, 0);
                    Instantiate(boulder, boulderSpawnPosition, Quaternion.identity);
                }
            }
        }
    }
}
