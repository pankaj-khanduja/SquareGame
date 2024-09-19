using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameOver : MonoBehaviour
{
    public TextMeshProUGUI _IQScoreText;
    // Start is called before the first frame update
   

    private void OnEnable()
    {
        _IQScoreText.text = SquareController.Instance.PlayerIQScore.ToString();
    }

   
}
