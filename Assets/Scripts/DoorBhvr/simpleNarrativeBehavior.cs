using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class simpleNarrativeBehavior : MonoBehaviour
{
    public UnityEvent methodToCall;
    AudioSource aud;

    private void Start()
    {
        aud = GetComponent<AudioSource>();
    }

    private void Update()
    {

    }

    void onAudioFinished()
    {
        if (aud.isPlaying == false)
        {
            methodToCall.Invoke();
        }
    }

}
