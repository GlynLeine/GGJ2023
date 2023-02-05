using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Portal currentPortal = null;

    public int references = 0;

    private Vector3 m_prevPos;
    private Vector3 m_dir;

    public Vector3 dir { get { return m_dir; } }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.Normalize(m_dir));

        if (currentPortal == null) { return; }
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, currentPortal.transform.position);
    }

    private void LateUpdate()
    {
        m_dir = transform.position - m_prevPos;
        m_prevPos = transform.position;

        if (currentPortal == null) { return; }

        Vector3 pos = transform.position;

        Transform portalTransform = currentPortal.transform;

        Vector3 portalNormal = -portalTransform.forward;

        float dist = Vector3.Dot(pos - portalTransform.position, portalNormal);

        if (dist > 0.21 ) { return; }

        currentPortal.Teleport(gameObject);
    }
}
