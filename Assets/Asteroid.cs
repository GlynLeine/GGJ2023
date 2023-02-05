using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> m_asteroidSprites = new List<Sprite>();
    [SerializeField]
    private GameObject m_debrisPrefab;

    private SpriteRenderer m_renderer;

    void Start()
    {
        m_renderer = GetComponent<SpriteRenderer>();
        m_renderer.sprite = m_asteroidSprites[Random.Range(0, m_asteroidSprites.Count - 1)];
    }

    private void Update()
    {
        var dist = transform.position.magnitude;
        if (dist > Camera.main.orthographicSize * 2.0f)
        {
            transform.position = -(transform.position.normalized * (transform.position.magnitude - 2f));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.transform.name.Contains("Bullet"))
        {
            int num = Random.Range(0, 5);
            for (int i = 0; i < num; i++)
            {
                var go = Instantiate(m_debrisPrefab);
                go.transform.localScale = Vector3.one * .2f;
                go.GetComponent<SpriteRenderer>().sprite = m_asteroidSprites[Random.Range(0, m_asteroidSprites.Count - 1)];
                var rb = go.GetComponent<Rigidbody2D>();
                rb.AddForce(Random.onUnitSphere * 10f, ForceMode2D.Impulse);
            }
        }
    }
}
