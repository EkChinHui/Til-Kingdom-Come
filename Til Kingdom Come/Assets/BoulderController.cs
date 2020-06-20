using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float randomSize = Random.Range(1, 3);
        transform.localScale = new Vector3(randomSize, randomSize, 1);
    }
}
