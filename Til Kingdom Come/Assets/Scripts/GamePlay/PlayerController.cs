using System;
using System.Collections;
using GamePlay.Skills;
using UnityEngine;

namespace GamePlay
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {
        public static int totalPlayers;

        [Header("fields")]
        [HideInInspector] public Rigidbody2D rb;
        public Animator anim;
        public PlayerController otherPlayer;
        public int playerNo;
        public PlayerInput playerInput;

        private enum State { Idle, Run, Dead}
        private State state = State.Idle;

        [Header("Movement")]
        public float runSpeed = 4f;
        public float rollSpeed = 23f;
        private Vector2 originalPos;

        [Header("Combat")]
        public bool isAttacking;
        public bool isBlocking;
        public bool canRoll = true; 
        public bool isActing;
        public bool isSilenced;
        public LayerMask enemyLayer = 8;

        [Header("Health")]
        // Health system to make it convenient to change
        private const int MaxHealth = 1;
        public int currentHealth;

        [Header("Skills")] // FIX: update to a list of swappable skills
        public Attack attack;
        public Block block;
        public Roll roll;
        public Skill skill;

        public delegate void DeathDelegate(int playerNo);

        public static event DeathDelegate DeathEvent;
            
        
        public bool IsBlocking => isBlocking;

        private void Awake()
        {
            // remember the original position of the players so match can be reset
            originalPos = gameObject.transform.position;
            totalPlayers = 0;
        }

        private void Start()
        {
            // Instantiate variables on creation
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            currentHealth = MaxHealth;
            skill = GetComponent<Skill>();
            ScoreKeeper.ResetPlayersEvent += ResetPlayer;

            totalPlayers++;
            playerNo = totalPlayers;
        }

        public void Update()
        {
            if (isActing || state == State.Dead)
            {
                // if player is already acting, prevent player from doing other moves
                if (isActing && isSilenced)
                {
                    Move();
                }
            }else if (playerInput.AttemptSkill)
            {
                skill.Cast(this, otherPlayer);
            }
            else if (playerInput.AttemptAttack)
            {
                if (!isAttacking)
                {
                    attack.Cast(this, otherPlayer);
                }
            }
            else if (playerInput.AttemptBlock)
            {
                block.Cast(this, otherPlayer);
            }
            else
            {
                Move();
            }
            anim.SetInteger("state", (int) state);
        }

        public void Move()
        {
            if (playerInput.AttemptRoll && canRoll && !isSilenced)
            {
                roll.Cast(this, otherPlayer);
            }
            else if (playerInput.AttemptRight)
            {
                rb.velocity = new Vector2(runSpeed, rb.velocity.y);
                if (Math.Abs(transform.rotation.y) > Mathf.Epsilon)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }

                state = State.Run;
            }
            else if (playerInput.AttemptLeft)
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
            if (DeathEvent != null)
            {
                DeathEvent(playerNo);
            }
            state = State.Dead;

            // die animation
            anim.SetBool("Dead", true);

            // Disable sprite
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Rigidbody2D>().simulated = false;
            enabled = false;
        }
        
        public void ResetPlayer()
        {
            anim.SetBool("Dead", false);
            state = State.Idle;
            enabled = true;
            GetComponent<Collider2D>().enabled = true;
            GetComponent<Rigidbody2D>().simulated = true;
            gameObject.transform.position = originalPos;
        }

        private void OnDestroy()
        {
            ScoreKeeper.ResetPlayersEvent -= ResetPlayer;
        }
    }
}