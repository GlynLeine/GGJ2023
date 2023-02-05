using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class buttonBehaviour : MonoBehaviour
{
    public UnityEvent invokeMethodActivate;
    public UnityEvent invokeMethodDeactivate;
    string buttonTriggerTag = "ButtonActivator";
    public bool inTrigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == buttonTriggerTag && !inTrigger)
        {
            invokeMethodActivate.Invoke();
            inTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == buttonTriggerTag)
        {
            invokeMethodDeactivate.Invoke();
            inTrigger = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == buttonTriggerTag)
        {
          //  inTrigger = true;
        }          
    }



}
