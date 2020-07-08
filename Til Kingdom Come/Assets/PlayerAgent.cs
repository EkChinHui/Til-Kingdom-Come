﻿using System;
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

    //private Vector3 StartingPosition;
    //private EnvironmentParameters EnvironmentParameters;

    public event Action OnEnvironmentReset;

    public override void CollectObservations(VectorSensor sensor)
    {
        //sensor.AddObservation(enemyController.transform.position - playerController.transform.position);
        //sensor.AddObservation(ShotAvaliable);
        //Add Angle Y
    }

    private void FixedUpdate()
    {
        if (playerController.combatState == PlayerController.CombatState.Dead)
        {
            EndEpisode();
        }

        if (enemyController.combatState == PlayerController.CombatState.Dead)
        {
            AddReward(1f);
            Debug.Log("player killed");
            EndEpisode();
        }
        AddReward(-1f/MaxStep);
        
    }
    public override void OnActionReceived(float[] vectorAction)
    {
        var enemyHealth = enemyController.currentHealth;
        if (Mathf.RoundToInt(vectorAction[0]) >= 1)
        {
            if (playerController.attack.charges.CurrentCharge > 0 && playerController.playerInput.inputIsEnabled)
            {
                playerController.attack.Cast(enemyController);
            }
            
        }

        /*Rb.velocity = new Vector3(vectorAction[1] * speed, 0f, vectorAction[2] * speed);
        transform.Rotate(Vector3.up, vectorAction[3] * rotationSpeed);*/
    }

    public override void Initialize()
    {
        //StartingPosition = transform.position;
        playerController = gameObject.GetComponent<PlayerController>();
        // PlayerController.onDeath += EndEpisodeHelper;
        OnEnvironmentReset += ResetPlayers;
        Attack.onSuccessfulAttack += SuccessfulAttack;
        Attack.onMissedAttack += MissAttack;
        /*Rb = GetComponent<Rigidbody>();
        
        //TODO: Delete
        Rb.freezeRotation = true;*/
        //EnvironmentParameters = Academy.Instance.EnvironmentParameters;
    }

    private void MissAttack()
    {
        AddReward(-0.5f);
    }

    private void SuccessfulAttack()
    {
        AddReward(1f);
    }
    public override void Heuristic(float[] actionsOut)
    {
        // actionsOut[0] = Input.GetKey(KeyCode.P) ? 1f : 0f;
        actionsOut[0] = Input.GetKey(KeyCode.F) ? 1f : 0f;
    }

    public override void OnEpisodeBegin()
    {
        OnEnvironmentReset?.Invoke();

        //Load Parameter from Curciulum
        //minStepsBetweenShots = Mathf.FloorToInt(EnvironmentParameters.GetWithDefault("shootingFrequenzy", 30f));
        //enemyController.transform.position = new Vector2(Random.Range(-20f, 12f), enemyController.transform.position.y);
        //playerController.transform.position = new Vector2(Random.Range(-20f, 12f), playerController.transform.position.y);
    }

    private void ResetPlayers()
    {
        playerController.ResetPlayer();
        enemyController.ResetPlayer();
    }

}
