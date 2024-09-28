using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PlayFab.ClientModels;
using SquareOne;
using UnityEngine.UI;
public class PlayerScore : MonoBehaviour
{
    [SerializeField]
     TextMeshProUGUI playerNameText, scoreText , rank;
   

    public void DisplayData(PlayerLeaderboardEntry data)
    {
        playerNameText.text = data.DisplayName;
        scoreText.text = data.StatValue.ToString();
        rank.text = (data.Position+1) .ToString();
        if(data.PlayFabId == Constant.PlayFabID)
        {
            GetComponent<Image>().color = Color.white;
        }
    }


    public void ResetData()
    {
        playerNameText.text = scoreText.text = rank.text = string.Empty;
    }
}
