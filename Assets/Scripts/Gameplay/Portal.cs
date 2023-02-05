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
    static readonly int LinkedBool = Shader.PropertyToID("_Linked");

    static int noCollisionLayer;
    static int defaultLayer;
    static int canPortalLayer;

    public LayerMask wallLayerMask;

    [SerializeField]
    private Portal m_linkedPortal;

    [HideInInspector]
    public Portal linkedPortal { get { return m_linkedPortal; } }

    [SerializeField]
    private MeshRenderer m_screen;

    private RenderTexture m_viewTexture;

    public List<GameObject> m_walls = new List<GameObject>();

    private void Awake()
    {
        noCollisionLayer = LayerMask.NameToLayer("No Wall Collision");
        defaultLayer = LayerMask.NameToLayer("Default");
        canPortalLayer = LayerMask.NameToLayer("Can Be Portaled");

        CreateViewTexture();

        if (m_linkedPortal != null)
        {
            m_screen.material.SetInt(LinkedBool, 1);
        }

        CheckWall();
    }

    public void CheckWall()
    {
        foreach (GameObject wall in m_walls)
        {
            wall.layer = canPortalLayer;
        }

        m_walls.Clear();

        Vector3 sphereOffset = new Vector3(0f, 1f, 0f);
        Vector3 pos = transform.position;

        RaycastHit[] hits = Physics.CapsuleCastAll(pos + sphereOffset, pos - sphereOffset, 0.5f, transform.forward, 1f, wallLayerMask);

        foreach (RaycastHit hit in hits)
        {
            m_walls.Add(hit.transform.gameObject);
        }

    }

    void CreateViewTexture()
    {
        if (m_viewTexture == null)
        {
            m_viewTexture = new RenderTexture(Screen.width, Screen.height, 24);
            GetComponentInChildren<Camera>().targetTexture = m_viewTexture;
            m_screen.material.SetTexture(MainTexture, m_viewTexture);
        }
    }

    // Update is called once per frame
    public void LinkPortal(Portal portal)
    {
        if (portal != this && m_linkedPortal == null)
        {
            m_linkedPortal = portal;
            portal.m_linkedPortal = this;

            m_linkedPortal.m_screen.material.SetInt(LinkedBool, 1);
            m_screen.material.SetInt(LinkedBool, 1);
        }
    }

    public void Teleport(GameObject obj)
    {
        Teleporter teleporter = obj.GetComponent<Teleporter>();

        if (Vector3.Dot(-transform.forward, teleporter.dir) >= -float.Epsilon) { return; }

        //Debug.Log($"Target: {obj.name} From: {gameObject.name} to: {m_linkedPortal.gameObject.name}");

        Vector3 posA = transform.position;
        quaternion rotA = transform.rotation;
        Matrix4x4 toPortal = Matrix4x4.Inverse(Matrix4x4.TRS(posA, rotA, Vector3.one));

        Vector3 posB = m_linkedPortal.transform.position;
        quaternion rotB = quaternion.AxisAngle(m_linkedPortal.transform.up, Mathf.PI) * m_linkedPortal.transform.rotation;
        Matrix4x4 fromPortal = Matrix4x4.TRS(posB, rotB, Vector3.one);

        Matrix4x4 reflectionMatrix = fromPortal * toPortal;

        Vector3 objPos = reflectionMatrix.MultiplyPoint(obj.transform.position);
        Quaternion objRot = reflectionMatrix.rotation * obj.transform.rotation;

        Rigidbody rb = obj.GetComponent<Rigidbody>();

        rb.velocity = reflectionMatrix.MultiplyVector(rb.velocity);

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

            foreach (GameObject wall in m_walls)
            {
                wall.layer = noCollisionLayer;
            }
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

                foreach (GameObject wall in m_walls)
                {
                    wall.layer = canPortalLayer;
                }
            }
        }
    }
}
