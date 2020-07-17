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
    
    private void Awake()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Master client");
            var player = PhotonNetwork.Instantiate("Player 1 Multiplayer Variant", Vector3.zero, Quaternion.identity);
            PlayerController playerController = player.GetComponent<PlayerController>();
            cameraGroup.AddMember(player.transform, 1, 0);
            playerController.healthBarController = healthBarControllerOne;
            playerController.cooldownUiController = cooldownUiControllerOne;
            playerController.playerNo = 1;
            playerController.playerInput.Toggle();
        }
        else
        {
            Debug.Log("Not Master client");
            var player = PhotonNetwork.Instantiate("Player 2 Multiplayer Variant", Vector3.zero, Quaternion.identity);
            PlayerController playerController = player.GetComponent<PlayerController>();
            playerController.playerInput.Toggle();
            playerController.playerNo = 2;
            cameraGroup.AddMember(player.transform, 1, 0);
            playerController.healthBarController = healthBarControllerTwo;
            playerController.cooldownUiController = cooldownUiControllerTwo;
        }


    }
}