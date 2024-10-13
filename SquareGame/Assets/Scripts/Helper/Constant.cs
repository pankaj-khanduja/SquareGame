using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SquareOne
{

   
    public enum GameMode
    {
        Game1 = 0,
        Game2,
        Game3
    }

    public enum GameNetwok
    {
        Solo = 0,
        Multi
    }

    public enum Scene
    {
        LoginScene = 0,
        MenuScene,
        GameScene
    }

    public enum LeaderboardCategory
    {
        Weekly = 0 ,
        Overall
    }

    public static class Constant
    {
        public const string playerLoginStatus = "playerloginStatus";
        public static float range1 = 0.1f;
        public static int duration = 10;
        public static float speed = 0.8f;
        public static GameMode gameMode;
        public static GameNetwok gameNetwork;
        public static int[] directionMode = new int[] { -1, 1 };
        public static bool isPlayingMulti = false;
        public const string userName = "userName";
        public const string picBase64 = "picBase64";
        public const string customID = "customID";


        public static void SwitchScene(Scene scene)
        {
            SceneManager.LoadScene(scene.ToString());
        }

        public static bool PlayerLogin
        {
            set
            {
                PlayerPrefs.SetInt(playerLoginStatus, Convert.ToInt32(value));
            }
            get
            {
                return  Convert.ToBoolean( PlayerPrefs.GetInt(playerLoginStatus, 0)) ;
            }
        }
     

        public static string PlayFabID
        {
            get
            {
                return PlayerPrefs.GetString("PlayFabID", string.Empty);    
            }
            set
            {
                PlayerPrefs.SetString("PlayFabID", value);
            }
        }

        public static string CustomID
        {
            get
            {
                return PlayerPrefs.GetString("customID", string.Empty);
            }
            set
            {
                PlayerPrefs.SetString("customID", value);
            }
        }

        public static bool MuteStatus
        {
            get
            {
                return Convert.ToBoolean(PlayerPrefs.GetInt("MuteStatus", 0));
            }
            set
            {
                PlayerPrefs.SetInt("MuteStatus", Convert.ToInt32(value));
            }
        }

        public static string GetleaderboardName(GameMode gameCategory , LeaderboardCategory leaderboardCategory)
        {
            if (gameCategory.Equals(GameMode.Game1) && leaderboardCategory.Equals(LeaderboardCategory.Weekly))
                return "memory_weekly";
            else if (gameCategory.Equals(GameMode.Game1) && leaderboardCategory.Equals(LeaderboardCategory.Overall))
                return "memory_overall";
            else if (gameCategory.Equals(GameMode.Game2) && leaderboardCategory.Equals(LeaderboardCategory.Weekly))
                return "speed_weekly";
            else if (gameCategory.Equals(GameMode.Game2) && leaderboardCategory.Equals(LeaderboardCategory.Overall))
                return "speed_overall";
            else if (gameCategory.Equals(GameMode.Game3) && leaderboardCategory.Equals(LeaderboardCategory.Weekly))
                return "accuracy_weekly";
            else 
                return "accuracy_overall";
           
        }

        public static bool TutorialStatus
        {
            get
            {
                Debug.Log("  Constant.gameMode.ToString()   " + Constant.gameMode.ToString());
                return Convert.ToBoolean(PlayerPrefs.GetInt(Constant.gameMode.ToString(), 1));
            }
            set
            {
                PlayerPrefs.SetInt(Constant.gameMode.ToString(), Convert.ToInt32(value));
            }
        }

        public static string SceneTutorialText()
        {
            switch (Constant.gameMode)
            {
                case GameMode.Game1: return "Drag your finger to connect squares"; 
                case GameMode.Game2: return "Drag your finger to connect squares"; 
                default: return "Pop grey squares and avoid red one"; 

            }

        }




    }

}
