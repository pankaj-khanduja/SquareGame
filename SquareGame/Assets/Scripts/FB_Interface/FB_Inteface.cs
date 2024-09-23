﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using System;
using UnityEngine.UI;
using SquareOne;
public class FB_Inteface : MonoBehaviour {
	public static FB_Inteface _instance = null;
	string PlayerName,playerId ;

	ArrayList FriendsList; 
	Texture2D playerPic;
	FB_SingletonClass _Fb_Singleton;

	public delegate void CallBackINIT();
	public static event CallBackINIT _CallBackAfterLogged;

	void Awake(){
		if (_instance != null) {
			Destroy(this.gameObject);
			return;
		}
		INIT ();
		DontDestroyOnLoad (this.gameObject);
	}

	void INIT(){
		_instance = this;
		PlayerName = "";
		playerPic = null;
		FriendsList = new ArrayList ();

		_Fb_Singleton = new FB_SingletonClass ();
		FB_SingletonClass.Fb_UserName += SetCallback;
		FB_SingletonClass.Fb_User_DP += SetUserDisplayPic;
//		FB_SingletonClass.Fb_Friends_List += SetFriendsList;
	}
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
	//======================================== Facebook Getter Methods =============================//
	public string GetPlayerFB_UserName(){
		return PlayerName;
	}
	public Texture2D GetPlayerDP(){
		return playerPic;
	}

	public string GetPlayerID(){
		return playerId;
	}

	public ArrayList GetFirendsList(){
		return FriendsList;
	}

	public bool IsPlayerLoggedInFb{
		get{return FB.IsLoggedIn; }
	}
	public bool IsPlayerIntialised{
		get{return FB.IsInitialized; }
	}
	public byte[] DecodeTexture(){
		return playerPic.EncodeToPNG ();
	}


	//======================================= FAcebook Setter Methods =============================//
	void SetCallback(string name, string ID){
		PlayerName = name;
		playerId = ID;

		Debug.Log ("  Player Name  " + PlayerName);
	}

	void SetUserDisplayPic(Texture2D pic){
		playerPic = pic;
		_CallBackAfterLogged();
	}

	void SetFriendsList(ArrayList F_List){
		FriendsList = (F_List);
	}

	// ==================================== Resetting Values ========================================//
	public void ResetValues(){
		playerPic = null;
		PlayerName = "";
		playerId = "";
		
	}

	// ======================================= Login And Initialize Facebook ===============================// 
	public void LoginToFb(){
//		if (InternetConnection.CheckInternet.IsInternetActive ()) {
		if (!FB.IsLoggedIn) {
			FB.Init (this.OnInitComplete, this.OnHideUnity);
		}
//		} 
	}

	private void OnInitComplete()
	{
		FB.LogInWithReadPermissions (new List<string> () { "public_profile", "email"}, this.Fetch_User_DB);
	}
	
	private void OnHideUnity(bool isGameShown){
	}

	public void Fetch_User_DB(IResult result)
	{
		if (FB.IsInitialized) {
			_Fb_Singleton.Fetch_FB_User_Data ();
		}
	}

	public void OnFBInvite_Clicked(){
		if (FB.IsInitialized)
			FB.AppRequest ("Come and play against me Buddy!", callback: this.HandleResults);
	}
	
	protected void HandleResults(IResult result)
	{
		Debug.Log ("Sent Request");
		//GameObject.Find ("SignIn").GetComponent<Text> ().text ="Successfull";
	}

	public void Loadscene()
	{
		Constant.SwitchScene(Scene.MenuScene);
	}






}