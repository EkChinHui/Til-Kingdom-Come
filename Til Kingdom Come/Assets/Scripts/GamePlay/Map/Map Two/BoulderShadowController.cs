﻿using UnityEngine;

namespace GamePlay.Map.Map_Two
{
    public class BoulderShadowController : MonoBehaviour
    {
        private float y = -3.10f;
        private float groundHeight = -2.3f;
        public GameObject boulder;
        private float scaleRatio = 6;
        private void Start()
        {
            transform.position = new Vector2(transform.position.x, y);
        }

        // Update is called once per frame
        private void Update()
        {
            if (boulder != null)
            {
                // From the lowest point of the boulder
                var distanceFromGround = boulder.transform.position.y - groundHeight;
                if (distanceFromGround < 1) {
                    distanceFromGround = 1;
                }
                
                transform.localScale = new Vector3(scaleRatio / distanceFromGround, transform.localScale.y, transform.localScale.z);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}