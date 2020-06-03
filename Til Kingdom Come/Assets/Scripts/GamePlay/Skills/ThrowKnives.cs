using UnityEngine;

namespace GamePlay.Skills
{
    public class ThrowKnives : Skill
    {
        public Transform rangePoint;
        public GameObject knife;

        // Start is called before the first frame update
        void Start()
        {
            rangePoint = gameObject.transform.GetChild(1);
            Debug.Log(rangePoint.name);
            this.skillName = "Throw Knives";
            this.skillDescription = "Throws knives at opponent";
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        public override void Cast(PlayerController player, PlayerController opponent)
        {
            Instantiate(knife, rangePoint.position, rangePoint.rotation);
        }

    }
}
