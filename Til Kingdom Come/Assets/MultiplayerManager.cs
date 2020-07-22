using System;
using Cinemachine;
using GamePlay.Player;
using Photon.Pun;
using Photon.Realtime;
using UI.GameUI.Cooldown;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayerManager : MonoBehaviourPunCallbacks
{
    public CinemachineTargetGroup cameraGroup;
    public GameObject playerPrefab;
    public HealthBarController healthBarControllerOne;
    public HealthBarController healthBarControllerTwo;
    public CooldownUIController cooldownUiControllerOne;
    public CooldownUIController cooldownUiControllerTwo;
    
    public PlayerController playerOne;
    public PlayerController playerTwo;
    
    private void Awake()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Master client");
            var startOffset = new Vector3(-10, 0, 0);
            var player = PhotonNetwork.Instantiate("Player 1 Multiplayer Variant", Vector3.zero + startOffset, Quaternion.identity);
            player.name = "Player 1";
            PlayerController playerController = player.GetComponent<PlayerController>();
            playerController.playerNo = 1;
            cameraGroup.AddMember(player.transform, 1, 0);
            // Have not added dummy player to camera target group
            // cameraGroup.AddMember(playerController.otherPlayer.transform, 1, 0);
            playerController.healthBarController = healthBarControllerOne;
            playerController.cooldownUiController = cooldownUiControllerOne;
            playerOne = playerController;
            playerController.playerInput.DisableInput();
        }
        else
        {
            Debug.Log("Not Master client");
            var startOffset = new Vector3(10, 0, 0);
            var quaternion = new Quaternion();
            quaternion.eulerAngles = new Vector3(0, 180, 0);
            var player = PhotonNetwork.Instantiate("Player 2 Multiplayer Variant", Vector3.zero + startOffset, quaternion);
            player.name = "Player 2";
            PlayerController playerController = player.GetComponent<PlayerController>();
            playerController.playerNo = 2;
            playerController.playerInput.DisableInput();
            cameraGroup.AddMember(player.transform, 1, 0);
            // cameraGroup.AddMember(playerController.otherPlayer.transform, 1, 0);
            playerController.healthBarController = healthBarControllerTwo;
            playerController.cooldownUiController = cooldownUiControllerTwo;
            playerTwo = playerController;
        }
    }

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            playerOne.otherPlayer = GameObject.Find("Player 2 Multiplayer Variant(Clone)").GetComponent<PlayerController>();
        }
        else 
            playerTwo.otherPlayer = GameObject.Find("Player 1 Multiplayer Variant(Clone)").GetComponent<PlayerController>();
    }


    public void QuitRoom()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Multiplayer Lobby");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        QuitRoom();
    }
}