using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Portal : MonoBehaviour
{
    static readonly int MainTexture = Shader.PropertyToID("_BaseMap");

    static int noCollisionLayer;
    static int defaultLayer;

    [SerializeField]
    private Portal m_linkedPortal;
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
        Vector3 posA = transform.position;
        quaternion rotA = transform.rotation;
        Matrix4x4 toPortal = Matrix4x4.Inverse(Matrix4x4.TRS(posA, rotA, Vector3.one));


        Vector3 posB = m_linkedPortal.transform.position;
        quaternion rotB = quaternion.AxisAngle(Vector3.up, Mathf.PI) * m_linkedPortal.transform.rotation;
        Matrix4x4 fromPortal = Matrix4x4.TRS(posB, rotB, Vector3.one);

        Matrix4x4 reflectionMatrix = fromPortal * toPortal;

        obj.transform.position = reflectionMatrix.MultiplyPoint(obj.transform.position);
        obj.transform.rotation = reflectionMatrix.rotation * obj.transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter " + gameObject.name  + ": " + other.gameObject.name);
        if (m_linkedPortal == null) { return; }

        Teleporter teleporter = other.GetComponent<Teleporter>();
        if (teleporter == null)
        {
            teleporter = other.AddComponent<Teleporter>();
            teleporter.currentPortal = this;
            teleporter.references = 1;
            other.gameObject.layer = noCollisionLayer;
            return;
        }

        if (teleporter.currentPortal == this || teleporter.currentPortal == m_linkedPortal)
        {
            teleporter.currentPortal = this;
            teleporter.references++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exit " + gameObject.name  + ": " + other.gameObject.name);
        if (m_linkedPortal == null) { return; }

        Teleporter teleporter = other.GetComponent<Teleporter>();
        if (teleporter != null)
        {
            if (teleporter.currentPortal == this || teleporter.currentPortal == m_linkedPortal)
            {
                if (--teleporter.references <= 0)
                {
                    Destroy(teleporter);
                    other.gameObject.layer = defaultLayer;
                }
            }
        }
    }
}
