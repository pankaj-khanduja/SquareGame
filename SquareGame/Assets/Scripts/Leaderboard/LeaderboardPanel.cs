using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SquareOne;
using PlayFab.ClientModels;

public class LeaderboardPanel : MonoBehaviour
{
    public GameObject leaderboardPrefab;
    public RectTransform Content;

    public GameObject ownPlayerData;
    public Button Btn_Memory, Btn_Speed, Btn_Accuracy , Btn_Weekly , Btn_Overall;
    GameMode leaderboardMode;
    LeaderboardCategory leaderboardCategory;
    List<GameObject> leaderboardData;
    // Start is called before the first frame update
    void Start()
    {
        Btn_Memory.onClick.AddListener(() => { leaderboardMode = GameMode.Game1; OnBtnClicked(); });
        Btn_Speed.onClick.AddListener(() => { leaderboardMode = GameMode.Game2; OnBtnClicked(); });
        Btn_Accuracy.onClick.AddListener(() => { leaderboardMode = GameMode.Game3; OnBtnClicked(); });
        Btn_Weekly.onClick.AddListener(() => { leaderboardCategory = LeaderboardCategory.Weekly; OnBtnClicked(); });
        Btn_Overall.onClick.AddListener(() => { leaderboardCategory = LeaderboardCategory.Overall; OnBtnClicked(); });

    }

    void OnBtnClicked()
    {
        LoadLeaderboardData();
    }

    private void OnEnable()
    {
        leaderboardData = new List<GameObject>();
        leaderboardMode = GameMode.Game1;
        leaderboardCategory = LeaderboardCategory.Weekly;
        LoadLeaderboardData();

    }

    void LoadLeaderboardData()
    {
        LoadingComponent.instance.Enableloader();
        DestrroyAllData();
        leaderboardData = new List<GameObject>();
        ownPlayerData.GetComponent<PlayerScore>().ResetData();
        Content.sizeDelta = new Vector2(Content.sizeDelta.x, 300);
        PlayfabController.Instance.GetLeaderboardData(Constant.GetleaderboardName(leaderboardMode, leaderboardCategory) , OnLeaderboardDataLoaded);


    }

    void OnLeaderboardDataLoaded(List<PlayerLeaderboardEntry> data)
    {
        foreach (var entry in data)
        {
            GameObject obj =  Instantiate(leaderboardPrefab);
            obj.transform.SetParent(Content.transform);
            Content.sizeDelta = new Vector2(Content.sizeDelta.x, Content.sizeDelta.y + 125);
            obj.GetComponent<PlayerScore>().DisplayData(entry);
            leaderboardData.Add(obj);
            Debug.Log($"Player: {entry.DisplayName}, Rank: {entry.Position}, Score: {entry.StatValue}");
            if(entry.PlayFabId.Equals(Constant.PlayFabID))
            {
                ownPlayerData.GetComponent<PlayerScore>().DisplayData(entry);
            }
            // Optionally, you can display this data in your UI
        }
        LoadingComponent.instance.DisableLoader();
    }

    void DestrroyAllData()
    {

        foreach(GameObject obj in leaderboardData)
        {
            Destroy(obj);
        }
    }

    private void OnDisable()
    {
        DestrroyAllData();
    }



}
