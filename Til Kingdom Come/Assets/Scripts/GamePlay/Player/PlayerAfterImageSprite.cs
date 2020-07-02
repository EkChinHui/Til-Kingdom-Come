using UnityEngine;

namespace GamePlay.Player
{
    public class PlayerAfterImageSprite : MonoBehaviour
    {
        private Transform player;
        public int playerNo;
        private SpriteRenderer sr;
        private SpriteRenderer playerSR;

        //private float activeTime = 0.2f;
        private float timeActivated;
        private float alpha;
        private float alphaSet = 1f;
        private float alphaMultiplier = 0.90f;
    
        private Color color;

        private void OnEnable()
        {
            var playerName = "Player " + playerNo;
            foreach (var players in GameObject.FindGameObjectsWithTag("Player"))
            {
                
                if (playerName == players.name)
                {
                    player = players.transform;
                }
            }
            sr = GetComponent<SpriteRenderer>();
            playerSR = player.transform.GetComponent<SpriteRenderer>();

            alpha = alphaSet;
            sr.sprite = playerSR.sprite;
            transform.position = player.position;
            transform.rotation = player.rotation;
            timeActivated = Time.time;
        }

        // Update is called once per frame
        void Update()
        {
            alpha *= alphaMultiplier;
            color = new Color(1f, 1f, 1f, alpha);
            sr.color = color;
            
            var queue = playerNo == 1
                ? PlayerAfterImagePool.Instance.playerOneAfterImages
                : PlayerAfterImagePool.Instance.playerTwoAfterImages;
            if (Time.time >= timeActivated + Time.time)
            {
                PlayerAfterImagePool.Instance.AddToPool(gameObject, queue);
            }
        }
    }
}
