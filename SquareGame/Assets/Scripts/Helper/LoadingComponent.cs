using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingComponent : MonoBehaviour
{
    public GameObject LoadingCanvasPrefab;

    public static LoadingComponent instance = null;

    GameObject LoaderObj = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            return;
        }
        Destroy(this.gameObject);
    }

    public void Enableloader()
    {
        LoaderObj = Instantiate(LoadingCanvasPrefab);
    }

    public void DisableLoader()
    {
        if (LoaderObj != null)
            Destroy(LoaderObj);
    }
}
