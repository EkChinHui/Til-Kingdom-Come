using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class ForcePush : Skill
{
    [SerializeField] private float pushDistance = 6f;
    [SerializeField] private float stunDuration = 0.2f;

    private void Start()
    {
        this.skillName = "Force Push";
        this.skillDescription = "Pushes the enemy away from you";
        this.skillCooldown = 1f;
    }


    public override void Cast(PlayerController player, PlayerController opponent)
    {
        int pushDirection = opponent.transform.position.x - player.transform.position.x > 0 ? -1 : 1;
        Vector2 enemyVelocity = opponent.rb.velocity;
        opponent.knockBack(pushDirection * pushDistance);
        StartCoroutine(stun(opponent));
    }

    public IEnumerator stun(PlayerController opponent)
    {
        opponent.isActing = true;
        yield return new WaitForSeconds(stunDuration);
        opponent.isActing = false;
        yield return null;
    }
}
