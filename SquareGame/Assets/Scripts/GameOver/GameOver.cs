using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SquareOne;
public class GameOver : MonoBehaviour
{
    
    // Start is called before the first frame update
    public GameObject SoloGameOverCanvas, MultiGameOverCanas;
  

    private void Start()
    {
       GameObject obj =  Constant.isPlayingMulti ? MultiGameOverCanas: SoloGameOverCanvas;
       obj.SetActive(true);

    }


}
