﻿using System.Collections;
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
        int pullDirection = opponent.transform.position.x - player.transform.position.x > 0 ? 1 : -1;
        Vector2 enemyVelocity = opponent.rb.velocity;
        opponent.knockBack(pullDirection * pullDistance);
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
