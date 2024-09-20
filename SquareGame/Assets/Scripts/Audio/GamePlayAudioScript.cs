using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SquareOne;
public class GamePlayAudioScript : MonoBehaviour
{
    public AudioClip SoloBgMusic, MultiBgMusic;
    // Start is called before the first frame update
    void Start()
    {
        if (Constant.isPlayingMulti)
            GetComponent<AudioSource>().clip = MultiBgMusic;
        else
            GetComponent<AudioSource>().clip = SoloBgMusic;

        GetComponent<AudioSource>().Play();

    }

    
}
