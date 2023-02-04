using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEditor;
using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> m_viewBounds = new List<GameObject>();
    [SerializeField]
    private Transform m_target;
    [SerializeField]
    private float m_vSmoothTime = .2f;
    [SerializeField]
    private float m_hSmoothTime = .2f;
    [SerializeField]
    private float m_snapRadius = .001f;

    private bool m_verticalScroll = true;
    private bool m_horizontalScroll = true;

    void Start()
    {

    }

    void FixedUpdate()
    {
        float height = 2f * Camera.main.orthographicSize;
        float width = height * Camera.main.aspect;

        //m_horizontalScroll = true;
        //foreach (GameObject bound in m_viewBounds)
        //{
        //    var xDist = Mathf.Abs(bound.transform.position.x - m_target.position.x);
        //    if (xDist < width)
        //    {
        //        m_horizontalScroll = false;
        //        break;
        //    }
        //}

        //m_verticalScroll = true;
        //foreach (GameObject bound in m_viewBounds)
        //{
        //    var yDist = Mathf.Abs(bound.transform.position.y - m_target.position.y);
        //    if (yDist < height)
        //    {
        //        m_verticalScroll = false;
        //        break;
        //    }
        //}

        var pos = transform.position;
        if (m_verticalScroll)
        {
            pos.x = Mathf.Lerp(pos.x, m_target.position.x, Time.deltaTime * m_vSmoothTime);
            if ((m_target.position.x - pos.x) < m_snapRadius)
            {
                pos.x = m_target.position.x;
            }
        }

        if (m_horizontalScroll)
        {
            pos.y = Mathf.Lerp(pos.y, m_target.position.y, Time.deltaTime * m_hSmoothTime);
            if ((m_target.position.y - pos.y) < m_snapRadius)
            {
                pos.y = m_target.position.y;
            }
        }
        transform.position = pos;
    }

    private void OnDrawGizmos()
    {
        //foreach (GameObject bound in m_viewBounds)
        //{
        //    var xDist = Mathf.Abs(bound.transform.position.x - m_target.position.x);
        //    Gizmos.color = Vector3.Distance(bound.transform.position, m_target.position) < (Camera.main.orthographicSize*2f) ? Color.green : Color.red;
        //    Gizmos.DrawLine(m_target.position, bound.transform.position);
        //    Handles.Label(m_target.position + ((bound.transform.position - m_target.position).normalized), xDist.ToString());
        //}
    }
}
