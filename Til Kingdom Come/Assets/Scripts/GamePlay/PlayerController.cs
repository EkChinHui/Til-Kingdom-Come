using System;
using GamePlay.Skills;
using UI;
using Unity.Mathematics;
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
        public GameObject bloodSplash;
        
        public enum CombatState { NonCombatState, Blocking, Rolling, Attacking, Skill, Dead}
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

        [Header("UI")] 
        public CooldownUiController cooldownUiController;

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
            SkillSelectionManager.passPlayerSkills += PassPlayerSkills;
            SkillSelectionManager.instance.AssignSkills();
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
            // if the player is dead, the player state should not be updated
            if (combatState == CombatState.Dead) return;
            // the player should only be able to perform other actions in the NonCombatState
            if (combatState != CombatState.NonCombatState) return;
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
        }

        private void Move()
        {
            if (playerInput.AttemptRight)
            {
                anim.SetInteger("state", 1);
                rb.velocity = new Vector2(runSpeed, rb.velocity.y);
                if (Math.Abs(transform.rotation.y) > Mathf.Epsilon)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
            else if (playerInput.AttemptLeft)
            {
                anim.SetInteger("state", 1);
                rb.velocity = new Vector2(-1 * runSpeed, rb.velocity.y);
                if (Math.Abs(transform.rotation.y) < Mathf.Epsilon)
                {
                    transform.rotation = Quaternion.Euler(0, 180f, 0);
                }
            }
            else
            {
                anim.SetInteger("state", 0);
            }
        }
        

        public void KnockBack(float distance)
        {
            var velocity = rb.velocity;
            velocity = Math.Abs(transform.localRotation.eulerAngles.y - 180) < Mathf.Epsilon
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
            // offset particles to be emitted at body level
            var heightOffset = new Vector3(0, 2f, 0);
            var startRotation = 0f;

            ParticleSystem bloodSplashEffect = bloodSplash.GetComponent<ParticleSystem>();
            var shapeModule = bloodSplashEffect.shape;
            if (Math.Abs(transform.localRotation.eulerAngles.y) < Mathf.Epsilon)
            {
                shapeModule.rotation = new Vector3(0, -90, 0);
            }
            else
            {
                shapeModule.rotation = new Vector3(0, 90, 0);
            }

            Instantiate(bloodSplash, transform.position + heightOffset, Quaternion.identity);
        }
        
        private void ResetPlayer()
        // resets player position based on start position and resets combatState
        {
            anim.SetBool("Dead", false);
            combatState = CombatState.NonCombatState;
            enabled = true;
            GetComponent<Collider2D>().enabled = true;
            GetComponent<Rigidbody2D>().simulated = true;
            gameObject.transform.position = originalPos;
        }
        
        private void PassPlayerSkills(int player, GameObject assignSkill)
        // assign player skills based on skill selection menu
        {
            if (player != playerNo) return;
            skill = Instantiate(assignSkill).GetComponent<Skill>();
        }

        private void OnDestroy()
        {
            ScoreKeeper.resetPlayersEvent -= ResetPlayer;
            SkillSelectionManager.passPlayerSkills -= PassPlayerSkills;
        }
    }
}