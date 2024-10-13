using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SquareOne;
public class Tutorial : SingletonComponent<Tutorial>
{

    public GameObject PrefabHand ;
    public TextMeshProUGUI tutorialText;


    GameObject handObj;
    // Start is called before the first frame update

    private void OnEnable()
    {
       
            handObj = Instantiate(PrefabHand);
            tutorialText.text = Constant.SceneTutorialText();
        if(Constant.gameMode.Equals(GameMode.Game3))
            StartCoroutine(ScalingAnim());
        else
            StartCoroutine(MovementAnim());
        
      
    }

    void Update()
    {
        if(handObj && SquareController.Instance.GetSquareList.Count == 0)
        {
            if (handObj) Destroy(handObj);
            Destroy(this.gameObject);
        }
    }

    public void TutoiralComplete()
    {
        Constant.TutorialStatus = false;
        if (handObj) Destroy(handObj);
        Destroy(this.gameObject);
    }


    IEnumerator MovementAnim()
    {
      
            Vector3 targetPos = SquareController.Instance.GetSquareList[1].transform.position + (Vector3.up * -0.5f);
            while (true)
            {
                
                handObj.transform.position = SquareController.Instance.GetSquareList[0].transform.position + (Vector3.up * -0.5f);
                while (!handObj.transform.position.Equals(targetPos))
                {

                    handObj.transform.position = Vector3.MoveTowards(handObj.transform.position, targetPos, Time.deltaTime * 2);
                    yield return new WaitForEndOfFrame();
                }
                yield return new WaitForEndOfFrame();
            }
        
       
    }

    IEnumerator ScalingAnim()
    {

        while (true)
        {

            handObj.transform.position = SquareController.Instance.GetSquareList[0].transform.position + (Vector3.up * -0.5f);
            while (handObj.transform.localScale.x > 0.5f)
            {

                handObj.transform.localScale -= Vector3.one * Time.deltaTime; 
                yield return new WaitForEndOfFrame();
            }
            while (handObj.transform.localScale.x < 1f)
            {

                handObj.transform.localScale += Vector3.one * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();
        }


    }
}
