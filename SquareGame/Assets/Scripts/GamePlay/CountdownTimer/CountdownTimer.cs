using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CountdownTimer : MonoBehaviour
{
    int countdown = 3;
    public TextMeshProUGUI countdownText;
    // Start is called before the first frame update
    void Start()
    {
        countdownText = GetComponent<TextMeshProUGUI>();
        StartCoroutine("CountdownAnimate");
    }

    IEnumerator CountdownAnimate()
    {
        while(countdown >0)
        {
            countdownText.text = countdown.ToString();
            this.transform.localScale = Vector3.one;
            while (this.transform.localScale.x >  0.2f)
            {
                this.transform.localScale -= this.transform.localScale * (Time.deltaTime * 2);
                yield return null;
            }
            --countdown;
        }
        SquareController.Instance.StartGame();
        this.gameObject.SetActive(false);

    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
