using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ElevatorBeanLift : MonoBehaviour
{
    RaycastHit liftData;
    

    void Start()
    {
        
    }

    void Update()
    {  
        getFocusedButton();
    }

    void getFocusedButton()
    {
        if(Keyboard.current.eKey.isPressed)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            Physics.Raycast(ray, out liftData, 2);
            Debug.DrawRay(transform.position, transform.forward, Color.green);
            GameObject focusButton = liftData.collider.gameObject;
            if (focusButton.tag == "LiftButton")
            {
                focusButton.GetComponent<liftButtonBHVR>().handlePress(); ; //.dothethingie
            }
        }


        

    }
}
