using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBar : MonoBehaviour
{
    [SerializeField]
    private GameObject m_life1;
    [SerializeField]
    private GameObject m_life2;
    [SerializeField]
    private GameObject m_life3;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(ShipController.m_health < 3)
        {
            Destroy(m_life1);
        }

        if (ShipController.m_health < 2)
        {
            Destroy(m_life2);
        }

        if (ShipController.m_health < 1)
        {
            Destroy(m_life3);
        }

        if (ShipController.m_health < 0)
        {
            Debug.Log("Game Over");
        }
    }
}
