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
        transform.Rotate(0, 0, Player.instance.move.x * _tiltAmount);
    }
}
