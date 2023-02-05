using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class QuakeCamera : MonoBehaviour
{
    public float leanAngle = 5f;
    float rate = 2.0f;

    void Update()
    {
        var x = Player.instance.move.x;

        //Quaternion.Lerp(transform.localRotation, Quaternion.AngleAxis(x * -leanAngle, transform.parent.forward), Time.deltaTime * rate);
        //transform.forward = fwd;
        //transform.right = right;
        //transform.localRotation = ;

        // = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, x * -leanAngle), Time.deltaTime * rate);
    }

}
