using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Portal currentPortal = null;

    public int references = 0;

    private Vector3 m_prevPos;

    public Vector3 direction
    {
        get
        {
            Vector3 diff = transform.position - m_prevPos;
            float sqrDist = diff.sqrMagnitude;
            if (sqrDist < float.Epsilon)
                return diff;

            return diff / Mathf.Sqrt(sqrDist);
        }
    }

    private void Update()
    {
        Transform portalTransform = currentPortal.transform;
        Vector3 pos = transform.position;

        Vector3 portalNormal = -portalTransform.forward;

        float dist = Vector3.Dot(pos - portalTransform.position, portalNormal);
        float dir = -Vector3.Dot(transform.position - m_prevPos, portalNormal);

        Debug.Log("[" + dist + "], [" + dir + "], " + currentPortal.gameObject.name);
        if (dist > 0f || dir <= float.Epsilon) { return; }

        currentPortal.Teleport(gameObject);
    }

    private void LateUpdate()
    {
        m_prevPos = transform.position;
    }
}
