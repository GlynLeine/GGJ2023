using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController2D : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField]
    private float m_jumpForce = 5f;
    [SerializeField]
    private float m_moveSpeed = 50f;
    [SerializeField]
    private float m_linearDrag = 4f;
    [SerializeField]
    private float m_maxSpeed = 7f;
    [SerializeField]
    private Vector2 m_groundCheckDimensions;
    [SerializeField]
    private LayerMask m_platformLayer;
    [SerializeField]
    private LayerMask m_enemyLayer;
    private bool m_facingRight = true;

    private Rigidbody2D m_rb => GetComponent<Rigidbody2D>();
    private Animator m_animator => GetComponent<Animator>();
    private bool m_isGrounded = false;
    private Vector2 m_input;

    private void OnJump()
    {
        if (m_isGrounded)
            m_rb.velocity += Vector2.up * m_jumpForce;
    }

    private void OnMove(InputValue axis)
    {
        m_input = axis.Get<Vector2>();
    }

    private void Update()
    {
        CheckForGround();
        m_animator.SetBool("Grounded", m_isGrounded);
    }

    private void FixedUpdate()
    {
        m_rb.AddForce(Vector2.right * m_input * m_moveSpeed);

        if ((m_input.x > 0 && !m_facingRight) || (m_input.x < 0 && m_facingRight))
        {
            Flip();
        }

        if (Mathf.Abs(m_rb.velocity.x) > m_maxSpeed)
        {
            m_rb.velocity = new Vector2(Mathf.Sign(m_rb.velocity.x) * m_maxSpeed, m_rb.velocity.y);
        }

        m_animator.SetBool("Walk", Mathf.Abs(m_input.x) > 0);

        bool changingDirections = (m_input.x > 0 && m_rb.velocity.x < 0) || (m_input.x < 0 && m_rb.velocity.x > 0);

        if (Mathf.Abs(m_input.x) < 0.4f || changingDirections)
        {
            m_rb.drag = m_linearDrag;
        }
        else
        {
            m_rb.drag = 0f;
        }
    }

    private void CheckForGround()
    {
        m_isGrounded = Physics2D.BoxCast(transform.position, m_groundCheckDimensions, 0f,
                     -transform.up, 0.1f, m_platformLayer);

        RaycastHit2D hitInfo = Physics2D.BoxCast(transform.position, m_groundCheckDimensions, 0f, -transform.up, 0.1f, m_enemyLayer);
        if (hitInfo.collider)
        {
            var goomba = hitInfo.collider.gameObject.GetComponent<Goomba>();
            m_rb.velocity += Vector2.up * m_jumpForce;
            goomba.Kill();
        }
    }

    void Flip()
    {
        m_facingRight = !m_facingRight;
        transform.rotation = Quaternion.Euler(0, m_facingRight ? 0 : 180, 0);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, (Vector3)m_groundCheckDimensions);
    }
}
