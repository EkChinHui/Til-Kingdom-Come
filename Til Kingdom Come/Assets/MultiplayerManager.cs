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
        var player = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
        cameraGroup.AddMember(player.transform, 1, 0);
        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.playerInput.Toggle();
        playerController.healthBarController = healthBarControllerOne;
        playerController.cooldownUiController = cooldownUiControllerOne;
    }
}