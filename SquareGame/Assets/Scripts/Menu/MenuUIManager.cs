using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using EasyTransition;
using SquareOne;
using PlayFab;
public class MenuUIManager : MonoBehaviour
{
    public Button btn_Game1 , btn_Game2 , btn_Game3 , btn_Play , btn_Play1 , btn_Solo , btn_Multi;

    public GameObject _Panel , _Game3Panel , _Game2Lobby;
    public TMP_InputField _rangeInput , _durationInput , _speedInput;
    public Action gameCallBack;
    public TransitionSettings _transitionSetting;
    public GameObject MenuPanel, LoginPanel;
    // public s
    // Start is called before the first frame update
    void Start()
    {
        btn_Game1.onClick.AddListener(Game1Called);
        btn_Game2.onClick.AddListener(Game2Called);
        btn_Game3.onClick.AddListener(Game3Called);
        btn_Play.onClick.AddListener(SelectedRange);
        btn_Play1.onClick.AddListener(SelectedRange);
        btn_Solo.onClick.AddListener(SoloClicked);
        btn_Multi.onClick.AddListener(MultiClicked);

        GameObject obj = Constant.IsPlayerLogin ? MenuPanel : LoginPanel;
        obj.SetActive(true);

    }





    void Game1Called()
    {
        Constant.gameMode = GameMode.Game1;
        LoadGameScene();
    }

    void LoadGameScene()
    {

       // TransitionManager.Instance().Transition(Scene.GameScene.ToString(), _transitionSetting ,0.0f);
        //if(int.TryParse(_rangeInput.text,out int res))
        //    Constant.range1 = res / 100.0f;
        Constant.SwitchScene(SquareOne.Scene.GameScene);
    }

    void SelectedRange()
    {
        gameCallBack.Invoke();
    }

    void PanelOpen(Action callback)
    {
        _Panel.SetActive(true);
        gameCallBack = callback;
    }

    void PanelDual(Action callback)
    {
        _Game3Panel.SetActive(true);
        gameCallBack = callback;
    }
    void Game2Called()
    {
        Constant.gameMode = GameMode.Game2;
        _Game2Lobby.SetActive(true);
    }

    void SoloClicked()
    {
        Constant.gameNetwork = GameNetwok.Solo;
        LoadGameScene();
    }

    void MultiClicked()
    {
        Constant.gameNetwork = GameNetwok.Multi;
        LoadGameScene();
    }


     void Game3Called()
    {
        Constant.gameMode = GameMode.Game3;
        //if (int.TryParse(_durationInput.text, out int res)) Constant.duration = res;
        //if (float.TryParse(_speedInput.text, out float res1)) Constant.speed = res1;
        //Constant.SwitchScene(Scene.GameScene);
        LoadGameScene();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveUserData()
    {

    }
}
