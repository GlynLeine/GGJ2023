using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    private float m_damage = 10f;

    private void OnEnable()
    {
        Player.instance.onLeftClick += OnLeftClick;
    }

    void OnLeftClick(InputValue value)
    {
        if (value.isPressed)
        {
            RaycastHit hitInfo;

            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, Mathf.Infinity, Physics.AllLayers))
            {
                var enemy = hitInfo.collider.GetComponent<RumbleEnemy>();
                if (enemy != null)
                {
                    enemy.Damage(m_damage);
                }
            }
        }
    }
}
