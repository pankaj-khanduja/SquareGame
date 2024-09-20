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
        public static float range1 = 0.1f;
        public static int duration = 10;
        public static float speed = 0.8f;
        public static GameMode gameMode;
        public static GameNetwok gameNetwork;
        public static int[] directionMode = new int[] { -1, 1 };
        public static bool isPlayingMulti = false;

        public static void SwitchScene(Scene scene)
        {
            SceneManager.LoadScene(scene.ToString());
        }

        public static bool IsPlayerLogin
        {
            get
            {
                return  Convert.ToBoolean( PlayerPrefs.GetInt(playerLoginStatus, 0)) ;
            }
        }

        public static void SetPlayerLoginStatus(bool value)
        {
            PlayerPrefs.SetInt(playerLoginStatus, Convert.ToInt32(value));
        }


    }

}
