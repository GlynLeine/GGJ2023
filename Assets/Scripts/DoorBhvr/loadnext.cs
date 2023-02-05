using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadnext : MonoBehaviour
{
    public int nextSceneIndex;

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        loadNextScene();
    }

    void loadNextScene()
    {
        SceneManager.LoadScene(nextSceneIndex);
    }



}
