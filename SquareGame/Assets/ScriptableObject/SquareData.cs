using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SquareContainer", order = 1)]
public class SquareContainer : ScriptableObject
{
    public List<Square> Square;

}

[System.Serializable]
public class Square
{
    public int squareNumber;
    public Color squareColor;
}