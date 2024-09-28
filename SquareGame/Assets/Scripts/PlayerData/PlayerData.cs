using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SquareOne;
using System;
public struct PlayerDataContainer
{
    public  string playerName ; 
    public string playerID;
    public Texture2D userPic;

    public PlayerDataContainer(string playerName, string playerID , byte[] userPicBytes)
    {
        this.playerName = playerName;
        this.playerID = playerID;
        this.userPic = new Texture2D(2, 2);
        this.userPic.LoadImage(userPicBytes);
    }
}
public class PlayerData : MonoBehaviour
{
    public PlayerDataContainer _PlayerDataContainer;
    public Action callBack;
    public void LoadPlayerData(string loginID ,  Action callback)
    {
        this.callBack = callback;
        //PlayfabController <PlayerData> playfabController = new PlayfabController<PlayerData>();
        PlayfabController.Instance.GetUSerData(loginID, OnCallBackData);
    }



    public void OnCallBackData(Dictionary<string, string> data)
    {
        _PlayerDataContainer = new PlayerDataContainer(data[Constant.userName], data[Constant.customID], Convert.FromBase64String(data[Constant.picBase64]));
        callBack.Invoke();
        
    }
}
