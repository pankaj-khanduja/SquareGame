using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon;
using Photon.Realtime;

public class PlayerObj : MonoBehaviourPun, IPunObservable
{

    public int score;
    public string playerName;
    public Texture2D texture;


    PhotonView photonViewX;
    private void Start()
    {
        photonViewX = GetComponent<PhotonView>();
    }


    void SharePlayerData()
    {
        if (photonViewX.IsMine)
        {
            playerName = MyProfile.Instance._PlayerData._PlayerDataContainer.playerName;
            texture = MyProfile.Instance._PlayerData._PlayerDataContainer.userPic;
            byte[] picByte = texture.EncodeToPNG();
            object[] sendingData = new object[]
            {
                playerName ,picByte
            };
            photonViewX.RPC("PlayerData", RpcTarget.Others, sendingData);
            SquareController.Instance.Action_OnLocalPlayerDataReceived.Invoke(playerName, texture);
            Debug.Log("Datatatattata");
            
        }
    }


    [PunRPC]
    public void PlayerData(object[] receivingData)
    {
        playerName = receivingData[0].ToString();
        byte[] picByte = (byte[])receivingData[1];
        texture = new Texture2D(64, 64, TextureFormat.RGBA32, false);
        texture.LoadImage(picByte);
        texture.Apply();
        SquareController.Instance.Action_OnOpponentDataReceived.Invoke(playerName , texture);
    }


    [PunRPC]
    public void OnGameStart(int seed)
    {
      
        if (!PhotonNetwork.IsMasterClient)
        {
            SquareController.Instance.SetMasterSeed(seed);
        }
        SquareController.Instance.Action_OnMultiplayerStart?.Invoke();
        SquareController.Instance.StartGame2();
        Debug.Log("    ===========   =======  " + photonViewX.IsMine);
        if (photonViewX.IsMine)
            SharePlayerData();
        else
            GetOtherPlayerPhotonView();
    }

    void GetOtherPlayerPhotonView()
    {
        GameObject playerGameObject = PhotonView.Find(SquareController.Instance.viewID).gameObject;
        playerGameObject.GetComponent<PlayerObj>().SharePlayerData();


        //foreach (Player player in PhotonNetwork.PlayerList)
        //{
        //    if (player != PhotonNetwork.LocalPlayer) // Ignore the local player
        //    {
                
        //        Debug.Log("Found PhotonView with ID: " + player.ActorNumber);
        //        GameObject playerGameObject = PhotonView.Find(SquareController.Instance.viewID).gameObject;
        //        playerGameObject.GetComponent<PlayerObj>().SharePlayerData();
        //        //PhotonView playerPhotonView = playerGameObject.GetComponent<PhotonView>();
              
        //    }
        //}
    }

    private void Update()
    {
        if(this.photonView.IsMine)
        {
            score = SquareController.Instance.PlayerIQScore;
        }
        else
        {
            SquareController.Instance.opponentScore = score;
        }
    }

    // Implementing OnPhotonSerializeView to sync data
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // We are the one sending data to others
        if (stream.IsWriting)
        {
            // Send playerHealth value to other players
            stream.SendNext(score);
        }
        else
        {
            // We are receiving data
            score = (int)stream.ReceiveNext();
        }
    }
}
