using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class MultiplayerController : MonoBehaviourPunCallbacks
{
    PhotonView photonView;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region PUN CALLBACKS

    public override void OnConnectedToMaster()
    {
        Debug.Log("  OnConnectedToMaster ");
        //photonView = PhotonView.Get(this);
        PhotonNetwork.JoinRandomRoom();
        SquareController.Instance.roomStatus = "Searching for room";
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("  roomList " + roomList.Count); ;
    }

    public override void OnJoinedLobby()
    {
        // whenever this joins a new lobby, clear any previous room lists
       
    }

    // note: when a client joins / creates a room, OnLeftLobby does not get called, even if the client was in a lobby before
    public override void OnLeftLobby()
    {
        Debug.Log("   OnLeftLobby  ");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
       
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
       
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        SquareController.Instance.roomStatus = "Creating room";
        string roomName = "Room " + Random.Range(1000, 10000);

        RoomOptions options = new RoomOptions { MaxPlayers = 2};

        PhotonNetwork.CreateRoom(roomName, options, null);
    }

    public override void OnJoinedRoom()
    {
        SquareController.Instance.roomStatus = "Looking for players";
        Debug.Log("   Player Joned Room ");
        GameObject pl = PhotonNetwork.Instantiate(SquareController.Instance.player.name, Vector3.zero , Quaternion.identity);
        photonView = pl.GetComponent<PhotonView>();
        SquareController.Instance.viewID = photonView.ViewID;
        // joining (or entering) a room invalidates any cached lobby room list (even if LeaveLobby was not called due to just joining a room)

    }

    public override void OnLeftRoom()
    {
       
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        SquareController.Instance.roomStatus = "New Player Joined";
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2 && PhotonNetwork.IsMasterClient)
            this.photonView.RPC("OnGameStart", RpcTarget.All, SquareController.Instance.randomSeed);

    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("   OnPlayerLeftRoom  ");
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
      
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        
    }

    private void OnDestroy()
    {
        PhotonNetwork.Disconnect();
    }



    #endregion
}
