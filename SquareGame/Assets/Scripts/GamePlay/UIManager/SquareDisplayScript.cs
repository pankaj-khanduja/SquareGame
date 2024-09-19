using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SquareDisplayScript : MonoBehaviour
{
    public TextMeshProUGUI _squareBoardText;
    // Start is called before the first frame update
    void Awake()
    {
       
    }

    private void OnEnable()
    {
        _squareBoardText = GetComponent<TextMeshProUGUI>();
        
    }

    private void Update()
    {
        if(SquareController.Instance != null)
        {
            _squareBoardText.text = "Square On Screen : " + SquareController.Instance.GetSquareList.Count;
        }
    }


}
