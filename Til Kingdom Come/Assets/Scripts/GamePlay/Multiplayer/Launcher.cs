using System;
using System.Collections.Generic;
using GamePlay.Multiplayer.Lobby;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using UI.GameUI.Player_Score;
using UI.Map;

namespace GamePlay.Multiplayer
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        private PhotonView photonView;
        
        [Header("Login Panel")]
        public GameObject LoginPanel;

        // public InputField PlayerNameInput;
        public TextMeshProUGUI PlayerNameInput;

        [Header("Selection Panel")]
        public GameObject SelectionPanel;

        [Header("Create Room Panel")]
        public GameObject CreateRoomPanel;
        public TextMeshProUGUI RoomNameInputField;
        public UpdateWins UpdateWins;
        public MapChanger MapChanger;

        [Header("Lobby Panel")]
        public GameObject RoomListPanel;
        public GameObject RoomListContent;
        public GameObject RoomListEntryPrefab;

        [Header("Room Panel")]
        public GameObject RoomLobbyPanel;
        public Transform LobbyHorizontalLayoutGroup;
        public GameObject PlayerEntryPrefab;
        public GameObject startButton;
        public Button button;

        [Header("Skill Select Panel")] 
        public GameObject SkillSelectPanel;
        

        private Dictionary<string, RoomInfo> cachedRoomList;
        private Dictionary<string, GameObject> roomListEntries;
        private Dictionary<int, GameObject> playerListEntries;
        


        #region Private Fields

        private bool isConnecting;
        /// <summary>
        /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
        /// </summary>
        string gameVersion = "1";


        #endregion


        #region MonoBehaviour CallBacks


        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
        /// </summary>
        void Awake()
        {
            // #Critical
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;
            button = startButton.GetComponent<Button>();
            
            cachedRoomList = new Dictionary<string, RoomInfo>();
            roomListEntries = new Dictionary<string, GameObject>();
            photonView = GetComponent<PhotonView>();
        }


        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase.
        /// </summary>
        void Start()
        {
            PlayerNameInput.text = PlayerPrefs.GetString("Nickname", "");
        }

        private void Update()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
                {
                    button.interactable = true;
                }
                else
                {
                    button.interactable = false;
                }
            }
        }

        #endregion

        


        #region UI CALLBACKS

        public void OnRoomLobbyBackButtonClicked()
        {
            PhotonNetwork.LeaveRoom();
        }

        public void OnNicknamePanelBackButtonClicked()
        {
            SceneManager.LoadScene("Main Menu");
        }

        public void OnSelectionPanelBackButtonClicked()
        {
            PhotonNetwork.Disconnect();
        }
        
        public void OnCreateRoomButtonClicked()
        {
            string roomName = PlayerPrefs.GetString("Nickname", "") + "'s room";
            var roomOptions = new RoomOptions {MaxPlayers = 2};
            PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
        }

        #endregion

        public void SetActivePanel(string activePanel)
        {
            LoginPanel.SetActive(activePanel.Equals(LoginPanel.name));
            SelectionPanel.SetActive(activePanel.Equals(SelectionPanel.name));
            CreateRoomPanel.SetActive(activePanel.Equals(CreateRoomPanel.name));
            RoomListPanel.SetActive(activePanel.Equals(RoomListPanel.name));    // UI should call OnRoomListButtonClicked() to activate this
            RoomLobbyPanel.SetActive(activePanel.Equals(RoomLobbyPanel.name));
            SkillSelectPanel.SetActive(activePanel.Equals(SkillSelectPanel.name));
        }
        
        public void OnRoomListButtonClicked()
        {
            if (!PhotonNetwork.InLobby)
            {
                PhotonNetwork.JoinLobby();
            }

            SetActivePanel(RoomListPanel.name);
        }
        
        public void OnLoginButtonClicked()
        {
            string playerName = PlayerNameInput.text;
            PlayerPrefs.SetString("Nickname", playerName);
            
            if (!playerName.Equals(""))
            {
                PhotonNetwork.LocalPlayer.NickName = playerName;
                PhotonNetwork.ConnectUsingSettings();
            }
            else
            {
                Debug.LogError("Player Name is invalid.");
            }
        }
        
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 2;
            PhotonNetwork.CreateRoom("testRoom", roomOptions, TypedLobby.Default);
        }

        public void OnStartButtonClicked()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.Log("PhotonNetwork: Trying to load a level but we are not the master client");
                return;
            }
            UpdateWins.photonView.RPC("MultiplayerPassWins", RpcTarget.All, MapChanger.current + 1, UpdateWins.wins);
            photonView.RPC("SkillSelectRPC", RpcTarget.All);
            //PhotonNetwork.LoadLevel("Skill Selection Multiplayer");
        }

        [PunRPC]
        private void SkillSelectRPC()
        {
            SetActivePanel(SkillSelectPanel.name);
        }

        public void OnSkillSelectStartButtonClicked()
        {
            PhotonNetwork.LoadLevel("MultiplayerArena");
        }


        #region PUN CALLBACKS

        public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
            {
                startButton.gameObject.SetActive(true);
            }
        }
        
        public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
        {
            Debug.Log("Player " + otherPlayer.ActorNumber + " has left the room");
            Destroy(playerListEntries[otherPlayer.ActorNumber].gameObject);
            playerListEntries.Remove(otherPlayer.ActorNumber);
        }

        public override void OnLeftRoom()
        {
            SetActivePanel(SelectionPanel.name);

            foreach (GameObject entry in playerListEntries.Values)
            {
                Destroy(entry.gameObject);
            }

            playerListEntries.Clear();
            playerListEntries = null;
        }
        
        public override void OnConnectedToMaster()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
            // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
            /*if (isConnecting)
            {
                PhotonNetwork.JoinRandomRoom();
                isConnecting = false;
            }*/
            SetActivePanel(SelectionPanel.name);

        }


        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
            SetActivePanel(LoginPanel.name);
        }
        
        public override void OnJoinedRoom()
        {
            if (playerListEntries == null)
            {
                playerListEntries = new Dictionary<int, GameObject>();
            }

            foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
            {
                GameObject entry = Instantiate(PlayerEntryPrefab);
                entry.transform.SetParent(LobbyHorizontalLayoutGroup);
                entry.transform.localScale = Vector3.one;
                entry.GetComponent<PlayerEntry>().SetName(player.NickName);
                playerListEntries.Add(player.ActorNumber, entry);
            }
            Debug.Log("Joined room");
            SetActivePanel(RoomLobbyPanel.name);
            if (!PhotonNetwork.IsMasterClient)
            {
                startButton.SetActive(false);
            }
        }
        
        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            GameObject entry = Instantiate(PlayerEntryPrefab);
            entry.transform.SetParent(LobbyHorizontalLayoutGroup.transform);
            entry.transform.localScale = Vector3.one;
            entry.GetComponent<PlayerEntry>().SetName(newPlayer.NickName);
            playerListEntries.Add(newPlayer.ActorNumber, entry);
        }
        
        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            ClearRoomListView();

            UpdateCachedRoomList(roomList);
            UpdateRoomListView();
        }


        public override void OnCreatedRoom()
        {
            Debug.Log("Room created");
        }

        #endregion
        
        private void ClearRoomListView()
        {
            foreach (GameObject entry in roomListEntries.Values)
            {
                Destroy(entry.gameObject);
            }

            roomListEntries.Clear();
        }
        
        private void UpdateCachedRoomList(List<RoomInfo> roomList)
        {
            foreach (RoomInfo info in roomList)
            {
                // Remove room from cached room list if it got closed, became invisible or was marked as removed
                if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
                {
                    if (cachedRoomList.ContainsKey(info.Name))
                    {
                        cachedRoomList.Remove(info.Name);
                    }

                    continue;
                }

                // Update cached room info
                if (cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList[info.Name] = info;
                }
                // Add new room info to cache
                else
                {
                    cachedRoomList.Add(info.Name, info);
                }
            }
        }

        private void UpdateRoomListView()
        {
            foreach (RoomInfo info in cachedRoomList.Values)
            {
                GameObject entry = Instantiate(RoomListEntryPrefab);
                entry.transform.SetParent(RoomListContent.transform);
                entry.transform.localScale = Vector3.one;
                entry.GetComponent<RoomListEntry>().Initialize(info.Name, (byte)info.PlayerCount, info.MaxPlayers);

                roomListEntries.Add(info.Name, entry);
            }
        }

    }
}
