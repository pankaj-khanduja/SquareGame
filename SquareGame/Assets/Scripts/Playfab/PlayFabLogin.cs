
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayFabLogin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LoginWithCustomID();
    }

    // Method to log in using a Custom ID
    private void LoginWithCustomID()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,  // Use device unique identifier for login
            CreateAccount = true  // Automatically create an account if the player doesn't have one
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }

    // Callback for successful login
    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Successfully logged into PlayFab!");

        // Optionally, you can now retrieve player information, stats, etc.
    }

    // Callback for failed login
    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogError("Error logging into PlayFab: " + error.GenerateErrorReport());
    }
}
