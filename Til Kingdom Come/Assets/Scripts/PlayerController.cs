using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerController : MonoBehaviour
{
    [Header("fields")]
    private Rigidbody2D rb;
    private Animator anim;
    public ScoreKeeper scoreKeeper;

    [Header("Achivements")]
    public int wins;
    public int losses;

    private enum State { idle, run }
    private State state = State.idle;

    [Header("Movement")]
    public float runSpeed = 4f;
    public float rollSpeed = 13f;
    private float rollTime = 0.3f;
    public float rollDelay = 0.5f;
    public bool canRoll = true;
    public bool isActing = false;

    [Header("Input")]
    // note to use getAxis for multiplayer mode so user can change their input
    private string left = "a";
    private string right = "d";
    private string down = "s";
    private string block = ".";
    private string attack = "/";
    private bool attemptLeft = false;
    private bool attemptRight = false;
    private bool attemptRoll = false;
    private bool attemptAttack = false;
    private bool attemptBlock = false;

    [Header("Combat")]
    public Transform attackPoint;
    public float attackRange = 2.43f;
    public float attackDelay = 0.5f;
    public float blockDelay = 0.7f;
    public float knockDist = 10f;
    public int damage = 1;
    public float reactionDelay = 0.2f;
    // can only hit players in the enemy layer, figure out how this works in multiplayer
    public LayerMask enemylayers;
    private bool isAttacking = false;
    private bool isBlocking = false;

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
        enemylayers = LayerMask.GetMask("Player2");

        // set keys for player 2 in local multiplayer test
        SetKeysEnemy();
    }

    public void Update()
    {
        InputManager();
        if (isActing)
        {
            // if player is already acting, prevent player from doing other moves
        }
        else if (attemptAttack)
        {
            if (!isAttacking)
            {
                StartCoroutine(AttackAnimDelay());
            }
        }
        else if (attemptBlock)
        {
            StartCoroutine(BlockAnimDelay());
        }
        else if (!isAttacking)
        {
            Move();
        }
        anim.SetInteger("state", (int)state);
    }

    private void Move()
    {
        if (attemptRoll && canRoll) 
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
            StartCoroutine(rollAnimDelay());
            anim.SetTrigger("Roll");
            transform.localScale = new Vector2(1, 1);
        }
        else if (attemptLeft)
        {
            StartCoroutine(rollAnimDelay());
            anim.SetTrigger("Roll");
            transform.localScale = new Vector2(-1, 1);
        }

    }

    private IEnumerator rollAnimDelay()
    {
        canRoll = false;
        isActing = true;
        runSpeed = rollSpeed;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().gravityScale = 0f;
        Move();
        yield return new WaitForSeconds(rollTime);
        runSpeed = 4f;
        isActing = false;
        GetComponent<Collider2D>().enabled = true;
        GetComponent<Rigidbody2D>().gravityScale = 3.01f;
        yield return new WaitForSeconds(rollDelay);
        canRoll = true;
    }


    public void Attack()
    {
        // Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemylayers);

        // Damage enemies
        foreach(Collider2D enemy in hitEnemies)
        {
            if (enemy.GetComponent<PlayerController>() != null)
            {
                PlayerController otherPlayer = enemy.GetComponent<PlayerController>();
                float enemyDirection = otherPlayer.transform.localScale.x;
                float myDirection = transform.localScale.x;
                if (otherPlayer.isShieldUp() && (enemyDirection != myDirection))
                {
                    // otherplayer succesfully defends against attack\
                    this.knockBack();
                    //otherPlayer.knockBack();

                }
                else 
                {
                    enemy.GetComponent<PlayerController>().TakeDamage(damage);
                }
            
            }
        }
    }

    private IEnumerator AttackAnimDelay()
    {
        isActing = true;
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(reactionDelay);
        Attack(); 
        isAttacking = true;
        yield return new WaitForSeconds(attackDelay);
        isActing = false;
        isAttacking = false;
    }

    private IEnumerator BlockAnimDelay()
    {
        anim.SetTrigger("Block");
        isBlocking = true;
        isActing = true;
        yield return new WaitForSeconds(blockDelay);
        isBlocking = false;
        isActing = false;
    }

    public bool isShieldUp ()
    {
        return isBlocking;
    }

    public void knockBack()
    {
        if (transform.localScale.x < 0)
        {
            rb.velocity = new Vector2(knockDist, rb.velocity.y);
        } else
        {
            rb.velocity = new Vector2(-knockDist, rb.velocity.y);
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
        losses++;
        if (gameObject.tag == "Player1")
        {
            scoreKeeper.updateWins(2);
        }
        else if (gameObject.tag == "Player2")
        {
            scoreKeeper.updateWins(1);
        }

        // die animation
        anim.SetTrigger("Dead");

        // Disable sprite
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;
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
        if (gameObject.tag == "Player2")
        {
            left = "left";
            right = "right";
            down = "down";
            attack = "]";
            block = "\\";
            enemylayers = LayerMask.GetMask("Player1");
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
