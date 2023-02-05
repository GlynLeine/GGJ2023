using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject m_asteroid;

    private List<GameObject> m_spawned = new List<GameObject>();

    public static float m_score = 0;

    private void Start()
    {
        ResetLevel();
    }

    public void ResetLevel()
    {
        for (int i = 0; i < m_spawned.Count; i++)
        {
            Destroy(m_spawned[i]);
        }

        m_spawned.Clear();

        int num = Random.Range(10, 20);
        for (int i = 0; i < num; i++)
        {
            var go = Instantiate(m_asteroid);
            var rand = Random.onUnitSphere;
            rand = Camera.main.transform.position + rand + (rand * Camera.main.orthographicSize);
            rand.z = 0;
            go.transform.position = rand;
            go.GetComponent<Rigidbody2D>().AddForce((go.transform.position - transform.position).normalized * 5f, ForceMode2D.Impulse);
            go.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-20f, 20f));
            m_spawned.Add(go);
        }
    }
}
