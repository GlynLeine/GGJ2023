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

    [SerializeField]
    private Sprite m_thrustSprite;
    [SerializeField]
    private Sprite m_idleSprite;

    private Rigidbody2D m_rb;
    private SpriteRenderer m_renderer;

    void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        var fwd = Mathf.Clamp01(Player.instance.move.y);
        var lat = Player.instance.move.x;

        if (fwd > 0)
            m_renderer.sprite = m_thrustSprite;
        else
            m_renderer.sprite = m_idleSprite;

        m_rb.AddForce(transform.right * fwd * m_fwdThrust * Time.deltaTime, ForceMode2D.Impulse);
        m_rb.AddTorque(-lat * m_rotThrust * Time.deltaTime, ForceMode2D.Impulse);

        var dist = transform.position.magnitude;
        if (dist > Camera.main.orthographicSize * 2.0f)
        {
            transform.position = -(transform.position.normalized * (transform.position.magnitude - 2f));
        }
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            var bullet = Instantiate(m_bullet);
            bullet.transform.position = transform.position + transform.right;
            bullet.transform.up = transform.right;
            bullet.GetComponent<Rigidbody2D>().AddForce(bullet.transform.up * 10f, ForceMode2D.Impulse);
        }
    }
}
