using System.Collections;
using System.Collections.Generic;
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

    private GameObject m_teleportingPlayer;

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
        if(m_viewTexture == null || m_viewTexture.width != Screen.width || m_viewTexture.height != Screen.height)
        {
            if(m_viewTexture != null)
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void LinkPortal(Portal portal)
    {
        if (m_linkedPortal == null)
        {
            m_linkedPortal = portal;
            portal.m_linkedPortal = this;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(m_linkedPortal == null) { return; }

        if (other.gameObject.tag == "Player")
        {
            m_teleportingPlayer = other.gameObject;
            other.gameObject.layer = noCollisionLayer;
        }
        else if (m_teleportingPlayer != null && other.gameObject.tag == "MainCamera")
        {
            Matrix4x4 reflectionMatrix = m_linkedPortal.transform.localToWorldMatrix * transform.worldToLocalMatrix;

            Transform playerTransform = m_teleportingPlayer.transform;

            playerTransform.position = reflectionMatrix.MultiplyPoint(playerTransform.position);

            FPSController fpsController = m_teleportingPlayer.GetComponent<FPSController>();

            fpsController.mouseLook.SetCharRot(playerTransform.rotation = reflectionMatrix.rotation * playerTransform.rotation);
            Transform cameraTransform = m_teleportingPlayer.GetComponentInChildren<Camera>().transform;
            fpsController.mouseLook.SetCamRot(cameraTransform.rotation = reflectionMatrix.rotation * cameraTransform.rotation);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (m_linkedPortal == null) { return; }

        if (other.gameObject.tag == "Player")
        {
            m_teleportingPlayer = null;
            other.gameObject.layer = defaultLayer;
        }
    }
}
