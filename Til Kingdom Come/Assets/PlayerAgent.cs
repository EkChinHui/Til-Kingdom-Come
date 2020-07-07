using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay.Player;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class PlayerAgent : Agent
{
    private PlayerController playerController;
    public PlayerController enemyController;

    private Vector3 StartingPosition;
    private EnvironmentParameters EnvironmentParameters;

    public event Action OnEnvironmentReset;

    private void EndEpisodeHelper(int i)
    {
        EndEpisode();
    }
    

    public override void CollectObservations(VectorSensor sensor)
    {
        var distance = enemyController.transform.position.x - playerController.transform.position.x;
        sensor.AddObservation(distance);
        //sensor.AddObservation(ShotAvaliable);
        //Add Angle Y
    }

    private void FixedUpdate()
    {
        
    }
    public override void OnActionReceived(float[] vectorAction)
    {
        if (Mathf.RoundToInt(vectorAction[0]) >= 1)
        {
            playerController.attack.Cast(enemyController);
        }

        /*Rb.velocity = new Vector3(vectorAction[1] * speed, 0f, vectorAction[2] * speed);
        transform.Rotate(Vector3.up, vectorAction[3] * rotationSpeed);*/
    }
    
    public override void Initialize()
    {
        StartingPosition = transform.position;
        playerController = gameObject.GetComponent<PlayerController>();
        PlayerController.onDeath += EndEpisodeHelper;
        /*Rb = GetComponent<Rigidbody>();
        
        //TODO: Delete
        Rb.freezeRotation = true;*/
        //EnvironmentParameters = Academy.Instance.EnvironmentParameters;
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
        
        transform.position = StartingPosition;
        
    }

}
