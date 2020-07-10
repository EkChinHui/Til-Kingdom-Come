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

    #region Rewards

    private float enemyDeath = 5f;
    private float successfulAttack = 0.8f;
    private float missedAttack = -1f;
    private float movement = 0.1f;
    private float overTime = -7f; // per episode
    private float takeDamage = -2f;

    #endregion
    
    public event Action OnEnvironmentReset;

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(enemyController.transform.position.x - playerController.transform.position.x); // 1
        // sensor.AddObservation(enemyController.transform.rotation.y); // 1
        sensor.AddObservation(playerController.transform.rotation.y); // 1
    }

    private void FixedUpdate()
    {
        if (playerController.combatState == PlayerController.CombatState.Dead)
        {
            EndEpisode();
        }

        if (enemyController.combatState == PlayerController.CombatState.Dead)
        {
            AddReward(5f);
            Debug.Log("player killed");
            EndEpisode();
        }
        AddReward(overTime/MaxStep);
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
            playerController.RollTrigger();
        }

        if (vectorAction[1] >= 1 && vectorAction[2] >= 1)
        {
        }
        else if (vectorAction[2] >= 1) // move right
        {
            AddReward(movement);
            playerController.MoveRight();
        }
        else if (vectorAction[1] >= 1) // move left
        {
            AddReward(movement);
            playerController.MoveLeft();
        }
        

        /*Rb.velocity = new Vector3(vectorAction[1] * speed, 0f, vectorAction[2] * speed);
        transform.Rotate(Vector3.up, vectorAction[3] * rotationSpeed);*/
    }

    public override void Heuristic(float[] actionsOut)
    {
        // actionsOut[0] = Input.GetKey(KeyCode.P) ? 1f : 0f;
        actionsOut[0] = Input.GetKey(KeyCode.F) ? 1f : 0f; // attack
        actionsOut[1] = Input.GetKey(KeyCode.A) ? 1f : 0f; // move left
        actionsOut[2] = Input.GetKey(KeyCode.D) ? 1f : 0f; // move right
        actionsOut[3] = Input.GetKey(KeyCode.S) ? 1f : 0f; // roll
    }
    
    public override void Initialize()
    {
        //StartingPosition = transform.position;
        playerController = gameObject.GetComponent<PlayerController>();
        // PlayerController.onDeath += EndEpisodeHelper;
        OnEnvironmentReset += ResetPlayers;
        PlayerController.onDamageTaken += TakeDamage;
        PlayerController.onSuccessfulAttack += SuccessfulAttack;
        PlayerController.onMissedAttack += MissAttack;
        PlayerController.onSuccessfulDodge += SuccessfulDodge;
        /*Rb = GetComponent<Rigidbody>();
        
        //TODO: Delete
        Rb.freezeRotation = true;*/
        //EnvironmentParameters = Academy.Instance.EnvironmentParameters;
    }

    private void MissAttack(int no)
    {
        if (playerController.playerNo == no)
        {
            AddReward(missedAttack);
        }
    }

    private void SuccessfulAttack(int no)
    {
        if (playerController.playerNo == no)
        {
            AddReward(successfulAttack);
        }
    }

    private void TakeDamage(int no)
    {
        if (playerController.playerNo == no)
        {
            AddReward(takeDamage);
        }
    }

    private void SuccessfulDodge(int no)
    {
        if (playerController.playerNo == no)
        {
            AddReward(takeDamage);
        }
    }


    public override void OnEpisodeBegin()
    {
        OnEnvironmentReset?.Invoke();

        //Load Parameter from Curciulum
        //minStepsBetweenShots = Mathf.FloorToInt(EnvironmentParameters.GetWithDefault("shootingFrequenzy", 30f));
        enemyController.transform.position = new Vector2(Random.Range(-10f, 10f) + enemyController.transform.position.x, enemyController.transform.position.y);
        //playerController.transform.position = new Vector2(Random.Range(-20f, 12f), playerController.transform.position.y);
    }

    private void ResetPlayers()
    {
        playerController.ResetPlayer();
        enemyController.ResetPlayer();
    }

}
