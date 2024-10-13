using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using SquareOne;
public class Game1Square : MonoBehaviour, ISquare
{
    int connectingNumber;
    public float TimeLeft { get; private set; }
    public int NoOfSquares { get; private set; }
    public int NoOfChances { get; private set; }
    public string HighlightText { get; private set; }


    private void OnEnable()
    {
        NoOfChances = 3;
        Debug.Log("Game1Square");
    }
    void Start()
    {
      
        Constant.range1 = 0.1f;
        OnRestartGame();
    }

    public void OnRestartGame()
    {
        NoOfSquares = 1;
        NoOfChances = 3;
        SquareController.Instance.ResetIQ();
        RoundCleared();
    }

    public void OnBeginTime()
    {


    }

    private void Update()
    {
        if(NoOfSquares > 8)
        {
            if(SquareController.Instance.gameTime < 3)
            {
                if(!SquareController.Instance._timerTextColor.Equals(Color.red))
                {
                    SquareController.Instance._timerTextColor = Color.red;
                }
            }
            else
            {
                if (!SquareController.Instance._timerTextColor.Equals(Color.white))
                {
                    SquareController.Instance._timerTextColor = Color.white;
                }
            }
        }
    }



    public void GenerateSquare()
    {
        SquareController.Instance.onGameBegin?.Invoke();
        Debug.Log($"no Of Squuuar {NoOfSquares}");
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

    public void OnEndTime()
    {
        Debug.Log("End Time Called");
        if(GameObject.Find("Line Renderer"))
        {
            Debug.Log("game Over");
            PlayfabController.Instance.SubmitScore(SquareController.Instance.PlayerIQScore, GameMode.Game1);
            CallGameOver();
            return;
        }
        
        TimeLeft = NoOfSquares ;
        if (Constant.TutorialStatus)
        {
            TimeLeft = 10;
        }
        string timeText = String.Format("{0:00}:{1:000}", Mathf.FloorToInt(TimeLeft % 60), Mathf.FloorToInt((TimeLeft * 1000) % 1000));
        HighlightText = "You have\n" + timeText + "\nto connect";
        foreach (var item in SquareController.Instance.GetSquareList)
        {
            item.GetComponent<SquarePrefab>().OnResetSquare();
        }
        SquareController.Instance.onTimeHighlight?.Invoke(OnAllowUserToConnect);
       
      
    }

    void OnAllowUserToConnect()
    {
        if (Constant.TutorialStatus)
        {
            Instantiate(SquareController.Instance.PrefabTutorial);
        }
        new GameObject("Line Renderer").AddComponent<LineRendererManager>();
        SquareController.Instance.onGameBegin?.Invoke();
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
        Debug.Log($" Connecting Numer {connectingNumber} and selected square {obj.GetComponent<SquarePrefab>().squareData.number}");
        if (obj.GetComponent<SquarePrefab>().squareData.number == connectingNumber)
        {
            if (!isSquare2) connectingNumber++;
            else
            {
                if(Tutorial.Instance != null)
                {
                    Tutorial.Instance.TutoiralComplete();
                }
                SquareController.Instance.UpdateScore(1);
            }
              
            obj.GetComponent<SquarePrefab>().OnUserResponse(true);
           
            return true;
        }
        NoOfChances--;
        if (SquareController.Instance.onPenatltyUpdate != null)
            SquareController.Instance.onPenatltyUpdate();
        obj.GetComponent<SquarePrefab>().OnUserResponse(false);
        if (NoOfChances == 0)
        {
            Debug.Log(" GAme Reset");
            CallGameOver();
        }
        return false;
    }

    void CallGameOver()
    {
        SquareController.Instance.gameTime = 0;
        SquareController.Instance.OnResetGame();
        SquareController.Instance.OnGameOver();

    }

    public void UndoStep(GameObject squareObj)
    {
        connectingNumber--;
        squareObj.GetComponent<SquarePrefab>().CheckForUndo();
        squareObj.GetComponent<SquarePrefab>().OnResetSquare();
    }

    public void RemoveLines()
    {
        // GameObject.Find("LineRendererController").GetComponent<LineBetweenObjects>().RemoveLines();
    }

    public void CheckForRoundCompletion()
    {
        if (NoOfSquares == connectingNumber)
        {
            SquareController.Instance.onLevelCleared?.Invoke();
            if (UnityEngine.Random.Range(0, 2) == 0 && SquareController.Instance.PlayerIQScore > 9) SquareController.Instance.AnimateEncouragingText();
            Invoke("RoundCleared", 0.5f);
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
        if(NoOfSquares < 8)
            TimeLeft = NoOfSquares * 0.75f;
        else
            TimeLeft = NoOfSquares * 0.5f;
        string timeText = String.Format("{0:00}:{1:000}", Mathf.FloorToInt(TimeLeft % 60), Mathf.FloorToInt((TimeLeft * 1000) % 1000));
        HighlightText = "You have\n" + timeText + "\nto memorize";
        SquareController.Instance.onTimeHighlight?.Invoke(GenerateSquare);
        
    }




}
