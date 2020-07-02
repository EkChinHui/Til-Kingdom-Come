using System;
using System.Collections.Generic;
using UnityEngine;

namespace GamePlay.Player
{
    public class PlayerAfterImagePool : MonoBehaviour
    {
        public GameObject afterImagePrefab1;
        public GameObject afterImagePrefab2;
        
        public Queue<GameObject> playerOneAfterImages = new Queue<GameObject>();
        public Queue<GameObject> playerTwoAfterImages = new Queue<GameObject>();
        public static PlayerAfterImagePool Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            GrowPool(playerOneAfterImages);
            GrowPool(playerTwoAfterImages);
        }

        private void GrowPool(Queue<GameObject> queue)
        {
            var prefab = queue == playerOneAfterImages ? afterImagePrefab1 : afterImagePrefab2;
            for (int i = 0; i < 10; i++)
            {
                var instanceToAdd = Instantiate(prefab, transform, true);
                AddToPool(instanceToAdd, queue);
            }
        }

        public void AddToPool(GameObject instance, Queue<GameObject> queue)
        {
            instance.SetActive(false);
            queue.Enqueue(instance);
        }

        public GameObject GetFromPool(int playerNo)
        {
            var queue = playerOneAfterImages;
            switch (playerNo)
            {
                case 1:
                    queue = playerOneAfterImages;
                    break;
                case 2:
                    queue = playerTwoAfterImages;
                    break;
            }
            if (queue.Count == 0)
            {
                GrowPool(queue);
            }

            var instance = queue.Dequeue();
            instance.SetActive(true);
            return instance;
        }
    }
}
