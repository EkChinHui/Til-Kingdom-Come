﻿using UnityEngine;
using System;
using GamePlay.Information;
using GamePlay.Skills;
using UI.GameUI;
using UI.GameUI.Cooldown;
using System.Collections;
using GamePlay.Player;
using Photon.Pun;


namespace GamePlay.Multiplayer
{
    public class MultiplayerController : MonoBehaviourPun
    {
        #region multiplayer variables
        #endregion
        
        public static int totalPlayers;
        private SpriteRenderer sprite;
        private float runSpeed = 4f;
        private Vector2 originalPosition;
        private Quaternion originalRotation;
        
        #region States
        public enum CombatState { NonCombat, Blocking, Rolling, Attacking, Hurt, Skill, Dead}
        public bool godMode = false;
        public CombatState combatState = CombatState.NonCombat;
        #endregion

        #region Health Properties
        private const float maxHealth = 100;
        public float currentHealth;
        #endregion

        #region Hurt Properties
        private float hurtDuration = 0.4f;
        private float hurtInterval = 0.2f;
        private float hurtDistance = 8f;
        private float stunDuration = 0.2f;
        #endregion

        #region Events
        public static Action<int> onDeath;
        public Action onSuccessfulBlock;
        #endregion

        [Header("Fields")]
        public Rigidbody2D rb;
        public Animator anim;
        public PlayerController otherPlayer;
        public int playerNo;
        public PlayerInput playerInput;
        
        [Header("Particle effects")]
        public GameObject bloodSplash;
        public GameObject sparks;
        public GameObject confusion;
        
        [Header("Health")]
        public HealthBarController healthBarController;

        [Header("Skills")] 
        public Attack attack;
        public Block block;
        public Roll roll;
        public Skill skill;

        [Header("UI")] 
        public CooldownUIController cooldownUiController;

        private void Awake()
        {
            // Remember the original position of the players so match can be reset
            originalPosition = gameObject.transform.position;
            originalRotation = gameObject.transform.rotation;
            totalPlayers++;
            playerNo = totalPlayers;
            ScoreKeeper.resetPlayersEvent += ResetPlayer;
            SkillSelectionManager.passPlayerSkills += PassPlayerSkill;

            onSuccessfulBlock += SuccessfulBlock;
        }
        private void Start()
        {
            // Instantiate variables on creation
            sprite = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            currentHealth = maxHealth;
            SkillSelectionManager.instance.AssignSkills(playerNo);
        }
        public void Update()
        {
            healthBarController.SetHealth(currentHealth);
            // combo system
            if (combatState == CombatState.Attacking)
            {
                if (playerInput.AttemptAttack)
                {
                    attack.Cast(otherPlayer);
                }
            }

            // if the player is dead, the player state should not be updated
            if (combatState == CombatState.Dead) return;
            // the player can only move, block and roll while hurt
            if (combatState == CombatState.Hurt)
            {
                ListenForRoll();
                ListenForBlock();
                Move();
            }
            else if (combatState == CombatState.NonCombat)
            {
                ListenForRoll();
                ListenForAttack();
                ListenForBlock();
                ListenForSkill();
                Move();
            }
        }
        private void Move()
        {
            if (playerInput.AttemptRight && playerInput.AttemptLeft)
            {
                anim.SetInteger("state", 0);
            }
            else if (playerInput.AttemptRight)
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
        private void ListenForRoll()
        {
            if (playerInput.AttemptRoll)
            {
                roll.Cast(otherPlayer);
            }
        }
        private void ListenForAttack()
        {
            if (playerInput.AttemptAttack)
            {
                attack.Cast(otherPlayer);
            }
        }
        private void ListenForBlock()
        {
            if (playerInput.AttemptBlock)
            {
                block.Cast(otherPlayer);
            }
        }
        private void ListenForSkill()
        {
            if (playerInput.AttemptSkill)
            {
                skill.Cast(otherPlayer);
            }
        }

        private IEnumerator Hurt()
        {
            combatState = CombatState.Hurt;
            // enable god mode
            godMode = true;
            // start hurt animation
            var animationRoutine = StartCoroutine(ChangeSpriteColorAndWait(hurtInterval));
            // stun player
            StartCoroutine(Stun(stunDuration));
            // knock player up
            HurtKnockUp(hurtDistance);
            yield return new WaitForSeconds(hurtDuration);
            // stop hurt animation
            StopCoroutine(animationRoutine);
            sprite.color = Color.white;
            // disable god mode
            godMode = false;
            combatState = CombatState.NonCombat;
        }

        private IEnumerator Stun(float duration)
        {
            playerInput.DisableInput();
            yield return new WaitForSeconds(duration);
            playerInput.EnableInput();
            Debug.Log("Player " + playerNo + " enable input after stun");
        }

        private IEnumerator ChangeSpriteColorAndWait(float interval)
        {
            while (true)
            {
                sprite.color = Color.red;
                yield return new WaitForSeconds(interval);
                sprite.color = Color.white;
                yield return new WaitForSeconds(interval);
            }
        }

        private void HurtKnockUp(float distance)
        {
            var velocity = rb.velocity;
            rb.velocity = new Vector2(velocity.x, distance);
        }

        public void KnockBack(float distance)
        {
            var velocity = rb.velocity;
            velocity = Math.Abs(transform.localRotation.eulerAngles.y - 180) < Mathf.Epsilon
                ? new Vector2(distance, velocity.y)
                : new Vector2(-distance, velocity.y);
            rb.velocity = velocity;
        }

        public void TakeDamage(float damageTaken)
        {
            if (!godMode)
            {
                Debug.Log("Player " + playerNo + " takes " + damageTaken + " damage.");
                currentHealth -= damageTaken;
                if (currentHealth <= 0 && combatState != CombatState.Dead)
                {
                    Die();
                } 
                else
                {
                    // Hurt;
                    StartCoroutine(Hurt());
                }
            }
        }
        
        private void SuccessfulBlock()
        {
            var heightOffset = new Vector3(0, 1.5f, 0);
            ParticleSystem sparksEffect = sparks.GetComponent<ParticleSystem>();
            // Adjust rotation of particle effect
            var shapeModule = sparksEffect.shape;
            shapeModule.rotation = Math.Abs(transform.localRotation.eulerAngles.y) < Mathf.Epsilon 
                ? new Vector3(0, 90, 0) 
                : new Vector3(0, -90, 0);
            
            Instantiate(sparks, transform.position + heightOffset, Quaternion.identity);
        }

        private void Die()
        {
            Debug.Log("Player " + playerNo + " dies.");
            Debug.Log("Player " + otherPlayer.playerNo + " enters God mode.");
            onDeath?.Invoke(playerNo);
            otherPlayer.godMode = true;
            combatState = CombatState.Dead;

            // Disable input
            if (PlayerInput.onDisableInput != null) PlayerInput.onDisableInput();

            // Die animation
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
        // Reset player position based on start position and resets combatState
        {
            attack.charges.RefillCharges();
            block.charges.RefillCharges();
            godMode = false;
            anim.SetBool("Dead", false);
            anim.SetInteger("State", 0);
            currentHealth = maxHealth;
            rb.velocity = Vector2.zero;
            combatState = CombatState.NonCombat;
            enabled = true;
            GetComponent<Collider2D>().enabled = true;
            GetComponent<Rigidbody2D>().simulated = true;
            gameObject.transform.position = originalPosition;
            gameObject.transform.rotation = originalRotation;
        }
        
        private void PassPlayerSkill(int player, GameObject skill)
        // Assign player skills based on skill selection menu
        {
            if (player != playerNo) return;
            var skillObject = Instantiate(skill).GetComponent<Skill>();
            this.skill = skillObject;
            skillObject.transform.parent = transform;
            // this.skill.AssignPlayer(this);
            var skillCharges = this.skill.GetComponent<Charges>();
            if (skillCharges != null)
            {
                cooldownUiController.skillIcon.GetComponent<DisplayCharges>().charges = skillCharges.CurrentCharge;
            }
        }

        private void OnDestroy()
        {
            ScoreKeeper.resetPlayersEvent -= ResetPlayer;
            SkillSelectionManager.passPlayerSkills -= PassPlayerSkill;
            onSuccessfulBlock -= SuccessfulBlock;
        }
    }
}
