using System;
using GamePlay.Player;
using UnityEngine;

namespace GamePlay.Skills
{
    public class FireBreathGround : MonoBehaviour
    {
        private BoxCollider2D coll2D;
        private int damagePerTick = 5;
        private float nextTime;
        private float timeBetweenTicks = 0.5f;

        // Start is called before the first frame update
        void Start()
        {
            coll2D = gameObject.GetComponent<BoxCollider2D>();
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            var player = other.gameObject.GetComponent<PlayerController>();
            // next time the player can be damaged
            if (nextTime < Time.time)
            {
                player.TakeDamage(damagePerTick);
                nextTime = Time.time + timeBetweenTicks;
            }
        }
        
    }
}
