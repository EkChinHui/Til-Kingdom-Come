using Cinemachine;
using GamePlay.Player;
using Photon.Pun;
using UI.GameUI.Cooldown;
using UnityEngine;

public class MultiplayerManager : MonoBehaviour
{
    public CinemachineTargetGroup cameraGroup;
    public GameObject playerPrefab;
    public HealthBarController healthBarControllerOne;
    public HealthBarController healthBarControllerTwo;
    public CooldownUIController cooldownUiControllerOne;
    public CooldownUIController cooldownUiControllerTwo;
    
    public static PlayerController playerOne;
    public static PlayerController playerTwo;
    
    private void Awake()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Master client");
            var player = PhotonNetwork.Instantiate("Player 1 Multiplayer Variant", Vector3.zero, Quaternion.identity);
            player.name = "Player 1";
            PlayerController playerController = player.GetComponent<PlayerController>();
            playerController.playerNo = 1;
            cameraGroup.AddMember(player.transform, 1, 0);
            playerController.healthBarController = healthBarControllerOne;
            playerController.cooldownUiController = cooldownUiControllerOne;
            playerOne = playerController;
            playerController.playerInput.Toggle();
            playerController.otherPlayer = GameObject.Find("Player 2 Multiplayer Variant(clone)").GetComponent<PlayerController>();
        }
        else
        {
            Debug.Log("Not Master client");
            var player = PhotonNetwork.Instantiate("Player 2 Multiplayer Variant", Vector3.zero, Quaternion.identity);
            player.name = "Player 2";
            PlayerController playerController = player.GetComponent<PlayerController>();
            playerController.playerNo = 2;
            playerController.playerInput.Toggle();
            cameraGroup.AddMember(player.transform, 1, 0);
            playerController.healthBarController = healthBarControllerTwo;
            playerController.cooldownUiController = cooldownUiControllerTwo;
            playerTwo = playerController;
            playerController.otherPlayer = GameObject.Find("Player 1 Multiplayer Variant(clone)").GetComponent<PlayerController>();
        }
    }


}