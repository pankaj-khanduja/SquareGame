using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SquareOne;
using System;
public class MyProfile : MonoBehaviour
{
    public static MyProfile Instance = null;
    [HideInInspector]
    public PlayerData _PlayerData;
    public Action OnPlayerDataLoaded;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            return;
        }
        Destroy(this.gameObject);
    }

    private void OnEnable()
    {
        _PlayerData = gameObject.AddComponent<PlayerData>();
        LoadPlayerData();
    }

    public void SaveProfileData(Dictionary<string, string> customData)
    {
        LoadingComponent.instance.Enableloader();
        PlayfabController.Instance.SaveUserData(customData, LoadPlayerData);
    }
   
   

    public void LoadPlayerData()
    {
        _PlayerData.LoadPlayerData(Constant.PlayFabID, PlayerDataLoaded);
    }

    public void PlayerDataLoaded()
    {
        LoadingComponent.instance.DisableLoader();
        OnPlayerDataLoaded?.Invoke();
    }

    public void Logout()
    {
        PlayerPrefs.DeleteAll();
        Constant.SwitchScene(Scene.LoginScene);
    }

    
}
