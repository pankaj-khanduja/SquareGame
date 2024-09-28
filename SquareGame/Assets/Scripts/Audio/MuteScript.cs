using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SquareOne;
public class MuteScript : MonoBehaviour
{
    Button Btn_UnMute;
    public GameObject MuteObj;
    // Start is called before the first frame update
    void Start()
    {
        Btn_UnMute = GetComponent<Button>();
        Btn_UnMute.onClick.AddListener(Btn_UnMuteClciked);
    }

    private void OnEnable()
    {
        CheckMuteSetting();
    }

    void CheckMuteSetting()
    {
        if (Constant.MuteStatus)
        {
            MuteObj.SetActive(true);
            AudioListener.volume = 0;
        }
        else
        {
            MuteObj.SetActive(false);
            AudioListener.volume = 1;
        }
            
    }

    void Btn_UnMuteClciked()
    {
        Constant.MuteStatus = !Constant.MuteStatus;
        CheckMuteSetting();
    }

   
}
