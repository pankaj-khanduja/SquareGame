using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AlertPanelScript : MonoBehaviour
{
    public Image image_RedAlert;
    float min = 0, max = 0.2f, timePassed = 0;
    int count = 0;
    float alpha = 0;
    private void Update()
    {
        timePassed += (Time.deltaTime * 5);
        alpha = Mathf.Clamp(timePassed, min, max);
        image_RedAlert.color = new Color(image_RedAlert.color.r, image_RedAlert.color.g, image_RedAlert.color.b, alpha);
        if(timePassed > 1)
        {
            count++;
            timePassed = max;
            max = min;
            min = timePassed;
            timePassed = 0;
        }
        if(count == 2)
        {
            Destroy(this.gameObject);
        }
    }
}
