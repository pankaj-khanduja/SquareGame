using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CloseScript : MonoBehaviour
{
    public TextMeshProUGUI customText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Updatetext(string msg)
    {
        customText.text = msg;
    }

    public void closeScreen()
    {
        Destroy(this.gameObject);
    }
}
