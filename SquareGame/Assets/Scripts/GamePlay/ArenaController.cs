using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // SquareController.Instance.Game1();
        // SquareController.Instance.GetManager().GenerateSquare(10 ,SquareGenerated);
    }

    public void SquareGenerated()
    {
        Debug.Log("Square generated!!!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
