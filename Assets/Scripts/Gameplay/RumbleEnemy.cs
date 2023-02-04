using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RumbleEnemy : MonoBehaviour
{
    public float m_health = 100f;

    public void Damage(float dmg)
    {
        m_health -= dmg;

        if(m_health < 0f)
        {
            Death();
        }
    }

    void Death()
    {
        Destroy(gameObject);
    }
}
