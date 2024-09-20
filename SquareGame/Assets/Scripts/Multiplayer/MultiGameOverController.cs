using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MultiGameOverController : MonoBehaviour
{
    public Image announcementImage;
    public Sprite[] resultImage;
    public GameObject VictoryParticles;
    // Start is called before the first frame update
    void Start()
    {
        if(SquareController.Instance.PlayerIQScore > SquareController.Instance.opponentScore)
        {
            announcementImage.sprite = resultImage[0];
            VictoryParticles.SetActive(true);
            announcementImage.enabled = true;
        }
        else if (SquareController.Instance.PlayerIQScore < SquareController.Instance.opponentScore)
        {
            announcementImage.sprite = resultImage[1];
            announcementImage.enabled = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
