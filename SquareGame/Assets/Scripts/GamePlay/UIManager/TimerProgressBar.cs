using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TimerProgressBar : MonoBehaviour
{
    Image img;
    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //if(img.fillAmount > 0)
        //{
        if (Constant.gameMode.Equals(GameMode.Game3))
            img.fillAmount = SquareController.Instance.GetSquareList.Count / 100.0f;
        else
            img.fillAmount = SquareController.Instance.gameTime/ SquareController.Instance.GetManager().TimeLeft;
           
        //}
    }
}
