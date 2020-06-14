using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderShadowController : MonoBehaviour
{
    private float y = -3.10f;
    private float groundHeight = -2.3f;
    public GameObject boulder;
    void Start()
    {
        transform.position = new Vector2(transform.position.x, y);
    }

    // Update is called once per frame
    void Update()
    {
        if (boulder != null)
        {
            var distanceFromGround = boulder.transform.position.y - groundHeight;
            transform.localScale = new Vector3(6 * 1 / distanceFromGround, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}