using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcePull : Skill
{
    [SerializeField] private float pullDistance = 6f;
    [SerializeField] private float stunDuration = 0.2f;

    private void Start()
    {
        this.skillName = "Force Pull";
        this.skillDescription = "Pulls the enemy towards you";
        this.skillCooldown = 1f;
    }

    public override void Cast(PlayerController player, PlayerController opponent)
    {
        Debug.Log("Pulls enemy");
        //PlayerController otherPlayer = GetComponent<PlayerController>();
        float enemyDirection = opponent.transform.localScale.x;
        float myDirection = player.transform.localScale.x;
        Vector2 enemyVelocity = opponent.rb.velocity;
        if (enemyDirection != myDirection)
        {
            opponent.knockBack(-pullDistance);
            StartCoroutine(stun(opponent));
        }
       
    }

    public IEnumerator stun(PlayerController opponent)
    {
        opponent.isActing = true;
        yield return new WaitForSeconds(stunDuration);
        opponent.isActing = false;
        yield return null;
    }
}
