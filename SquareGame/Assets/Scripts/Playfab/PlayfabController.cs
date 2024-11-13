using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Reflection;
using SquareOne;
public interface ICallback
{
    void OnCallBackData(Dictionary<string,string> data);
}
public class PlayfabController : MonoBehaviour 
{
    public static PlayfabController Instance { get; private set; } = null;
    public Action<Dictionary<string, string>> onCallBaack;
    public Action<List<PlayerLeaderboardEntry>> onLeaderboaardDataLoaded;
    public Action onDataSaveCallback;
    public GameObject ScoreUpdateForm;
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
        UpdatePlayerDisplayName(customData[Constant.userName]);
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

    public void SubmitScore(int score, GameMode gameMode)
    {
        UpdatePlayerHighestScore(score, Constant.GetleaderboardName(gameMode, LeaderboardCategory.Weekly), Constant.GetleaderboardName(gameMode, LeaderboardCategory.Overall));
        //StartCoroutine(SubmitScoreAsync( score, gameMode));
    }

    IEnumerator SubmitScoreAsync(int score, GameMode gameMode)
    {
        UpdatePlayerHighestScore(score, Constant.GetleaderboardName(gameMode, LeaderboardCategory.Weekly), Constant.GetleaderboardName(gameMode, LeaderboardCategory.Overall));
        yield return new WaitForSeconds(0.1f);
        //UpdatePlayerHighestScore(score, Constant.GetleaderboardName(gameMode, LeaderboardCategory.Overall));
    }

    void UpdatePlayerHighestScore(int newScore ,  string leaderboardName ,  string leaderboardName2)
    {

        //var request = new UpdatePlayerStatisticsRequest
        //{
        //    Statistics = new List<StatisticUpdate>
        //            {
        //                new StatisticUpdate
        //                {
        //                    StatisticName = leaderboardName,
        //                    Value = newScore
        //                },
        //                new StatisticUpdate
        //                {
        //                    StatisticName = leaderboardName2, // Second leaderboard statistic
        //                     Value = newScore
        //                }
        //            }
        //};

        ////PlayFabClientAPI.UpdatePlayerStatistics(request, OnSubmitScoreSuccess, OnSubmitScoreFailure);

        //int currentHighScore = PlayerPrefs.GetInt(String.Join(" ",Constant.PlayFabID, leaderboardName), 0);
        //Debug.Log("  currentHighScore   " + currentHighScore  +"    playfabID    "+ String.Join(" ", Constant.PlayFabID, leaderboardName)) ;
        ////OpenSocreForm("Request Received to submit score " + newScore +"  previous High Score "+ currentHighScore  +"  ID "+ String.Join(" ", Constant.PlayFabID, leaderboardName));
        //if (newScore > currentHighScore)
        //{
        //    PlayerPrefs.SetInt(String.Join(" ", Constant.PlayFabID, leaderboardName), newScore);
        //    var request = new UpdatePlayerStatisticsRequest
        //    {
        //        Statistics = new List<StatisticUpdate>
        //            {
        //                new StatisticUpdate
        //                {
        //                    StatisticName = leaderboardName,
        //                    Value = newScore
        //                },
        //                new StatisticUpdate
        //                {
        //                    StatisticName = leaderboardName2, // Second leaderboard statistic
        //                     Value = newScore
        //                }
        //            }
        //    };
        //    OpenSocreForm("Sending Request " );
        //    PlayFabClientAPI.UpdatePlayerStatistics(request, OnSubmitScoreSuccess, OnSubmitScoreFailure);
        //}


        int currentHighScore = 0;
        PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest(), result =>
        {
          
            // Check if the player already has a score recorded
            foreach (var stat in result.Statistics)
            {
                if (stat.StatisticName == leaderboardName)
                {
                    currentHighScore = stat.Value;
                    break;
                }
            }
            string score = "previous high score " + currentHighScore + "  new score " + newScore;
            //OpenSocreForm(score);
            // Only update if the new score is higher
            if (newScore > currentHighScore)
            {
                var request = new UpdatePlayerStatisticsRequest
                {
                    Statistics = new List<StatisticUpdate>
                    {
                        new StatisticUpdate
                        {
                            StatisticName = leaderboardName,
                            Value = newScore
                        },
                        new StatisticUpdate
                        {
                            StatisticName = leaderboardName2, // Second leaderboard statistic
                             Value = newScore
                        }
                    }
                };

                PlayFabClientAPI.UpdatePlayerStatistics(request, OnSubmitScoreSuccess, OnSubmitScoreFailure);
            }
           
        }, error => { OpenSocreForm("Error getting player statistics");  Debug.LogError("Error getting player statistics: " + error.GenerateErrorReport()); });
    }

    void OpenSocreForm(string msg)
    {
        GameObject obj = Instantiate(ScoreUpdateForm);
        obj.GetComponent<CloseScript>().Updatetext(msg);
    }


    void SubmitScoreToServer(int score , string leaderboardName)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = leaderboardName,  // Name of the leaderboard in PlayFab
                    Value = score
                }
            }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(request, OnSubmitScoreSuccess, OnSubmitScoreFailure);
    }

    private void OnSubmitScoreSuccess(UpdatePlayerStatisticsResult result)
    {
        OpenSocreForm("Successfully submitted score to leaderboard");
        Debug.Log("Successfully submitted score to leaderboard!");
    }

    private void OnSubmitScoreFailure(PlayFabError error)
    {
        OpenSocreForm("Failed "+ error.GenerateErrorReport());
        Debug.LogError("Failed to submit score: " + error.GenerateErrorReport());
    }

    public void GetLeaderboardData(string leaderboardName , Action<List<PlayerLeaderboardEntry>> callback)
    {
        // Create the request to get the leaderboard
        var request = new GetLeaderboardRequest
        {
            StatisticName = leaderboardName,  // The name of the statistic in PlayFab (must match the one in the dashboard)
            StartPosition = 0,            // The position to start fetching results from (0 = top of the leaderboard)
            MaxResultsCount = 50          // Maximum number of results to fetch (e.g., top 10 players)
        };
        this.onLeaderboaardDataLoaded = callback;
        // Call the GetLeaderboard API
        PlayFabClientAPI.GetLeaderboard(request, OnGetLeaderboardSuccess, OnGetLeaderboardFailure);
    }

    // Success callback
    private void OnGetLeaderboardSuccess(GetLeaderboardResult result)
    {
        Debug.Log("Successfully retrieved leaderboard data!");
        onLeaderboaardDataLoaded?.Invoke(result.Leaderboard);
        foreach (var entry in result.Leaderboard)
        {
            Debug.Log($"Player: {entry.DisplayName}, Rank: {entry.Position}, Score: {entry.StatValue}");
            // Optionally, you can display this data in your UI
        }
    }

    // Failure callback
    private void OnGetLeaderboardFailure(PlayFabError error)
    {
        Debug.LogError("Failed to retrieve leaderboard data: " + error.GenerateErrorReport());
        LoadingComponent.instance.DisableLoader();
    }

    public void UpdatePlayerDisplayName(string newDisplayName)
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = newDisplayName    // The new display name for the player
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnUpdateDisplayNameSuccess, OnUpdateDisplayNameFailure);
    }

    private void OnUpdateDisplayNameSuccess(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Successfully updated display name to: " + result.DisplayName);
    }

    private void OnUpdateDisplayNameFailure(PlayFabError error)
    {
        Debug.LogError("Failed to update display name: " + error.GenerateErrorReport());
    }

    public void DestroyController()
    {
        Destroy(this.gameObject);
    }

}
