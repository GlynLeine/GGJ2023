using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitToDie : MonoBehaviour
{
    [SerializeField]
    private float m_time = 5f;

    private void Awake()
    {
        StartCoroutine(WaitToDestroy());
    }

    private IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(m_time);
        Destroy(gameObject);
    }
}
