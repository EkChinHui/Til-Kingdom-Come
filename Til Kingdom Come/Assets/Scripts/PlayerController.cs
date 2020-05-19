using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("fields")]
    private Rigidbody2D rb;
    private Animator anim;

    private enum State { idle, run}//, attack, block, roll, die}
    private State state = State.idle;

    [Header("Movement")]
    // note to use getAxis for multiplayer mode so user can change their input
    private string left = "a";
    private string right = "d";
    private string down = "s";
    private string block = ".";
    private string attack = "/";
    public float runSpeed = 2;
    public float rollSpeed = 20;

    [Header("Input")]
    private bool attemptLeft = false;
    private bool attemptRight = false;
    private bool attemptRoll = false;
    private bool attemptAttack = false;
    private bool attemptBlock = false;


    [Header("Attacking")]
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public float attackRate = 2f;
    public LayerMask enemylayers; 
    // can only hit players in the enemy layer, figure out how this works in multiplayer
    private float nextAttackTime = 0;

    [Header("Health")]
    // Health system to make it convenient to change
    public int maxHealth = 1;
    private int currentHealth;

    private void Start()
    {
        // Instantiate variables on creation
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;

        // set keys for player 2 in local multiplayer test
        SetKeysEnemy();
    }

    public void Update()
    {
        InputManager();
        if (attemptAttack && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + 1f / attackRate;
        }
        else if (attemptBlock)
        {
            Block();
        }
        else
        {
            Move();
        }
        anim.SetInteger("state", (int)state);
    }

    private void Move()
    {
        if (attemptRoll) 
        {
            Roll();
        }
        else if (attemptRight)
        {
            rb.velocity = new Vector2(runSpeed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
            state = State.run;
        }
        else if (attemptLeft)
        {
            rb.velocity = new Vector2(-1 * runSpeed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
            state = State.run;
        }
        else
        {
            state = State.idle;
        }
    }

    public void Roll()
    {
        if (attemptRight)
        {
            rb.velocity = new Vector2(rollSpeed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }
        else
        {
            rb.velocity = new Vector2(-1 * rollSpeed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }
        anim.SetTrigger("Roll");
    }

    private void Block()
    {
        anim.SetTrigger("Block");
    }

    public void Attack()
    {
        // Play an attack animation
        anim.SetTrigger("Attack");

        // Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemylayers);

        // Damage enemies
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<PlayerController>().TakeDamage(1);
        }
    }

    public void TakeDamage(int damageTaken)
    {
        currentHealth -= damageTaken;
        if (currentHealth <= 0)
        {
            Die();
        } 
        else
        {
            // hurt;
        }
    }

    void Die()
    {
        Debug.Log("die");
        // die animation
        anim.SetTrigger("Dead");

        // Disable sprite
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }

    private void InputManager()
    {
        attemptLeft = Input.GetKey(left);
        attemptRight = Input.GetKey(right);
        attemptRoll = Input.GetKeyDown(down);
        attemptAttack = Input.GetKeyDown(attack);
        attemptBlock = Input.GetKeyDown(block);
    }

    private void SetKeysEnemy()
    {
        if (gameObject.tag == "Enemy")
        {
            left = "left";
            right = "right";
            down = "down";
            attack = "]";
            block = "\\";
        } 
    }

    // draws wireframe for attack point to make it easier to set attack range
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
