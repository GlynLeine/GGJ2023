using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class portalDoorBehaviour : MonoBehaviour
{
    Animator dAnim;
    AudioSource dAudio;

    [SerializeField]
    bool linked = false;
    public GameObject linkedButton;


    private void Start()
    {
        dAnim = GetComponent<Animator>();
        dAudio = GetComponent<AudioSource>();
        
    }

    public void openDoor()              
    {           
        dAnim.SetTrigger("tAnimDoorOpen");
        if (dAudio.clip != null)
        {
            dAudio.Play();
        }
    }

    public void openDoorWithButton()
    {
           if (linkedButton.GetComponent<buttonBehaviour>().inTrigger == true)
            {
            dAnim.SetBool("tAnimIsOpen", false);
            openDoor();
            }
            else
            {
            dAnim.SetBool("tAnimIsOpen", true);
            openDoor();
            }
    }

    










}
