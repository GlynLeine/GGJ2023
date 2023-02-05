using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject m_asteroid;

    private void Start()
    {
        int num = Random.Range(7, 15);
        for (int i = 0; i < num; i++)
        {
            var go = Instantiate(m_asteroid);
            go.transform.position = Random.insideUnitCircle * Camera.main.orthographicSize;
            go.GetComponent<Rigidbody2D>().AddForce((go.transform.position - transform.position).normalized * 5f, ForceMode2D.Impulse);
            go.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-20f, 20f));
        }
    }

    void Update()
    {

    }
}
