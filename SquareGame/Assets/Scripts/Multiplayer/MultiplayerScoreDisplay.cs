using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MultiplayerScoreDisplay : MonoBehaviour
{
    public bool isMaster;
     TextMeshProUGUI _ScoreText;
    // Start is called before the first frame update
    void Start()
    {
        _ScoreText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SquareController.Instance == null) return;
        if(isMaster)
        {
            _ScoreText.text = "IQ : "+SquareController.Instance.PlayerIQScore.ToString();
        }else
        {
            _ScoreText.text = "IQ : " + SquareController.Instance.opponentScore.ToString();
        }
    }


}
