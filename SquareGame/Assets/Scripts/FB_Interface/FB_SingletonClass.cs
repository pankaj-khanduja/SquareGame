using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using System;
using UnityEngine.UI;
public class FB_SingletonClass : MonoBehaviour {

	public delegate void GetUserName(string name, string ID);
	public static event GetUserName Fb_UserName;
	
	public delegate void GetUserDP( Texture2D picTexture);
	public static event GetUserDP Fb_User_DP;

	public delegate void GetUserfriends( ArrayList F_List);
	public static event GetUserfriends Fb_Friends_List;


	public FB_SingletonClass(){

	}

	
	public void Fetch_FB_User_Data(){
		FB.API("/me", HttpMethod.GET, this.UserCallBack);	
		FB.API ("/me/picture?width=100&height=100", HttpMethod.GET, this.ProfilePhotoCallback);
//		FB.API("/me/friends", HttpMethod.GET, OnFriendsDownloaded);

	}

	void UserCallBack(IResult result) {
		if (!string.IsNullOrEmpty(result.RawResult))
		{   
			Debug.Log ("com.tengaming.test   ");
			string playerName = result.ResultDictionary["name"].ToString();
			string playerid = result.ResultDictionary["id"].ToString();
			Fb_UserName(playerName,playerid );
			FB.ActivateApp();
			
		}

	}

	private void ProfilePhotoCallback(IGraphResult result)
	{
		if (string.IsNullOrEmpty(result.Error) && result.Texture != null)
		{
			Fb_User_DP( result.Texture);
//			GameObject.Find ("RawImage").GetComponent<RawImage> ().texture = result.Texture;
//			GameObject.Find ("SignIn").GetComponent<Text> ().text = "Sign Out";

			//			this.profilePic = result.Texture;
		}

	}


	public void GetFriendsPlayingThisGame()
	{
		FB.API("/me/friends", HttpMethod.GET, result =>
		       {
			var dictionary = (Dictionary<string, object>)Facebook.MiniJSON.Json.Deserialize(result.RawResult);
			var friendsList = (List<object>)dictionary["data"];
			Debug.Log ("friends null" +dictionary.Values.ToString() + "   data  Count " +dictionary.Values +  " Input rray  "  +dictionary.Count + " Output rray  "  +dictionary.Keys);
			if(friendsList!=null){
				Debug.Log ("friends inner" +friendsList);

//				foreach (var dict in friendsList){
//
//					Debug.Log ("friends ddd" +FriendsText.text);	
//					FriendsText.text += ((Dictionary<string, object>)dict)["name"];
//				
//				}
			}

		});
	}

//	void OnFriendsDownloaded(IGraphResult result) {
//		ArrayList FriendsList = new ArrayList ();
//		if (result.Error != null) {
//			Debug.LogError("Error getting FB friends: " + result.Error);
//		}
//		else {
//			Dictionary<string, object> responseObject = Facebook.MiniJSON.Json.Deserialize(result.RawResult) as Dictionary<string, object>;  
//			List<object> obj = (List<object>)responseObject["data"];
//			Debug.Log ("friends " +(List<object>)responseObject["data"]);	
//			foreach (var dict in obj){
//				Debug.Log ("friends innner" +FriendsList);	
//				object objectName = ((Dictionary<string, object>)dict)["name"];
//				Debug.Log ("friends innner" +objectName);	
//				FriendsList.Add(objectName);
//
//			}
//			Fb_Friends_List (FriendsList);
//		}
//		StopCoroutine ("Fetch_FB_Data");
//	}
	
//	IEnumerator GetFriendsFacebookUserInfo(List<object> responseDataObjects) {
//		int numFriends = responseDataObjects.Count;
//		foreach (object friendDataObject in responseDataObjects) {
//			Dictionary<string, object> friendDataObjectDict = friendDataObject as Dictionary<string, object>;
//			// get one friend info at a time
////			yield return StartCoroutine(GetFacebookUserInfo("/" + friendDataObjectDict["id"].ToString(), false));
////			if (FriendUserFacebookInfos.Count == numFriends) {
////				friendsInfoDownloadedEvent.Invoke();
////			}
//		}
//	}
	
}
