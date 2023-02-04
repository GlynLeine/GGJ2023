using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class QuakeCamera : MonoBehaviour
{
    //Editor variables, you can customize these
    public float _tiltAmount = 5;

    // Update is called once per frame
    void Update()
    {
        // If _leftBtn key is hit, rotate Z axis of camera by _tiltAmount
        if (Keyboard.current.aKey.IsPressed())
        {
            this.transform.Rotate(0, 0, _tiltAmount);
        }
        else
        {
            this.transform.Rotate(0, 0, -_tiltAmount);
        }

        // Same as above, but inverted values
        if (Keyboard.current.dKey.IsPressed())
        {
            this.transform.Rotate(0, 0, -_tiltAmount);
        }
        else 
        {
            this.transform.Rotate(0, 0, _tiltAmount);
        }
    }
}
