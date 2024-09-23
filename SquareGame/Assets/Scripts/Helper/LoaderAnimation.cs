using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LoaderAnimation : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine("RotateLoaderAnim");
    }

    IEnumerator RotateLoaderAnim()
    {
        while(true)
        {
            transform.Rotate(0, 0,  -(Time.deltaTime * 50)  );
            yield return null;
        }
    }
}
