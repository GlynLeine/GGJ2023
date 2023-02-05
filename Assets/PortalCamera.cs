using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PortalCamera : MonoBehaviour
{
    public Portal portal;

    void Update()
    {
        if(portal.linkedPortal == null) { return; }

        Transform mainCamTransform = Camera.main.transform;
        Transform portalTransform = portal.transform;
        Transform linkedTransform = portal.linkedPortal.transform;

        Vector3 posA = portalTransform.position;
        quaternion rotA = portalTransform.rotation;
        Matrix4x4 toPortal = Matrix4x4.Inverse(Matrix4x4.TRS(posA, rotA, Vector3.one));

        Vector3 posB = linkedTransform.position;
        quaternion rotB = quaternion.AxisAngle(linkedTransform.up, Mathf.PI) * linkedTransform.rotation;
        Matrix4x4 fromPortal = Matrix4x4.TRS(posB, rotB, Vector3.one);

        Matrix4x4 reflectionMatrix = fromPortal * toPortal;

        Vector3 objPos = reflectionMatrix.MultiplyPoint(mainCamTransform.position);
        Quaternion objRot = reflectionMatrix.rotation * mainCamTransform.rotation;

        transform.position = objPos;
        transform.rotation = objRot;

        float dist = (portalTransform.position - transform.position).magnitude;

        //GetComponent<Camera>().nearClipPlane = dist;
    }
}
