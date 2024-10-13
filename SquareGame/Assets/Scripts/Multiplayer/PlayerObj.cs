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
            Debug.Log("Sharing Own Data");
            
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
        Debug.Log("Receiving oppo Data");

        ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
        playerProperties["ReadyKey"] = true;  // Set the ready status in player properties
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
        _ = StartCoroutine(nameof(CheckAllPlayerReady));

    }

    IEnumerator CheckAllPlayerReady()
    {
        bool isAllPlayerReady = false;
        while(!isAllPlayerReady)
        {
            yield return null;
            bool isAnyPlayerLeftTobeReady = false;
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if (!player.CustomProperties.ContainsKey("ReadyKey") || !(bool)player.CustomProperties["ReadyKey"])
                {
                    isAnyPlayerLeftTobeReady = true;
                    // At least one player is not ready
                    Debug.Log(player.NickName + " is not ready");
                    break;
                }
            }
            if(!isAnyPlayerLeftTobeReady) isAllPlayerReady = true;

        }

        if(isAllPlayerReady)
        {
            SquareController.Instance.Action_OnAllPlayerReady?.Invoke();
        }
    }


    [PunRPC]
    public void OnGameStart(int seed)
    {
      
        if (!PhotonNetwork.IsMasterClient)
        {
            SquareController.Instance.SetMasterSeed(seed);
        }
        Debug.Log("   OnGameStart " + photonViewX.IsMine);
        SquareController.Instance.Action_OnMultiplayerStart?.Invoke();
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
            Debug.Log("Received Socre from Oppo" + score);
        }
    }
}
