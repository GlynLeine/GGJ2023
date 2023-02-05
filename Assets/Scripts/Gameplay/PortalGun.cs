using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PortalGun : MonoBehaviour
{
    [SerializeField]
    private LayerMask m_canHit;
    [SerializeField]
    private GameObject m_leftPortalPrefab;
    [SerializeField]
    private GameObject m_rightPortalPrefab;

    [SerializeField]
    private Image m_leftPortalUI;
    [SerializeField]
    private Image m_rightPortalUI;

    private Portal m_leftPortal;
    private Portal m_rightPortal;

    private void OnEnable()
    {
        Player.instance.onLeftClick += OnLeftClick;
        Player.instance.onRightClick += OnRightClick;
    }

    private void OnDisable()
    {
        //Player.instance.onLeftClick -= OnLeftClick;
        //Player.instance.onRightClick -= OnRightClick;
    }

    void Start()
    {

    }

    void Update()
    {
        if (m_rightPortalUI)
        {
            m_rightPortalUI.color = m_rightPortal ? Color.green : Color.white;
        }

        if (m_leftPortalUI)
        {
            m_leftPortalUI.color = m_leftPortal ? Color.red : Color.white;
        }
    }

    void OnLeftClick(InputValue value)
    {
        if (value.isPressed)
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, Mathf.Infinity, m_canHit))
            {
                if (m_leftPortal != null)
                {
                    MovePortal(hitInfo, m_leftPortal.gameObject);
                }
                else
                {
                    m_leftPortal = ShootPortal(hitInfo, m_leftPortalPrefab).GetComponent<Portal>();
                    PortalManager.portals.Add(m_leftPortal);

                    if (m_rightPortal != null)
                    {
                        m_leftPortal.LinkPortal(m_rightPortal);
                    }
                }
            }
        }
    }

    void OnRightClick(InputValue value)
    {
        if (value.isPressed)
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, Mathf.Infinity, m_canHit))
            {
                if (m_rightPortal != null)
                {
                    MovePortal(hitInfo, m_rightPortal.gameObject);
                }
                else
                {
                    m_rightPortal = ShootPortal(hitInfo, m_rightPortalPrefab).GetComponent<Portal>();
                    PortalManager.portals.Add(m_rightPortal);

                    if (m_leftPortal != null)
                    {
                        m_rightPortal.LinkPortal(m_leftPortal);
                    }
                }
            }
        }
    }

    void MovePortal(RaycastHit hitInfo, GameObject existingPortal)
    {
        existingPortal.transform.position = hitInfo.point + (hitInfo.normal * .1f);
        existingPortal.transform.forward = -hitInfo.normal;
        existingPortal.GetComponent<Portal>().CheckWall();
    }

    GameObject ShootPortal(RaycastHit hitInfo, GameObject portalPrefab)
    {
        GameObject portal = Instantiate(portalPrefab);
        portal.transform.position = hitInfo.point + (hitInfo.normal * .1f);
        portal.transform.forward = -hitInfo.normal;
        portal.GetComponent<Portal>().CheckWall();
        return portal;
    }
}
