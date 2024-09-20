using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    public AudioClip[] _Clips;
    // Start is called before the first frame update
    void Start()
    {

        if (SquareController.Instance.PlayerIQScore == 0) return;
        int randomIndex = Random.Range(0, _Clips.Length - 1);
        GetComponent<AudioSource>().clip = _Clips[randomIndex];
        GetComponent<AudioSource>().Play();
    }

    
}
