using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Goomba : MonoBehaviour
{
    private float m_speed = 5f;
    private Vector2 m_direction;
    private Rigidbody2D m_rb;

    private void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_direction = -transform.right;
    }

    public void FixedUpdate()
    {
        m_rb.velocity = m_direction * m_speed;

        RaycastHit2D hitInfo = Physics2D.BoxCast(transform.position, new Vector2(.1f, .1f), 0, m_direction);
        if(hitInfo) 
        {
            m_direction = -m_direction;
        }
    }

    public void Kill()
    {
        Destroy(gameObject);
    }
}
