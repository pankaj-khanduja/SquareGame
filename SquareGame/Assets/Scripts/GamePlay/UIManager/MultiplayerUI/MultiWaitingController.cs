using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class MultiWaitingController : MonoBehaviour
{
    public TextMeshProUGUI _WaitingText;
    public Image progressBar;
    float waitingTime ;
    // Start is called before the first frame update
    void Start()
    {
        waitingTime = SquareController.Instance.multiWaitingTimeInSec;
        StartCoroutine("AnimateProgressBar");
    }

   

    IEnumerator AnimateProgressBar()
    {
        while (waitingTime > 0)
        {
            waitingTime -= Time.deltaTime;
            progressBar.fillAmount = waitingTime / SquareController.Instance.multiWaitingTimeInSec;
            yield return null;
        }
        _WaitingText.text = "No Room Found";
    }
}
