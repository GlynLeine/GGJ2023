using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class portalDoorBehaviour : MonoBehaviour
{
    Animator dAnim;
    AudioSource dAudio;



    private void Start()
    {
        dAnim = GetComponent<Animator>();
        dAudio = GetComponent<AudioSource>();
    }

    public void openDoor()              //when invoked, the components to play the animation and sound to open the door
    {
            dAnim.SetTrigger("tAnimDoorOpen");
            if (dAudio.clip != null)
            {
                dAudio.Play();
            }
    }

    public void closeDoor()
    {
        dAnim.SetTrigger("");
    }










}
