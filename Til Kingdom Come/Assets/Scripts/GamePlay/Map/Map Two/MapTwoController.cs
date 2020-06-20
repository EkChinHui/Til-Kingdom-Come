using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTwoController : MonoBehaviour
{
    private float frequencyOfBoulderSpawn = 0.01f;
    private float boulderSpawnYAxis = 40;
    public GameObject boulder;
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.PlayMusic("Battle Theme 2");
    }

    void Update()
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
