﻿using JetBrains.Annotations;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Xml.Schema;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static int totalPlayers = 0;

    [Header("fields")]
    [HideInInspector] public Rigidbody2D rb;
    private Animator anim;
    public ScoreKeeper scoreKeeper;
    public int Score = 0;
    public Skill skill;
    public PlayerController otherPlayer;
    public int playerNo;
    

    private enum State { idle, run, dead}
    private State state = State.idle;

    [Header("Movement")]
    public float runSpeed = 4f;
    public float rollSpeed = 23f;
    private bool canRoll = true;
    [HideInInspector] public bool isActing = false;
    private bool isSilenced = false;
    private Vector2 originalPos;

    [Header("Input")]
    // note to use getAxis for multiplayer mode so user can change their input
    public KeyCode leftKey;
    public KeyCode rightKey;
    public KeyCode rollKey;
    public KeyCode blockKey;
    public KeyCode attackKey;
    public KeyCode skillKey;
    private bool attemptLeft = false;
    private bool attemptRight = false;
    private bool attemptRoll = false;
    private bool attemptAttack = false;
    private bool attemptBlock = false;
    private bool attemptSkill = false;

    private float attackAnim;
    private float blockAnim;
    private float rollAnim;
    private float attackCooldown = 0.4f;
    private float reactionDelay = 0.2f;
    private float blockCooldown = 0.4f;
    private float rollCooldown = 0.6f;
  


    [Header("Combat")]
    public Transform attackPoint;
    public float attackRange = 2.43f;
    private float knockDistAttacking = 8f;
    private float knockDistBlocking = 4f;
    private int damage = 1;
    private LayerMask enemylayers;
    private bool isAttacking = false;
    private bool isBlocking = false;

    [Header("Health")]
    // Health system to make it convenient to change
    private int maxHealth = 1;
    private int currentHealth;

    private void Awake()
    {
        // remember the original position of the players so match can be reset
        originalPos = gameObject.transform.position;
        PlayerController.totalPlayers = 0;

    }

    private void Start()
    {
        // Instantiate variables on creation
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        UpdateAnimClipTimes();
        currentHealth = maxHealth;
        enemylayers = LayerMask.GetMask("Player");
        attackPoint = gameObject.transform.GetChild(0);
        skill = GetComponent<Skill>();

        totalPlayers++;
        playerNo = totalPlayers;
    }

    public void Update()
    {
        InputManager();
        if (isActing || state == State.dead)
        {
            // if player is already acting, prevent player from doing other moves
            if (isActing && isSilenced)
            {
                Move();
            }
        }else if (attemptSkill)
        {
            skill.Cast(this, otherPlayer);
            anim.SetTrigger(skill.SkillName);
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
        else
        {
            Move();
        }
        anim.SetInteger("state", (int) state);
    }

    private void Move()
    {
        if (attemptRoll && canRoll && !isSilenced)
        {
            Roll();
        }
        else if (attemptRight)
        {
            rb.velocity = new Vector2(runSpeed, rb.velocity.y);
            if (transform.rotation.y != 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            state = State.run;
        }
        else if (attemptLeft)
        {
            rb.velocity = new Vector2(-1 * runSpeed, rb.velocity.y);
            if (transform.rotation.y == 0)
            {
                transform.rotation = Quaternion.Euler(0, 180f, 0);
            }
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
            if (transform.rotation.y != 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        else if (attemptLeft)
        {
            StartCoroutine(rollAnimDelay());
            if (transform.rotation.y == 0)
            {
                transform.rotation = Quaternion.Euler(0, 180f, 0);
            }
        }

    }

    private IEnumerator rollAnimDelay()
    {
        canRoll = false;
        isActing = true;
        anim.SetTrigger("Roll");
        runSpeed = rollSpeed;
        Move();
        yield return new WaitForSeconds(rollAnim);
        runSpeed = 4f;
        isSilenced = true;
        yield return new WaitForSeconds(rollCooldown);
        isSilenced = false;
        isActing = false;
        canRoll = true;
    }


    public void Attack()
    {
        // Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemylayers);
        // maximum distance between both players for attack to be successful
        float attackDistance = 1.934f + attackRange;
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
                    // otherplayer succesfully defends against attack
                    this.knockBack(knockDistAttacking);
                    otherPlayer.knockBack(knockDistBlocking);

                }
                else if (!otherPlayer.canRoll)
                {
                    // the enemy is rolling and is invulnerable
                }
                else if (otherPlayer.isAttacking && (Mathf.Abs(otherPlayer.transform.position.x - transform.position.x) <= attackDistance))
                {
                    // the enemy attacked first and is in range
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
        // reaction delay to allow opponent to react
        yield return new WaitForSeconds(reactionDelay);
        isAttacking = true;
        Attack();
        yield return new WaitForSeconds(attackAnim - reactionDelay);
        isSilenced = true;
        yield return new WaitForSeconds(attackCooldown);
        isSilenced = false;
        isActing = false;
        isAttacking = false;
    }

    private IEnumerator BlockAnimDelay()
    {
        anim.SetTrigger("Block");
        isBlocking = true;
        isActing = true;
        yield return new WaitForSeconds(blockAnim);
        isSilenced = true;
        isBlocking = false;
        yield return new WaitForSeconds(blockCooldown);
        isSilenced = false;
        isActing = false;
    }

    public bool isShieldUp ()
    {
        return isBlocking;
    }

    public void knockBack(float distance)
    {
        if (transform.localScale.x < 0)
        {
            rb.velocity = new Vector2(distance, rb.velocity.y);
        } else
        {
            rb.velocity = new Vector2(-distance, rb.velocity.y);
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
        Debug.Log(scoreKeeper);
        Debug.Log(playerNo);
        scoreKeeper.updateWins(playerNo);

        state = State.dead;

        // die animation
        anim.SetBool("Dead", true);

        // Disable sprite
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;
        this.enabled = false;
    }

    private void InputManager()
    {
        attemptLeft = Input.GetKey(leftKey);
        attemptRight = Input.GetKey(rightKey);
        attemptRoll = Input.GetKeyDown(rollKey);
        attemptAttack = Input.GetKeyDown(attackKey);
        attemptBlock = Input.GetKeyDown(blockKey);
        attemptSkill = Input.GetKeyDown(skillKey);
    }

    /*
    private void SetKeysPlayer2()
    {
        if (gameObject.tag == "Player2")
        {
            leftKey= "a";
            rightKey = "d";
            rollKey = "s";
            attackKey = "f";
            blockKey = "g";
            skillKey = "h";
            enemylayers = LayerMask.GetMask("Player1");
        } 
    }
    */

    // draws wireframe for attack point to make it easier to set attack range
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void ResetPlayer()
    {
        anim.SetBool("Dead", false);
        state = State.idle;
        this.enabled = true;
        GetComponent<Collider2D>().enabled = true;
        GetComponent<Rigidbody2D>().simulated = true;
        gameObject.transform.position = originalPos;
        if (gameObject.CompareTag("Player2"))
        {
            gameObject.transform.localScale = new Vector2(-1, 1);
        } else if (gameObject.CompareTag("Player1"))
        {
            gameObject.transform.localScale = new Vector2(1, 1);
        }
    }

    // updates animation times
    public void UpdateAnimClipTimes()
    {
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        foreach(AnimationClip clip in clips)
        {
            switch(clip.name)
            {
                case "Attack":
                    attackAnim = clip.length;
                    break;
                case "Block":
                    blockAnim = clip.length;
                    break;
                case "Roll":
                    rollAnim = clip.length;
                    break;
            }
        }
    }

}
