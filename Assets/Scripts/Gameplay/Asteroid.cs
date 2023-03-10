using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> m_asteroidSprites = new List<Sprite>();
    [SerializeField]
    private GameObject m_debrisPrefab;

    private SpriteRenderer m_renderer;

    static int astroidCount = 0;

    void Start()
    {
        astroidCount++;
        m_renderer = GetComponent<SpriteRenderer>();
        m_renderer.sprite = m_asteroidSprites[Random.Range(0, m_asteroidSprites.Count - 1)];
    }

    private void Update()
    {
        Vector3 cameraPos = Camera.main.transform.position;
        cameraPos.z = 0;
        Vector3 fromCamera = transform.position - cameraPos;
        fromCamera.z = 0;
        //orthographicSize = half the size of the height of the screen. That is why we * it by 2
        float ySize = Camera.main.orthographicSize * 1.5f;
        //width of the camera depends on the acpect ration and the height
        Vector2 boxColliderSize = new Vector2(ySize * Camera.main.aspect, ySize);
        if (Mathf.Abs(fromCamera.x) > boxColliderSize.x)
        {
            fromCamera.x *= -1f;
        }

        if(Mathf.Abs(fromCamera.y) > boxColliderSize.y){
            fromCamera.y *= -1f;
        }

        transform.position = cameraPos + fromCamera;
    }

    private void OnDestroy()
    {
        astroidCount--;
        if (astroidCount <= 0)
        {
            SceneManager.LoadScene(5);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.transform.name.Contains("Bullet"))
        {
            int num = Random.Range(2, 5);
            for (int i = 0; i < num; i++)
            {
                var go = Instantiate(m_debrisPrefab);
                go.transform.position = transform.position;
                go.transform.localScale = Vector3.one * .2f;
                go.GetComponent<SpriteRenderer>().sprite = m_asteroidSprites[Random.Range(0, m_asteroidSprites.Count - 1)];
                var rb = go.GetComponent<Rigidbody2D>();
                rb.AddForce(Random.onUnitSphere * 10f, ForceMode2D.Impulse);
            }
            AsteroidSpawner.m_score += 100f;
            Destroy(collision.collider.transform.gameObject);
            Destroy(gameObject);
        }
    }
}
