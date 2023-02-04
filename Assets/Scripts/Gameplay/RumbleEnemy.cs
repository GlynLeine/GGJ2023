using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RumbleEnemy : MonoBehaviour
{
    [SerializeField]
    private float m_health = 100f;

    private int m_invincibilityFrames = 10;
    private int m_counter = 0;

    private Color m_savedColor;

    private void Start()
    {
        m_savedColor = GetComponent<MeshRenderer>().sharedMaterial.color;
    }

    private void Update()
    {
        if (m_counter > 0)
        {
            m_counter--;
            GetComponent<MeshRenderer>().sharedMaterial.color = Color.red;
        }
        else
        {
            GetComponent<MeshRenderer>().sharedMaterial.color = m_savedColor;
        }
    }

    public void Damage(float dmg)
    {
        if (m_counter <= 0)
        {
            m_health -= dmg;
            m_counter = m_invincibilityFrames;

            if (m_health < 0f)
            {
                Death();
            }
        }
    }

    void Death()
    {
        Destroy(gameObject);
    }
}
