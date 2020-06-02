using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowKnives : Skill
{
    public Transform rangePoint;
    public GameObject knife;
    public Transform spawnPoint;

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
