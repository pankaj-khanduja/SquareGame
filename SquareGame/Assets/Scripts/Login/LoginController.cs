
using UnityEngine;
using SquareOne;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using System;
public class LoginController : SingletonComponent<LoginController>
{
   
    public GameObject LoginPanel;
    Dictionary<string, string> customData;

    private void Start()
    {
        customData = new Dictionary<string, string>();
        if (!Constant.PlayerLogin)
        {
            LoginPanel.SetActive(true);
            return;
        }
        Constant.SwitchScene(Scene.MenuScene);
    }


    public void LoginWithCustomID(Dictionary<string, string> customData)
    {
        LoadingComponent.instance.Enableloader();
        var request = new LoginWithCustomIDRequest
        {
            CustomId = customData[Constant.customID],  // Use device unique identifier for login
            CreateAccount = true  // Automatically create an account if the player doesn't have one
        };
        this.customData = customData;
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }

    // Callback for successful login
    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Successfully logged into PlayFab!"  + result.PlayFabId);

        PlayfabController.Instance.SaveUserData(customData, OnUserDataSaved);
        // Optionally, you can now retrieve player information, stats, etc.
    }

    // Callback for failed login
    private void OnLoginFailure(PlayFabError error)
    {
        LoadingComponent.instance.DisableLoader();
        Debug.LogError("Error logging into PlayFab: " + error.GenerateErrorReport());
    }

    public void OnUserDataSaved()
    {
        LoadingComponent.instance.DisableLoader();
        Constant.PlayerLogin = (true);
        Constant.SwitchScene(Scene.MenuScene);
    }

   




}