using System;
using GamePlay.Information;
using GamePlay.Skills;
using UI.GameUI.Cooldown;
using UnityEngine;
using System.Collections;
using Photon.Pun;
using UnityEngine.SceneManagement;

namespace GamePlay.Player
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {
        #region Multiplayer

        public bool MultiplayerMode = false;
        public PhotonView photonView;

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

        #region MONOBEHAVIOUR CALLS

        private void Awake()
        {
            // Remember the original position of the players so match can be reset
            originalPosition = gameObject.transform.position;
            originalRotation = gameObject.transform.rotation;
            
            ScoreKeeper.resetPlayersEvent += ResetPlayer;
            SkillSelectionManager.passPlayerSkills += PassPlayerSkill;

            if (MultiplayerMode)
            {
                photonView = GetComponent<PhotonView>();
                if (PhotonNetwork.IsMasterClient)
                {
                    // setting for dummy player on Master client side
                    playerNo = 2;
                    healthBarController = GameObject.Find("Player 2 Health").GetComponent<HealthBarController>();
                    cooldownUiController = GameObject.Find("Player 2 Cooldown").GetComponent<CooldownUIController>();
                    // otherPlayer = GameObject.Find("Player 2").GetComponent<PlayerController>();
                }
                else
                {
                    playerNo = 1;
                    healthBarController = GameObject.Find("Player 1 Health").GetComponent<HealthBarController>();
                    cooldownUiController = GameObject.Find("Player 1 Cooldown").GetComponent<CooldownUIController>();
                    // otherPlayer = GameObject.Find("Player 1").GetComponent<PlayerController>();
                }
            }
            else
            {
                totalPlayers++;
                playerNo = totalPlayers;
            }
            onSuccessfulBlock += SuccessfulBlock;
        }
        private void Start()
        {
            /*if (PhotonNetwork.IsMasterClient)
                otherPlayer = GameObject.Find("Player 2").GetComponent<PlayerController>();
            else 
                otherPlayer = GameObject.Find("Player 1").GetComponent<PlayerController>();*/
            // Instantiate variables on creation
            sprite = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            currentHealth = maxHealth;
            healthBarController.SetHealth(currentHealth);
            SkillSelectionManager.instance.AssignSkills(playerNo);
        }
        public void Update()
        {
            if (MultiplayerMode)
            {
                if (!photonView.IsMine) return;
            }

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
        
        private void OnDestroy()
        {
            ScoreKeeper.resetPlayersEvent -= ResetPlayer;
            SkillSelectionManager.passPlayerSkills -= PassPlayerSkill;
            onSuccessfulBlock -= SuccessfulBlock;
        }

        #endregion

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

        #region RPC METHODS
        
        [PunRPC]
        private void RPCRoll()
        {
            roll.Cast(otherPlayer);
        }
        
        [PunRPC]
        private void RPCAttack()
        {
            attack.Cast(otherPlayer);
        }
        
        [PunRPC]
        private void RPCBlock()
        {
            block.Cast(otherPlayer);
        }
        
        [PunRPC]
        private void RPCSkill()
        {
            skill.Cast(otherPlayer);
        }
        

        #endregion

        #region LISTEN FOR

        private void ListenForRoll()
        {
            if (playerInput.AttemptRoll)
            {
                if (MultiplayerMode)
                    photonView.RPC("RPCRoll", RpcTarget.All);
                else 
                    roll.Cast(otherPlayer);
            }
        }

        private void ListenForAttack()
        {
            if (playerInput.AttemptAttack)
            {
                if (MultiplayerMode) 
                    photonView.RPC("RPCAttack", RpcTarget.All);
                else 
                    attack.Cast(otherPlayer);
                
            }
        }

        private void ListenForBlock()
        {
            if (playerInput.AttemptBlock)
            {
                if (MultiplayerMode) 
                    photonView.RPC("RPCBlock", RpcTarget.All);
                else 
                    block.Cast(otherPlayer);
            }
        }
        
        private void ListenForSkill()
        {
            if (playerInput.AttemptSkill)
            {
                if (MultiplayerMode) 
                    photonView.RPC("RPCSkill", RpcTarget.All);
                else 
                    skill.Cast(otherPlayer);
            }
        }

        #endregion
       

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
        
        public void TakeDamage(float damageTaken)
        {
            if (!godMode)
            {
                Debug.Log("Player " + playerNo + " takes " + damageTaken + " damage.");
                currentHealth -= damageTaken;
                healthBarController.SetHealth(currentHealth);
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

        private IEnumerator Stun(float duration)
        {
            playerInput.Toggle();
            yield return new WaitForSeconds(duration);
            playerInput.Toggle();
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
            if (PlayerInput.onToggleInput != null) PlayerInput.onToggleInput();

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
            anim.SetInteger("state", 0);

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
            this.skill.AssignPlayer(this);
            var skillCharges = this.skill.GetComponent<Charges>();
            if (skillCharges != null)
            {
                cooldownUiController.skillIcon.GetComponent<DisplayCharges>().charges = skillCharges.CurrentCharge;
            }
        }

    }
}