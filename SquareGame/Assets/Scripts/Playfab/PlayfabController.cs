using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Reflection;

public interface ICallback
{
    void OnCallBackData(Dictionary<string,string> data);
}
public class PlayfabController : MonoBehaviour 
{
    public static PlayfabController Instance { get; private set; } = null;
    public Action<Dictionary<string, string>> onCallBaack;
    public Action onDataSaveCallback;
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

    public void GetUSerData(string customID , Action<Dictionary<string, string>> onCallBaack)
    {
        var request = new GetUserDataRequest
        {
            PlayFabId = customID // Specify the PlayFabId of the other player
        };
        this.onCallBaack = onCallBaack;
        PlayFabClientAPI.GetUserData(request, OnDataReceived, OnError);
        
    }

    // Callback for successful data retrieval
    private void OnDataReceived(GetUserDataResult result)
    {
        Dictionary<string, string> customData = new Dictionary<string, string>();
        if (result.Data == null || result.Data.Count == 0)
        {
           
            Debug.Log("No user data available.");
        }
        else
        {
            // Loop through the retrieved data and print it
            foreach (var entry in result.Data)
            {
                Debug.Log($"Key: {entry.Key}, Value: {entry.Value.Value}");
                customData.Add(entry.Key , entry.Value.Value);
            }
            
        }
        this.onCallBaack.Invoke(customData);
    }

    // Callback for errors
    private void OnError(PlayFabError error)
    {
        Debug.LogError("Error retrieving user data: " + error.GenerateErrorReport());
    }

    public void SaveUserData(Dictionary<string , string > customData  , Action onDataSaveCallback)
    {

        Debug.Log("User data to save: " + string.Join(", ", customData));
        var request = new UpdateUserDataRequest
        {
            Data = customData,
            Permission = UserDataPermission.Public // Set this to Public or Private as needed
        };
        this.onDataSaveCallback = onDataSaveCallback;
        PlayFabClientAPI.UpdateUserData(request, OnDataSaveSuccess, OnDataUpdateError);
    }

    // Success callback
    private void OnDataSaveSuccess(UpdateUserDataResult result)
    {
        this.onDataSaveCallback.Invoke();
      
        //MyProfile.Instance.LoadPlayerData();
        Debug.Log("User data saved successfully.");
    }

    // Error callback
    private void OnDataUpdateError(PlayFabError error)
    {
        Debug.LogError("Error saving user data: " + error.GenerateErrorReport());
    }

}
