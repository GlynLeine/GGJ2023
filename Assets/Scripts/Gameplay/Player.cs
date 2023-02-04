using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public delegate void InputAction(InputValue value);
    public InputAction onMove;
    public InputAction onLook;
    public InputAction onSprint;
    public InputAction onJump;
    public InputAction onLeftClick;
    public InputAction onRightClick;
    public InputAction onInteract;
    public InputAction onQ;
    public InputAction onE;


    [Header("Current State")]
    public bool m_inMenu = false;

    [Header("Character Input Values")]
    public Vector2 move;
    public Vector2 look;
    public bool leftClick;
    public bool rightClick;
    public bool sprint;
    public bool jump;

    [Header("Movement Settings")]
    public bool analogMovement;

    [Header("Mouse Look Settings")]
    public bool horizontalLock = false;

    [Header("Mouse Cursor Settings")]
    public bool m_lockCursor = true;
    public bool m_lockHorizontal = false;
    public bool m_lockVertical = false;
    public bool m_stopMove = false;
    public bool m_stopLook = false;

    public Transform m_root;
    public Transform m_hand;


    public static Player instance => FindObjectOfType<Player>();

    public void Update()
    {
        //var mouseLook = GetComponent<FirstPersonController>()?.mouseLook;
        //if (mouseLook == null)
        //    return;
        //mouseLook.SetCursorLock(m_lockCursor);
        //mouseLook.lockVertical = m_lockVertical;
        //mouseLook.lockHorizontal = m_lockHorizontal;
    }

    public void OnMove(InputValue value)
    {
        if (!m_stopMove && !m_inMenu)
            onMove(value);
    }

    public void OnLook(InputValue value)
    {
        if (!m_stopLook && !m_inMenu)
            onLook(value);
    }

    public void OnSprint(InputValue value)
    {
        onSprint(value);
    }

    public void OnJump(InputValue value)
    {
        onJump(value);
    }

    public void OnLeftClick(InputValue value)
    {
        onLeftClick(value);
    }

    public void OnRightClick(InputValue value)
    {
        onRightClick(value);
    }

    public void OnInteract(InputValue value)
    {
        if (value.isPressed)
        {
            onInteract(value);
        }
    }

    private void OnEnable()
    {
        onMove += MoveInput;
        onLook += LookInput;
        onSprint += SprintInput;
        onJump += JumpInput;
        onLeftClick += LeftClickInput;
        onRightClick += RightClickInput;
        onInteract += InteractInput;
    }

    private void OnDisable()
    {
        onMove -= MoveInput;
        onLook -= LookInput;
        onSprint -= SprintInput;
        onJump -= JumpInput;
        onLeftClick -= LeftClickInput;
        onRightClick -= RightClickInput;
        onInteract -= InteractInput;
    }

    public void MoveInput(InputValue value)
    {
        move = value.Get<Vector2>();
    }

    public void LookInput(InputValue value)
    {
        var vec = value.Get<Vector2>();
        vec.y *= -1f;
        look = vec;
    }

    public void SprintInput(InputValue value)
    {
        sprint = value.isPressed;
    }

    public void JumpInput(InputValue value)
    {
        jump = value.isPressed;
    }

    public void LeftClickInput(InputValue value)
    {
    }

    public void RightClickInput(InputValue value)
    {
    }

    public void InteractInput(InputValue value)
    {

    }

    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(m_lockCursor);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
}