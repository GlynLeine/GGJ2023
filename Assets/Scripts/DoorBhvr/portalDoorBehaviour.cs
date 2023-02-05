using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class portalDoorBehaviour : MonoBehaviour
{
    Animator dAnim;
    AudioSource dAudio;
    public GameObject linkedButton;


    private void Start()
    {
        dAnim = GetComponent<Animator>();
        dAudio = GetComponent<AudioSource>();
        
    }

    public void handleDoor()
    {
        if(dAnim.GetBool("tAnimIsOpen") == false)
        {
            openDoor();
            dAnim.SetBool("tAnimIsOpen", true);
        }
        else
        {
            closeDoor();
            dAnim.SetBool("tAnimIsOpen", false);
        }
    }

    void openDoor()              
    {           
            dAnim.SetTrigger("tAnimDoorOpen");
            if (dAudio.clip != null)
            {
                dAudio.Play();
            }
    }

    void closeDoor()
    {
        dAnim.SetBool("tAnimIsOpen", false);
        dAnim.SetTrigger("tAnimDoorOpen");
        if (dAudio.clip != null)
        {
            dAudio.Play();
        }
    }

    










}
