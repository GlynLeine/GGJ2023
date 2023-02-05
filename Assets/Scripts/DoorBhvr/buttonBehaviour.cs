using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class buttonBehaviour : MonoBehaviour
{
    public UnityEvent invokeMethodActivate;
    public UnityEvent invokeMethodDeactivate;
    string buttonTriggerTag = "ButtonActivator";


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == buttonTriggerTag)
        {
            invokeMethodActivate.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == buttonTriggerTag)
        {
            invokeMethodDeactivate.Invoke();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == buttonTriggerTag)
        {
           // inTrigger = true;
        }          
    }



}
