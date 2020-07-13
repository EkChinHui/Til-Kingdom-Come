using System;
using GamePlay.Player;
using GamePlay.Skills;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;
using Random = UnityEngine.Random;


public class PlayerAgent : Agent
{
    private PlayerController playerController;
    public PlayerController enemyController;
    private Vector3 startingPosition;

    #region Rewards

    private float enemyDeath = 3f;
    private float playerDeath = -3f;
    private float movement = 0.01f;
    
    
    /*private float successfulAttack = 0f;//2f;
    private float missedAttack = 0f;// -0.5f;
    private float overTime = 0f; //-5f; // per episode
    private float takeDamage = 0f;
    private float successfulDodge = 0f;*/

    #endregion
    
    public event Action OnEnvironmentReset;

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(Math.Abs(transform.position.x - playerController.transform.position.x)); // 1
        sensor.AddObservation(enemyController.transform.rotation.y); // 1
        sensor.AddObservation(playerController.transform.rotation.y); // 1
        // sensor.AddObservation(playerController.currentHealth);
        // sensor.AddObservation(enemyController.currentHealth);
    }

    private void FixedUpdate()
    {
        if (playerController.combatState == PlayerController.CombatState.Dead)
        {
            Debug.Log("Lose by death");
            AddReward(playerDeath);
            EndEpisode();
        }

        if (enemyController.combatState == PlayerController.CombatState.Dead)
        {
            Debug.Log("Win by death");
            AddReward(enemyDeath);
            EndEpisode();
        }
        
        if (StepCount >= MaxStep - 1)
        {
            var playerHealth = playerController.currentHealth;
            var enemyHealth = enemyController.currentHealth;
            if (playerHealth > enemyHealth)
            {
                Debug.Log("Win by overtime");
                AddReward(1f);
            }
            else if(playerHealth < enemyHealth)
            {
                Debug.Log("Lose by overtime");
                AddReward(-1f);
            }
            else
            {
                Debug.Log("Draw by overtime");
                AddReward(0f);
            }
        }
    }
    public override void OnActionReceived(float[] vectorAction)
    {
        if (!playerController.playerInput.inputIsEnabled) return;

        if (Mathf.RoundToInt(vectorAction[0]) >= 1)
        {
            if (playerController.attack.charges.CurrentCharge > 0)
            {
                playerController.AttackTrigger();
            }
        }
        
        if (Mathf.RoundToInt(vectorAction[3]) >= 1)
        {
            Debug.Log("Rolling");
            playerController.RollTrigger();
        }

        if (vectorAction[1] >= 1 && vectorAction[2] >= 1)
        {
        }
        else if (vectorAction[1] >= 1) // move left
        {
            AddReward(movement);
            Debug.Log("moving left");
            playerController.MoveLeft();
        }        
        else if (vectorAction[2] >= 1) // move right
        {
            AddReward(movement);
            Debug.Log("moving right");
            playerController.MoveRight();
        }
    }

    public override void Heuristic(float[] actionsOut)
    {
        Array.Clear(actionsOut, 0, actionsOut.Length);
        actionsOut[0] = Input.GetKeyDown(playerController.playerInput.attackKey) ? 1f : 0f; // attack
        actionsOut[1] = Input.GetKey(playerController.playerInput.leftKey) ? 1f : 0f; // move left
        actionsOut[2] = Input.GetKey(playerController.playerInput.rightKey) ? 1f : 0f; // move right
        actionsOut[3] = Input.GetKeyDown(playerController.playerInput.rollKey) ? 1f : 0f; // roll
    }
    
    public override void Initialize()
    {
        startingPosition = transform.position;
        playerController = gameObject.GetComponent<PlayerController>();
        OnEnvironmentReset += ResetPlayers;
        PlayerController.onDamageTaken += TakeDamage;
        PlayerController.onSuccessfulAttack += SuccessfulAttack;
        PlayerController.onMissedAttack += MissAttack;
        PlayerController.onSuccessfulDodge += SuccessfulDodge;

    }

    private void MissAttack(int no)
    {
        if (playerController.playerNo == no)
        {
            Debug.Log("Missed attack");
            //AddReward(missedAttack);
        }
    }

    private void SuccessfulAttack(int no)
    {
        if (playerController.playerNo == no)
        {
            Debug.Log("Successful attack");
            //AddReward(successfulAttack);
        }
    }

    private void TakeDamage(int no)
    {
        if (playerController.playerNo == no)
        {
            Debug.Log("Take damage");
            //AddReward(takeDamage);
        }
    }

    private void SuccessfulDodge(int no)
    {
        if (playerController.playerNo == no)
        {
            Debug.Log("Successful Dodge");
            //AddReward(successfulDodge);
        }
    }


    public override void OnEpisodeBegin()
    {
        OnEnvironmentReset?.Invoke();

        enemyController.transform.position = new Vector2(Random.Range(-10f, 10f) + enemyController.transform.position.x, enemyController.transform.position.y);
        playerController.transform.position = new Vector2(playerController.transform.position.x + Random.Range(-10f, 10f), playerController.transform.position.y);
        var rotation = new Quaternion
        {
            eulerAngles = Random.Range(0f, 1f) > 0.5 ? new Vector3(0, 180, 0) : new Vector3(0, 0, 0)
        };
        var rotation1 = new Quaternion
        {
            eulerAngles = Random.Range(0f, 1f) > 0.5 ? new Vector3(0, 180, 0) : new Vector3(0, 0, 0)
        };
        playerController.transform.rotation = rotation;
        enemyController.transform.rotation = rotation1;
    }

    private void ResetPlayers()
    {
        playerController.ResetPlayer();
        enemyController.ResetPlayer();
    }

}
