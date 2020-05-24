using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("fields")]
    private Rigidbody2D rb;
    private Animator anim;
    public ScoreKeeper scoreKeeper;
    public int Score = 0;

    private enum State { idle, run , dead}
    private State state = State.idle;

    [Header("Movement")]
    public float runSpeed = 4f;
    public float rollSpeed = 23f;
    public bool canRoll = true;
    public bool isActing = false;
    public bool isSilenced = false;
    public Vector2 originalPos;

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

    private float attackAnim;
    private float blockAnim;
    private float rollAnim;
    private float attackCooldown = 0.4f;
    private float blockCooldown = 0.4f;
    private float rollCooldown = 0.6f;
    public float reactionDelay = 0.2f;


    [Header("Combat")]
    public Transform attackPoint;
    public float attackRange = 2.43f;
    public float knockDistAttacking = 8f;
    public float knockDistBlocking = 4f;
    public int damage = 1;
    // can only hit players in the enemy layer, figure out how this works in multiplayer
    public LayerMask enemylayers;
    private bool isAttacking = false;
    private bool isBlocking = false;

    [Header("Health")]
    // Health system to make it convenient to change
    public int maxHealth = 1;
    private int currentHealth;

    private void Awake()
    {
        originalPos = gameObject.transform.position;
    }

    private void Start()
    {
        // Instantiate variables on creation
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        UpdateAnimClipTimes();
        currentHealth = maxHealth;
        enemylayers = LayerMask.GetMask("Player2");
        attackPoint = gameObject.transform.GetChild(0);

        // set keys for player 2 in local multiplayer test
        SetKeysEnemy();
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
        anim.SetInteger("state", (int)state);
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
            transform.localScale = new Vector2(1, 1);
        }
        else if (attemptLeft)
        {
            StartCoroutine(rollAnimDelay());
            transform.localScale = new Vector2(-1, 1);
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
                    this.knockBack(knockDistAttacking);
                    otherPlayer.knockBack(knockDistBlocking);

                } else if (!otherPlayer.canRoll)
                {
                    // the enemy is rolling and is invulnerable;
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
        if (gameObject.tag == "Player1")
        {
            scoreKeeper.updateWins(2);
        }
        else if (gameObject.tag == "Player2")
        {
            scoreKeeper.updateWins(1);
        }

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

    public void UpdateAnimClipTimes()
    {
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        float cutshort = 0f;
        foreach(AnimationClip clip in clips)
        {
            switch(clip.name)
            {
                case "Attack":
                    attackAnim = clip.length - cutshort;
                    break;
                case "Block":
                    blockAnim = clip.length - cutshort;
                    break;
                case "Roll":
                    rollAnim = clip.length - cutshort;
                    break;
            }
        }
    }

}
