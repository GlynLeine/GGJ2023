using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.PlayerSettings;

public class Portal : MonoBehaviour
{
    static readonly int MainTexture = Shader.PropertyToID("_BaseMap");

    static int noCollisionLayer;
    static int defaultLayer;

    [SerializeField]
    private Portal m_linkedPortal;

    public Portal linkedPortal { get { return m_linkedPortal; } }

    [SerializeField]
    private MeshRenderer m_screen;

    private Camera m_playerCam;
    private Camera m_portalCam;
    private RenderTexture m_viewTexture;

    private void Awake()
    {
        noCollisionLayer = LayerMask.NameToLayer("No Wall Collision");
        defaultLayer = LayerMask.NameToLayer("Default");
        m_playerCam = Camera.main;
        m_portalCam = GetComponentInChildren<Camera>();
        m_portalCam.enabled = false;
    }

    void CreateViewTexture()
    {
        if (m_viewTexture == null || m_viewTexture.width != Screen.width || m_viewTexture.height != Screen.height)
        {
            if (m_viewTexture != null)
            {
                m_viewTexture.Release();
            }

            m_viewTexture = new RenderTexture(Screen.width, Screen.height, 0);
            m_portalCam.targetTexture = m_viewTexture;
            m_linkedPortal.m_screen.material.SetTexture(MainTexture, m_viewTexture);
        }
    }

    public void Render(ScriptableRenderContext context)
    {
        if (m_linkedPortal == null)
            return;

        m_screen.enabled = false;
        CreateViewTexture();
        var m = transform.localToWorldMatrix * m_linkedPortal.transform.worldToLocalMatrix * m_playerCam.transform.localToWorldMatrix;
        m_portalCam.transform.SetPositionAndRotation(m.GetColumn(3), m.rotation);

        m_portalCam.Render();

        m_screen.enabled = true;
    }

    // Update is called once per frame
    public void LinkPortal(Portal portal)
    {
        if (portal != this && m_linkedPortal == null)
        {
            m_linkedPortal = portal;
            portal.m_linkedPortal = this;
        }
    }

    public void Teleport(GameObject obj)
    {
        Teleporter teleporter = obj.GetComponent<Teleporter>();

        if (Vector3.Dot(-transform.forward, teleporter.dir) >= -float.Epsilon) { return; }

        Debug.Log($"Target: {obj.name} From: {gameObject.name} to: {m_linkedPortal.gameObject.name}");

        Vector3 posA = transform.position;
        quaternion rotA = transform.rotation;
        Matrix4x4 toPortal = Matrix4x4.Inverse(Matrix4x4.TRS(posA, rotA, Vector3.one));

        Vector3 posB = m_linkedPortal.transform.position;
        quaternion rotB = quaternion.AxisAngle(m_linkedPortal.transform.up, Mathf.PI) * m_linkedPortal.transform.rotation;
        Matrix4x4 fromPortal = Matrix4x4.TRS(posB, rotB, Vector3.one);

        Matrix4x4 reflectionMatrix = fromPortal * toPortal;

        Vector3 objPos = reflectionMatrix.MultiplyPoint(obj.transform.position);
        Quaternion objRot = reflectionMatrix.rotation * obj.transform.rotation;

        Vector3 normal = -m_linkedPortal.transform.forward;
        float dist = Vector3.Dot(objPos - m_linkedPortal.transform.position, normal);

        objPos += normal * (Mathf.Sign(dist) * (math.abs(dist) + float.Epsilon));

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        obj.transform.position = objPos;
        obj.transform.rotation = objRot;
        rb.position = objPos;
        rb.rotation = objRot;
        teleporter.currentPortal = m_linkedPortal;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position - transform.forward);

        if (linkedPortal != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, linkedPortal.transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_linkedPortal == null) { return; }

        Teleporter teleporter = other.GetComponent<Teleporter>();
        if (teleporter == null) { return; }

        if (teleporter.currentPortal == null)
        {
            teleporter.currentPortal = this;
            teleporter.references = 1;
            other.gameObject.layer = noCollisionLayer;
            return;
        }

        if (teleporter.currentPortal == this || teleporter.currentPortal == m_linkedPortal)
        {
            teleporter.references++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (m_linkedPortal == null) { return; }

        Teleporter teleporter = other.GetComponent<Teleporter>();
        if (teleporter == null) { return; }

        if (teleporter.currentPortal == this || teleporter.currentPortal == m_linkedPortal)
        {
            if (--teleporter.references <= 0)
            {
                teleporter.currentPortal = null;
                other.gameObject.layer = defaultLayer;
            }
        }
    }
}
