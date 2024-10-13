using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chances : MonoBehaviour
{
    public GameObject prefab;
    Stack<GameObject> lifeObj;
    // Start is called before the first frame update
    void Start()
    {
        OnInitializeLifes();
    }

    private void OnEnable()
    {
        lifeObj = new Stack<GameObject>();
        SquareController.Instance.onPenatltyUpdate += PenaltyPointUpdate;
        SquareController.Instance.onRestart += OnInitializeLifes;

    }

    void OnInitializeLifes()
    {
    
        while (lifeObj.Count > 0)
        {
            GameObject obj = lifeObj.Pop();
            Destroy(obj);
        }
        if (SquareController.Instance.GetManager().NoOfChances > 0)
        {
            for (int count = 0; count < SquareController.Instance.GetManager().NoOfChances; count++)
            {
                GameObject obj = Instantiate(prefab);
                obj.transform.SetParent(this.gameObject.transform, false);
                lifeObj.Push(obj);
            }

        }
    }

    private void OnDisable()
    {
        if (SquareController.Instance != null)
        {
            SquareController.Instance.onPenatltyUpdate -= PenaltyPointUpdate;
            SquareController.Instance.onRestart -= OnInitializeLifes;
        }
      
    }

    void PenaltyPointUpdate()
    {
        if (lifeObj.Count == 0) return;
        GameObject obj =  lifeObj.Pop();
        Destroy(obj);
    }
}
