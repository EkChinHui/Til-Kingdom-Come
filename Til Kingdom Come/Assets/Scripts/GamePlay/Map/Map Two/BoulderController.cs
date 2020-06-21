using System;
using GamePlay.Information;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GamePlay.Map.Map_Two
{
    public class BoulderController : MonoBehaviour
    {
        private void Awake()
        {
            ScoreKeeper.resetPlayersEvent += DestroyBoulder;
        }

        // Start is called before the first frame update
        private void Start()
        {
            float randomSize = Random.Range(1, 3);
            transform.localScale = new Vector3(randomSize, randomSize, 1);
        }

        private void DestroyBoulder()
        {
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            ScoreKeeper.resetPlayersEvent -= DestroyBoulder;
        }
    }
}
