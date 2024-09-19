using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISquare
{
    public float TimeLeft { get; }
    public int NoOfSquares { get; }
    public int NoOfChances {get;}
    public string HighlightText { get; }

    /// <summary>
    ///     generate squares 
    /// </summary>
    /// <param name="callback"> callback function when all square generated on screen </param>
    /// <param name="noOfRounds"> no of squares to be generated /// </param>
    public void GenerateSquare();
      /// <summary>
    ///     Method called 
    /// </summary>
    public void MoveToNextlevel();
     /// <summary>
    ///     Method called to remove lines
    /// </summary>
    public void RemoveLines() { }
    /// <summary>
    ///     Method called when time start 
    /// </summary>
    public void OnBeginTime() { }
    /// <summary>
    ///     Method called when time over 
    /// </summary>
    public void OnEndTime() { }

    public void OnRestartGame();

    public bool IsCorrectSquare(GameObject obj , bool isSquare2);

    public void UndoStep(GameObject obj){}

    public void CheckForRoundCompletion();



}
