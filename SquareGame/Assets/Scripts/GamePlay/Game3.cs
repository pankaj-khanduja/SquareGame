    using System.Collections;
    using System.Collections.Generic;
    using Unity.VisualScripting;
    using UnityEngine;
using SquareOne;
public class Game3 : MonoBehaviour, ISquare
    {
    int connectingNumber;
    public float TimeLeft { get; private set; }
    public int NoOfSquares { get; private set; }
    public int NoOfChances { get; private set; }
    public string HighlightText { get; private set; }

    float timeToGenerateSquare;
    float timeToReduceRepeatinghRate ;

    private void OnEnable()
    {
        NoOfChances = 3;
        
    }
    private void Start() {
        HighlightText = "Pop square \nas many as you can";
        SquareController.Instance.onTimeHighlight?.Invoke(MoveToNextlevel);
        //MoveToNextlevel();
    }

    

    public void GenerateSquare()
    {
      
        Debug.Log($"no Of Squuuar {NoOfSquares}");
       
            GameObject obj = Instantiate(SquareController.Instance.squarePrefab, Vector3.zero, Quaternion.identity);
            while (obj.transform.position.Equals(Vector3.zero))
                obj.transform.position = SquareController.Instance.GenerateSpriteAtPos(obj, true);

        obj.GetComponent<SquarePrefab>().OnResetSquare();
            obj.AddComponent<BoxCollider2D>();
                obj.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            SquareController.Instance.AddSquareToList(obj);
        if(SquareController.Instance.GetSquareList.Count% 10 == 0)
        {
            SquareController.Instance.AnimateEncouragingText();
        }
        if(SquareController.Instance.GetSquareList.Count >= 100)
        {
            CallGameOver();
        }
        if (Constant.TutorialStatus)
        {
            Instantiate(SquareController.Instance.PrefabTutorial);
        }
        // obj.tr

    }

    void CallGameOver()
    {

        try
        {
            PlayfabController.Instance.SubmitScore(SquareController.Instance.PlayerIQScore, GameMode.Game3);
        }
        catch (System.Exception ex)
        {
            GameObject obj = Instantiate(SquareController.Instance.ScoreUpdateForm);
            obj.GetComponent<CloseScript>().Updatetext("Playfab not exist");
        }
        CancelInvoke();
        SquareController.Instance.OnResetGame();
        SquareController.Instance.OnGameOver();
        
    }

    public void OnBeginTime()
    {
        
    }

    public void OnEndTime()
    {
        // NoOfSquares = 1;
        // SquareController.Instance.ResetIQ ();
        // RoundCleared();
    }

    bool IsChooseNextSequence(GameObject obj)
    {
        return (obj.GetComponent<SquarePrefab>().squareData.number - 1) == connectingNumber;
    }

    public bool IsCorrectSquare(GameObject obj, bool isSquare2)
    {
        try
        {
            bool isPenaltySquare = obj.GetComponent<SquarePrefab>().squareData.isPenaltySquare;
            if(isPenaltySquare)
            {
                obj.GetComponent<SquarePrefab>().ShowRedAlert();
            }
            obj.GetComponent<SquarePrefab>().DestroySquare();
            if (isPenaltySquare)
            {
                NoOfChances--;
                if (Application.isMobilePlatform)
                {
                    Handheld.Vibrate();
                }

                if (SquareController.Instance.onPenatltyUpdate != null)
                    SquareController.Instance.onPenatltyUpdate();
                if (NoOfChances == 0)
                {
                    Debug.Log(" GAme Reset");
                    CallGameOver();
                }
            }
            else
            {
                if (Tutorial.Instance != null)
                {
                    Tutorial.Instance.TutoiralComplete();
                }
                SquareController.Instance.UpdateScore(1);
            }

        }
        catch (System.Exception ex)
        {
            Debug.Log(" IsCorrectSquare Exception  " + ex);
        }
        return false;

    }

    public void UndoStep(GameObject squareObj)
    {


    }

    public void RemoveLines()
    {
       
    }

    public void CheckForRoundCompletion()
    {

    }

    void RoundCleared()
    {
        
    }

    public void MoveToNextlevel()
    {
       
        SquareController.Instance.ResetIQ();
        NoOfSquares = 3;
       
        timeToGenerateSquare = Constant.speed;
        timeToReduceRepeatinghRate = Constant.duration;
        Debug.Log("  timeToGenerateSquare  " + timeToGenerateSquare + "  timeToReduceRepeatinghRate  " + timeToReduceRepeatinghRate);
        SquareController.Instance.onPenatltyUpdate();
        InvokeRepeating("GenerateSquare", 0.1f, timeToGenerateSquare);
        InvokeRepeating("ReduceRepeatingRate", timeToReduceRepeatinghRate, timeToReduceRepeatinghRate);
        InvokeRepeating("GeneratePenaltySquare", 5f, Random.Range(0.5f, 2.0f));
        new GameObject("Tap Handler").AddComponent<TapHandler>();


    }



    void GeneratePenaltySquare()
    {
        GameObject obj = Instantiate(SquareController.Instance.squarePrefab, Vector3.zero, Quaternion.identity);
        while (obj.transform.position.Equals(Vector3.zero))
            obj.transform.position = SquareController.Instance.GenerateSpriteAtPos(obj, true);
        obj.GetComponent<SquarePrefab>().OnResetSquare();
        obj.AddComponent<BoxCollider2D>();
        obj.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        obj.GetComponent<SquarePrefab>().PenaltySquare();
        SquareController.Instance.AddSquareToList(obj);
    }

    public void OnRestartGame()
    {
        NoOfChances = 3;
        MoveToNextlevel();
    }

    void ReduceRepeatingRate()
    {
        if (timeToGenerateSquare > 0.2f)
        {
            if(timeToGenerateSquare < 0.4f)
                timeToGenerateSquare -= 0.05f;
            else
                timeToGenerateSquare -= 0.1f;
            CancelInvoke("GenerateSquare");
            Debug.Log("  ReduceRepeatingRate timeToGenerateSquare  " + timeToGenerateSquare );
            InvokeRepeating("GenerateSquare", 0.01f, timeToGenerateSquare);
        }
    }
    }
