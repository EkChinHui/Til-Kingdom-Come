using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcePush : Skill
{
    [SerializeField] private float pushDistance = 6f;
    [SerializeField] private float stunDuration = 0.2f;

    private void Start()
    {
        this.skillName = "Force Pull";
        this.skillDescription = "Pulls the enemy towards you";
        this.skillCooldown = 1f;
    }

    public override void Cast(PlayerController player, PlayerController opponent)
    {
        Vector2 enemyVelocity = opponent.rb.velocity;
        opponent.knockBack(-pushDistance);
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
