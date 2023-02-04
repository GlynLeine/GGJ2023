using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PortalGun : MonoBehaviour
{
    private void OnEnable()
    {
        Player.instance.onInteract += OnInteract;
    }

    private void OnDisable()
    {
        Player.instance.onInteract -= OnInteract;
    }

    void Start()
    {

    }

    void Update()
    {

    }

    void OnInteract(InputValue value)
    {
    }
}
