using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using SquareOne;
public class UIManager : MonoBehaviour
{
    public Button restart_Btn;
    public TextMeshProUGUI timeLeft_Text;
    public TextMeshProUGUI playerIQScore ;
    public GameObject squareDisplayObj;
    public GameObject timeHighlightObj;
    public GameObject LifeControllerObj;
    public GameObject InGameCanvas, WaitingCanvas , MultiScoreView;
    bool gameOn = false;
    // public 
    // Start is called before the first frame update
    void Start()
    {
        
        restart_Btn.onClick.AddListener(OnHomeBtnClicked);
        if (Constant.gameMode.Equals(GameMode.Game3)) {
            SquareController.Instance.onPenatltyUpdate += PenaltyPointUpdate;
           // squareDisplayObj.SetActive(true);
        }
        else SquareController.Instance.onGameBegin += SquareGenerated;

        SquareController.Instance.Action_OnMultiplayerStart = EnableMultiCanvas;
        SquareController.Instance.onTimeHighlight += TimeDisplayCounter;
        SquareController.Instance.onUserUpdate += OnUserUpdate;
        SquareController.Instance.onLevelCleared += LevelCleared;
        SquareController.Instance.Game1();
        playerIQScore.text = "IQ : 0";
        LifeControllerObj.SetActive(true);

        if(!Constant.isPlayingMulti)
        {
            InGameCanvas.SetActive(true);
        }else
        {
            WaitingCanvas.SetActive(true);
            playerIQScore.enabled = false;
            MultiScoreView.SetActive(true);
        }

    }

    void EnableMultiCanvas()
    {
        InGameCanvas.SetActive(true);
        WaitingCanvas.SetActive(false);
    }

    private void OnDisable()
    {
        if (SquareController.Instance == null) return;
        if(Constant.gameMode.Equals(GameMode.Game3)) SquareController.Instance.onPenatltyUpdate += PenaltyPointUpdate;
        else SquareController.Instance.onGameBegin -= SquareGenerated;

        SquareController.Instance.onTimeHighlight -= TimeDisplayCounter;
        SquareController.Instance.onUserUpdate -= OnUserUpdate;
        SquareController.Instance.onLevelCleared -= LevelCleared;
    }

    void OnUserUpdate()
    {
        playerIQScore.text = String.Format("IQ : {0}", SquareController.Instance.PlayerIQScore);
    }

    void SquareGenerated()
    {

        StopCoroutine("OnTimeCountDown");
        Debug.Log("Square generated!!!");
        SquareController.Instance.gameTime = SquareController.Instance.GetManager().TimeLeft;
       
        StartCoroutine("OnTimeCountDown");

    }

    void PenaltyPointUpdate()
    {
        timeLeft_Text.text = "Chances : "+SquareController.Instance.GetManager().NoOfChances.ToString();
    }

    void LevelCleared()
    {
        StopCoroutine("OnTimeCountDown");
    }




    float timeLeft;
    IEnumerator OnTimeCountDown()
    {
        SquareController.Instance.GetManager().OnBeginTime();
        Debug.Log(" timeLeft   " + SquareController.Instance.gameTime);
        while (SquareController.Instance.gameTime > 0)
        {

            SquareController.Instance.gameTime -= Time.deltaTime;
            timeLeft = SquareController.Instance.gameTime;
            timeLeft_Text.text = String.Format("{0:00}:{1:000}",  Mathf.FloorToInt(timeLeft % 60) , Mathf.FloorToInt(( timeLeft *1000)%1000 ));
            yield return null;
        }
        SquareController.Instance.gameTime = 0;
        timeLeft_Text.text = String.Format("{0:00}:{1:000}",  Mathf.FloorToInt(timeLeft % 60), Mathf.FloorToInt((timeLeft * 1000) % 1000));
        SquareController.Instance.GetManager().OnEndTime();
    }

    void TimeDisplayCounter(Action callBack)
    {
        SquareController.Instance.onAction = callBack;
        timeHighlightObj.SetActive(true);
    }


   

    private void Update()
    {
        if(!timeLeft_Text.color.Equals(SquareController.Instance._timerTextColor))
        {
            timeLeft_Text.color = SquareController.Instance._timerTextColor;
        }
    }
    void OnHomeBtnClicked()
    {
        Constant.SwitchScene(Scene.MenuScene);
        // SquareController.Instance.ResetSquare(MoveToNextlevel);
    }

    void MoveToNextlevel()
    {
        // SquareController.Instance.GetManager().MoveToNextlevel(SquareGenerated);

    }



}
