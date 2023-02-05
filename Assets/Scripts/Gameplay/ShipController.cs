using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ShipController : MonoBehaviour
{
    [SerializeField]
    private ScreenBounds m_screenBounds;
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

    [SerializeField]
    private AsteroidSpawner m_spawner;

    private Rigidbody2D m_rb;
    private SpriteRenderer m_renderer;

    public static float m_health = 3f;

    void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_renderer = GetComponent<SpriteRenderer>();
        GetComponent<Player>().isShip = true;
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

        if (m_screenBounds.AmIOutOfBounds(transform.position))
        {
            Vector3 newPos = m_screenBounds.CalculateWrappedPosition(transform.position);
            transform.position = newPos;
        }
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            var bullet = Instantiate(m_bullet);
            bullet.transform.position = transform.position + (transform.right * .2f);
            bullet.transform.up = transform.right;
            bullet.GetComponent<Rigidbody2D>().AddForce(bullet.transform.up * 10f, ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.transform.gameObject.name.Contains("Asteroid"))
        {
            if (m_health >= 0)
            {
                m_health -= 1;

                transform.position = Vector2.zero;
                m_rb.velocity = Vector3.zero;
                m_spawner.ResetLevel();
                AsteroidSpawner.m_score = 0;
            }
            else
            {
                //transition out
                SceneManager.LoadScene(5);
            }
        }
    }
}
