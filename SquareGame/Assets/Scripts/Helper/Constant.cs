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
        MenuScene = 0,
        GameScene
    }

    public static class Constant
    {
        public const string playerLoginStatus = "playerloginStatus";
        public const string playerLoginID = "playerLoginID";
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
     

        public static string PlayerLoginID
        {
            get
            {
                return PlayerPrefs.GetString(playerLoginID, string.Empty);    
            }
            set
            {
                PlayerPrefs.SetString(playerLoginID, value);
            }
        }




    }

}
