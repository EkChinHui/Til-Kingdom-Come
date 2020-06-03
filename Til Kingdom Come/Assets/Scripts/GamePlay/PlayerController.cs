using System;
using System.Collections;
using GamePlay.Skills;
using UnityEngine;

namespace GamePlay
{
    public class PlayerController : MonoBehaviour
    {
        public static int totalPlayers;

        [Header("fields")]
        [HideInInspector] public Rigidbody2D rb;
        private Animator anim;
        public ScoreKeeper scoreKeeper;
        public int score;
        public Skill skill;
        public PlayerController otherPlayer;
        public int playerNo;
    

        private enum State { Idle, Run, Dead}
        private State state = State.Idle;

        [Header("Movement")]
        public float runSpeed = 4f;
        public float rollSpeed = 23f;
        private bool canRoll = true;
        [HideInInspector] public bool isActing;
        private bool isSilenced;
        private Vector2 originalPos;

        [Header("Input")]
        // note to use getAxis for multi-player mode so user can change their input
        public KeyCode leftKey;
        public KeyCode rightKey;
        public KeyCode rollKey;
        public KeyCode blockKey;
        public KeyCode attackKey;
        public KeyCode skillKey;
        private bool attemptLeft;
        private bool attemptRight;
        private bool attemptRoll;
        private bool attemptAttack;
        private bool attemptBlock;
        private bool attemptSkill;
        
        private const float AttackCooldown = 0.4f;
        private const float ReactionDelay = 0.2f;
        private const float BlockCooldown = 0.4f;
        private const float RollCooldown = 0.6f;


        [Header("Combat")]
        public Transform attackPoint;
        public float attackRange = 2.43f;
        private const float KnockDistAttacking = 8f;
        private const float KnockDistBlocking = 4f;
        private const int Damage = 1;
        private LayerMask enemylayer;
        private bool isAttacking;
        private bool isBlocking;

        [Header("Health")]
        // Health system to make it convenient to change
        private const int MaxHealth = 1;

        private int currentHealth;
        private static readonly int Roll1 = Animator.StringToHash("Roll");
        private static readonly int Dead = Animator.StringToHash("Dead");
        private static readonly int Attack1 = Animator.StringToHash("Attack");
        private static readonly int Block = Animator.StringToHash("Block");

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
            currentHealth = MaxHealth;
            enemylayer = LayerMask.GetMask("Player");
            attackPoint = gameObject.transform.GetChild(0);
            skill = GetComponent<Skill>();

            totalPlayers++;
            playerNo = totalPlayers;
        }

        public void Update()
        {
            InputManager();
            if (isActing || state == State.Dead)
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
                if (Math.Abs(transform.rotation.y) > Mathf.Epsilon)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }

                state = State.Run;
            }
            else if (attemptLeft)
            {
                rb.velocity = new Vector2(-1 * runSpeed, rb.velocity.y);
                if (Math.Abs(transform.rotation.y) < Mathf.Epsilon)
                {
                    transform.rotation = Quaternion.Euler(0, 180f, 0);
                }
                state = State.Run;
            }
            else
            {
                state = State.Idle;
            }
        }

        private void Roll()
        {
            if (attemptRight)
            {
                StartCoroutine(RollAnimDelay());
                if (Math.Abs(transform.rotation.y) > Mathf.Epsilon)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
            else if (attemptLeft)
            {
                StartCoroutine(RollAnimDelay());
                if (Math.Abs(transform.rotation.y) < Mathf.Epsilon)
                {
                    transform.rotation = Quaternion.Euler(0, 180f, 0);
                }
            }

        }

        private IEnumerator RollAnimDelay()
        {
            canRoll = false;
            isActing = true;
            anim.SetTrigger(Roll1);
            runSpeed = rollSpeed;
            Move();
            yield return new WaitForSeconds(AnimationTimes.instance.RollAnim);
            runSpeed = 4f;
            isSilenced = true;
            yield return new WaitForSeconds(RollCooldown);
            isSilenced = false;
            isActing = false;
            canRoll = true;
        }


        private void Attack()
        {
            // Detect enemies in range of attack
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemylayer);
            // maximum distance between both players for attack to be successful
            float attackDistance = 1.934f + attackRange;
            // Damage enemies
            foreach(Collider2D enemy in hitEnemies)
            {
                if (enemy.GetComponent<PlayerController>() == null) continue;
                PlayerController otherPlayer = enemy.GetComponent<PlayerController>();
                float enemyDirection = otherPlayer.transform.localScale.x;
                float myDirection = transform.localScale.x;
                if (otherPlayer.IsShieldUp() && (Math.Abs(enemyDirection - myDirection) > Mathf.Epsilon))
                {
                    // otherplayer succesfully defends against attack
                    this.KnockBack(KnockDistAttacking);
                    otherPlayer.KnockBack(KnockDistBlocking);

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
                    enemy.GetComponent<PlayerController>().TakeDamage(Damage);
                }
            }
        }

        private IEnumerator AttackAnimDelay()
        {
            isActing = true;
            anim.SetTrigger(Attack1);
            // reaction delay to allow opponent to react
            yield return new WaitForSeconds(ReactionDelay);
            isAttacking = true;
            Attack();
            yield return new WaitForSeconds(AnimationTimes.instance.AttackAnim - ReactionDelay);
            isSilenced = true;
            yield return new WaitForSeconds(AttackCooldown);
            isSilenced = false;
            isActing = false;
            isAttacking = false;
        }

        private IEnumerator BlockAnimDelay()
        {
            anim.SetTrigger(Block);
            isBlocking = true;
            isActing = true;
            yield return new WaitForSeconds(AnimationTimes.instance.BlockAnim);
            isSilenced = true;
            isBlocking = false;
            yield return new WaitForSeconds(BlockCooldown);
            isSilenced = false;
            isActing = false;
        }

        public bool IsShieldUp ()
        {
            return isBlocking;
        }

        public void KnockBack(float distance)
        {
            rb.velocity = transform.localScale.x < 0 
                ? new Vector2(distance, rb.velocity.y) 
                : new Vector2(-distance, rb.velocity.y);
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
            scoreKeeper.UpdateWins(playerNo);

            state = State.Dead;

            // die animation
            anim.SetBool(Dead, true);

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
            anim.SetBool(Dead, false);
            state = State.Idle;
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



    }
}
