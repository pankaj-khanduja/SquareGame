using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TimeHighlightCounter : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI highlightText;
    [SerializeField] Transform targetPos;
    public bool isAnimationCompleted = false;
    public Animator parentAnimator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        parentAnimator.enabled = true;
        parentAnimator.Play("Anim",0,0);
        highlightText.text = SquareController.Instance.GetManager().HighlightText;
        return;
        transform.localScale = (new Vector3(0.5f, 0.5f, 0.5f));
       transform.localPosition = Vector3.zero;
        float timeLeft = SquareController.Instance.GetManager().TimeLeft;
        string timeText = String.Format("{0:00}:{1:000}", Mathf.FloorToInt(timeLeft % 60), Mathf.FloorToInt((timeLeft * 1000) % 1000));
        highlightText.text = "You have\n" + timeText + "\nto memorize";
        StartCoroutine("AnimateScele");
    }

    IEnumerator AnimateScele()
    {
        while(transform.localScale.x < 1.2f)
        {
            transform.localScale += Vector3.one * Time.deltaTime * 5;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(1);
        //Debug.Log(" Vector3.Distance(transform.position , targetPos.position)   " + Vector3.Distance(transform.position, targetPos.position));
        while(Vector3.Distance(transform.position , targetPos.position) > 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos.position, Time.deltaTime * 1000);
            if(transform.localScale.x > 0.7f)
            transform.localScale -= Vector3.one * Time.deltaTime * 10;
            yield return null;
        }
        SquareController.Instance.GetManager().GenerateSquare();
        gameObject.SetActive(false);

    }

   

    // Update is called once per frame
    void Update()
    {
        if(isAnimationCompleted)
        {
            Debug.Log("    ========12334=========");
            isAnimationCompleted = false;
            SquareController.Instance.onAction?.Invoke();
            //SquareController.Instance.GetManager().GenerateSquare();
            parentAnimator.enabled = false;
            gameObject.SetActive(false);
        }
    }
}
