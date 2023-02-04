using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShipController : MonoBehaviour
{
    [SerializeField]
    private GameObject m_bullet;
    [SerializeField]
    private float m_fwdThrust = 2f;
    [SerializeField]
    private float m_rotThrust = 2f;

    private Rigidbody2D m_rb;

    void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        var fwd = Player.instance.move.y;
        var lat = Player.instance.move.x;
        m_rb.AddForce(transform.up * fwd * m_fwdThrust * Time.deltaTime, ForceMode2D.Impulse);
        m_rb.AddForce(transform.right * lat * m_rotThrust * Time.deltaTime, ForceMode2D.Impulse);
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            var bullet = Instantiate(m_bullet);
            bullet.transform.position = transform.position + transform.up;
            bullet.transform.up = transform.up;
            bullet.GetComponent<Rigidbody2D>().AddForce(bullet.transform.up * 10f, ForceMode2D.Impulse);
        }
    }
}
