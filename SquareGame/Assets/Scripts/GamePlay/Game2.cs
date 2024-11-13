using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SquareOne;
public class Game2 : MonoBehaviour, ISquare
{
    int connectingNumber;
    public float TimeLeft { get; private set; }
    public int NoOfSquares { get; private set; }
    public int NoOfChances { get; private set; }
    public string HighlightText { get; private set; }

    void Start()
    {

        Constant.range1 = 0.01f;
        TimeLeft = 60;
        HighlightText = "You have 60 seconds\nto connect as much as you can";
        SquareController.Instance.opponentScore = 0;
        SquareController.Instance.onTimeHighlight?.Invoke(OnRestartGame);
        //OnRestartGame();
    }

    private void Update()
    {

        if (SquareController.Instance.gameTime < 10)
        {
            if (SquareController.Instance._timerTextColor.Equals(Color.white))
            {
                SquareController.Instance._timerTextColor = Color.red;
            }
        }
        else
        {
            if (SquareController.Instance._timerTextColor.Equals(Color.red))
            {
                SquareController.Instance._timerTextColor = Color.white;
            }
        }

    }

    public void OnRestartGame()
    {
      
        NoOfSquares = 1;
        SquareController.Instance.ResetIQ();
        RoundCleared();
        SquareController.Instance.onGameBegin?.Invoke();
        if (Constant.TutorialStatus && Constant.gameNetwork == GameNetwok.Solo)
        {
            Instantiate(SquareController.Instance.PrefabTutorial);
        }
    }

    public void GenerateSquare()
    {
       
        
        for (int count = 0; count < NoOfSquares; count++)
        {
            GameObject obj = Instantiate(SquareController.Instance.squarePrefab, Vector3.zero, Quaternion.identity);
            while (obj.transform.position.Equals(Vector3.zero))
                obj.transform.position = SquareController.Instance.GenerateSpriteAtPos(obj, false);

            obj.GetComponent<SquarePrefab>().InIt(count);
            SquareController.Instance.AddSquareToList(obj);
            // obj.tr
        }
    }

    public void OnBeginTime()
    {
       
       
    }

    public void OnEndTime()
    {
        SquareController.Instance.OnResetGame();
        SquareController.Instance.OnGameOver();
        try
        {
            if (!Constant.isPlayingMulti)
            {
                PlayfabController.Instance.SubmitScore(SquareController.Instance.PlayerIQScore, GameMode.Game2);
            }
        }
        catch (System.Exception ex)
        {
            GameObject obj = Instantiate(SquareController.Instance.ScoreUpdateForm);
            obj.GetComponent<CloseScript>().Updatetext("Playfab not exist");
        }
        //NoOfSquares = 1;
        //SquareController.Instance.ResetIQ();
        //RoundCleared();
        //SquareController.Instance.onNextRoundBegin?.Invoke();
    }

    void OnLevelComplete()
    {
        RoundCleared();
    }

    bool IsChooseNextSequence(GameObject obj)
    {
        return (obj.GetComponent<SquarePrefab>().squareData.number - 1) == connectingNumber;
    }

    public bool IsCorrectSquare(GameObject obj, bool isSquare2)
    {
        // if( obj.GetComponent<SquarePrefab>().squareData.number == connectingNumber )
        if (!isSquare2 && IsChooseNextSequence(obj))
        {
            return false;
        }
      
        if (obj.GetComponent<SquarePrefab>().squareData.number == connectingNumber)
        {
            if (!isSquare2) connectingNumber++;
            else
            {
                if (Tutorial.Instance != null)
                {
                    Tutorial.Instance.TutoiralComplete();
                }
                SquareController.Instance.UpdateScore(1);
            }

               
            obj.GetComponent<SquarePrefab>().OnUserResponse(true);
            return true;
        }
        // obj.GetComponent<SquarePrefab>().OnUserResponse(false);
        return false;
    }

    public void UndoStep(GameObject squareObj)
    {
        connectingNumber--;
        squareObj.GetComponent<SquarePrefab>().CheckForUndo();

    }

    public void RemoveLines()
    {
        // GameObject.Find("LineRendererController").GetComponent<LineBetweenObjects>().RemoveLines();
    }

    public void CheckForRoundCompletion()
    {
        if (NoOfSquares == connectingNumber)
        {
            if (UnityEngine.Random.Range(0, 2) == 0 && SquareController.Instance.PlayerIQScore > 5) SquareController.Instance.AnimateEncouragingText();
            Invoke("OnLevelComplete", .1f);
        }
    }

    void RoundCleared()
    {
        SquareController.Instance.OnResetGame();
        MoveToNextlevel();
    }

    public void MoveToNextlevel()
    {
        connectingNumber = 1;
        NoOfSquares++;
        GenerateSquare();
        new GameObject("Line Renderer").AddComponent<LineRendererManager>();
    }


}
