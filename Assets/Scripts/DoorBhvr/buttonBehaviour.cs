using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class buttonBehaviour : MonoBehaviour
{
    public UnityEvent invokeMethodActivate;
   // public UnityEvent invokeMethodDeactivate;
    string buttonTriggerTag = "ButtonActivator";
    public bool inTrigger = false;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == buttonTriggerTag)
        {
            inTrigger = true;
            invokeMethodActivate.Invoke();            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == buttonTriggerTag)
        {
            inTrigger = false;
            invokeMethodActivate.Invoke();
        }
    }





}
