/*

using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using UnityEngine;

public class FBHelperScript : MonoBehaviour
{
    string playerName, playerID;
    public void LoginToFb()
    {
        if (!FB.IsLoggedIn)
        {
            FB.Init(this.OnInitComplete, this.OnHideUnity);
        }
    }

    private void OnInitComplete()
    {
        FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email" }, Fetch_FB_User_Data);
    }

    private void OnHideUnity(bool isGameShown)
    {
    }


    public void Fetch_FB_User_Data(IResult result)
    {
        FB.API("/me", HttpMethod.GET, this.UserCallBack);
    }

    void UserCallBack(IResult result)
    {
        if (!string.IsNullOrEmpty(result.RawResult))
        {
            Debug.Log("com.tengaming.test   ");
            playerName = result.ResultDictionary["name"].ToString();
            playerID = result.ResultDictionary["id"].ToString();
            FB.ActivateApp();
            FB.API("/me/picture?width=100&height=100", HttpMethod.GET, this.ProfilePhotoCallback);
        }

    }

    private void ProfilePhotoCallback(IGraphResult result)
    {
        if (string.IsNullOrEmpty(result.Error) && result.Texture != null)
        {
            //Fb_User_DP(result.Texture);
           
        }

    }


}
*/