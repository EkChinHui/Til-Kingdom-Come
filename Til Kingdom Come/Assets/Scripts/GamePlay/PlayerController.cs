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
        public Rigidbody2D rb;
        public Animator anim;
        private new Collider2D collider2D;
        public PlayerController otherPlayer;
        public int playerNo;
        public PlayerInput playerInput;

        private enum State { Idle, Run, Dead}
        private State state = State.Idle;
        public enum CombatState { Vulnerable, Blocking, Rolling, Attacking }
        public CombatState combatState = CombatState.Vulnerable;

        [Header("Movement")] 
        public float runSpeed = 4f;
        private Vector2 originalPos;

        [Header("Combat")]
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

        #region Events
        public delegate void DeathDelegate(int playerNo);
        public static event DeathDelegate DeathEvent;
        #endregion

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
            collider2D = GetComponent<Collider2D>();
            anim = GetComponent<Animator>();
            skill = GetComponent<Skill>();
            currentHealth = MaxHealth;
            ScoreKeeper.ResetPlayersEvent += ResetPlayer;

            totalPlayers++;
            playerNo = totalPlayers;
        }  

        public void Update()
        {
            if (state == State.Dead)
            {
            }else if (playerInput.AttemptSkill)
            {
                skill.Cast(this, otherPlayer);
            }
            else if (playerInput.AttemptAttack)
            {
                attack.Cast(this, otherPlayer);
            }
            else if (playerInput.AttemptBlock)
            {
                block.Cast(this, otherPlayer);
            } else if (playerInput.AttemptRoll)
            {
                roll.Cast(this, otherPlayer);
            }
            else if (combatState != CombatState.Rolling)
            {
                Move();
            }
            anim.SetInteger("state", (int) state);
        }

        private void Move()
        {
            if (playerInput.AttemptRight)
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
            var velocity = rb.velocity;
            velocity = transform.localScale.x < 0 
                ? new Vector2(distance, velocity.y)
                : new Vector2(-distance, velocity.y);
            rb.velocity = velocity;
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

        private void Die()
        {
            DeathEvent?.Invoke(playerNo);
            state = State.Dead;

            // die animation
            anim.SetBool("Dead", true);

            // Disable sprite
            collider2D.enabled = false;
            rb.simulated = false;
            enabled = false;
        }
        
        private void ResetPlayer()
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