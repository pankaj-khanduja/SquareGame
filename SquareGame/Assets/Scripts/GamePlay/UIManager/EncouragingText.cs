using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EncouragingText : MonoBehaviour
{
    public string[] _Phrases;
    public TextMeshProUGUI _encouragingtext;
    Coroutine coroutine;
    private void OnEnable()
    {
        _encouragingtext.text = _Phrases[Random.Range(0, (_Phrases.Length - 1))];
        coroutine = StartCoroutine("Animate");
        Invoke("DisableText", 1);
    }


    IEnumerator Animate()
    {
       while(true)
        {
            while(transform.localScale.x < 1.4f)
            {
                transform.localScale += (Vector3.one * Time.deltaTime * 3);
                yield return new WaitForEndOfFrame();
            }
            while (transform.localScale.x > 0.6f)
            {
                transform.localScale -= (Vector3.one * Time.deltaTime * 3);
                yield return new WaitForEndOfFrame();
            }
        }
    }

    void DisableText()
    {
        StopCoroutine(coroutine);
        gameObject.SetActive(false);
    }
}
