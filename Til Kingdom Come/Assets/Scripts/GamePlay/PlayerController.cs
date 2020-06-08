using System;
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
        public PlayerController otherPlayer;
        public int playerNo;
        public PlayerInput playerInput;

        private enum State { Idle, Run}
        private State state = State.Idle;
        public enum CombatState { NonCombatState, Blocking, Rolling, Attacking, Dead}
        public CombatState combatState = CombatState.NonCombatState;

        [Header("Movement")] 
        private float runSpeed = 4f;
        private Vector2 originalPos;

        [Header("Combat")]
        public LayerMask enemyLayer = 8;

        [Header("Health")]
        // Health system to make it convenient to change
        private const int MaxHealth = 1;
        public int currentHealth;

        [Header("Skills")] 
        public Attack attack;
        public Block block;
        public Roll roll;
        public Skill skill;

        #region Events
        public static Action<int> onDeath;
        #endregion

        private void Awake()
        {
            // remember the original position of the players so match can be reset
            originalPos = gameObject.transform.position;
            totalPlayers++;
            playerNo = totalPlayers;
            ScoreKeeper.resetPlayersEvent += ResetPlayer;
            ScoreKeeper.passPlayerSkills += PassPlayerSkills;
        }

        private void Start()
        {
            // Instantiate variables on creation
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            currentHealth = MaxHealth;
        }

        public void Update()
        {
            if (combatState == CombatState.Dead) return; 
            
            if (playerInput.AttemptSkill)
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
            else if (combatState == CombatState.NonCombatState)
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
            onDeath?.Invoke(playerNo);
            combatState = CombatState.Dead;

            // die animation
            anim.SetBool("Dead", true);
        }
        
        private void ResetPlayer()
        {
            anim.SetBool("Dead", false);
            combatState = CombatState.NonCombatState;
            enabled = true;
            GetComponent<Collider2D>().enabled = true;
            GetComponent<Rigidbody2D>().simulated = true;
            gameObject.transform.position = originalPos;
        }
        
        private void PassPlayerSkills(int player, int assignSkill)
        {
            if (player != playerNo) return;
            switch (assignSkill)
            {
                case 1:
                    gameObject.AddComponent<ForcePush>();
                    skill = GetComponent<ForcePush>();
                    break;
                case 2:
                    gameObject.AddComponent<ForcePull>();
                    skill = GetComponent<ForcePull>();
                    break;
                case 3: 
                    gameObject.AddComponent<ThrowKnives>();
                    skill = GetComponent<ThrowKnives>();
                    break;
            }
            
        }

        private void OnDestroy()
        {
            ScoreKeeper.resetPlayersEvent -= ResetPlayer;
        }
    }
}