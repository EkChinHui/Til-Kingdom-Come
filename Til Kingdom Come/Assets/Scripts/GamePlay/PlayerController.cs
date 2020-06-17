using System;
using GamePlay.Skills;
using UI;
using UI.GameUI;
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
        
        [Header("Particle effects")]
        public GameObject bloodSplash;
        public GameObject sparks;
        
        public enum CombatState { NonCombatState, Blocking, Rolling, Attacking, Skill, Dead}
        public CombatState combatState = CombatState.NonCombatState;

        [Header("Movement")] 
        private float runSpeed = 4f;
        private Vector2 originalPos;
        private Quaternion originalRotation;

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
        public Action onSuccessfulBlock;
        #endregion

        private void Awake()
        {
            // remember the original position of the players so match can be reset
            originalPos = gameObject.transform.position;
            originalRotation = gameObject.transform.rotation;
            totalPlayers++;
            playerNo = totalPlayers;
            ScoreKeeper.resetPlayersEvent += ResetPlayer;
            SkillSelectionManager.passPlayerSkills += PassPlayerSkills;
            SkillSelectionManager.instance.AssignSkills();
            onSuccessfulBlock += SuccessfulBlock;
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
            // combo system
            if (combatState == CombatState.Attacking)
            {
                if (playerInput.AttemptAttack)
                {
                    attack.Cast(this, otherPlayer);
                }
            }
            
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
            } 
            else if (playerInput.AttemptRoll)
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
            if (currentHealth <= 0 && combatState != CombatState.Dead)
            {
                Die();
            } 
            else
            {
                // hurt;
            }
        }
        private void SuccessfulBlock()
        {
            var heightOffset = new Vector3(0, 1.5f, 0);
            ParticleSystem sparksEffect = sparks.GetComponent<ParticleSystem>();
            // adjusts rotation of particle effect
            var shapeModule = sparksEffect.shape;
            shapeModule.rotation = Math.Abs(transform.localRotation.eulerAngles.y) < Mathf.Epsilon 
                ? new Vector3(0, 90, 0) 
                : new Vector3(0, -90, 0);
            
            Instantiate(sparks, transform.position + heightOffset, Quaternion.identity);
        }

        private void Die()
        {
            onDeath?.Invoke(playerNo);
            combatState = CombatState.Dead;
            // die animation
            anim.SetBool("Dead", true);
            
            // offset particles to be emitted at body level
            var heightOffset = new Vector3(0, 2f, 0);

            ParticleSystem bloodSplashEffect = bloodSplash.GetComponent<ParticleSystem>();
            // adjusts rotation of particle effect
            var shapeModule = bloodSplashEffect.shape;
            shapeModule.rotation = Math.Abs(transform.localRotation.eulerAngles.y) < Mathf.Epsilon 
                ? new Vector3(0, -90, 0) 
                : new Vector3(0, 90, 0);

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
            gameObject.transform.rotation = originalRotation;
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
            onSuccessfulBlock -= SuccessfulBlock;
        }
    }
}