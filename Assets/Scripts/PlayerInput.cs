using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    #region INITIALIZE INPUT
        
    //private InputActions _inputActions;
    private InputTest _inputActions;
    private void Awake() => _inputActions = new InputTest();
    private void OnEnable() => _inputActions.Enable();
    private void OnDisable() => _inputActions.Disable();

    #endregion

    #region INPUT VARIABLES
        
    public Vector2 MoveVector { get; private set; }
    public Vector2 LookVector { get; private set; }
        
    public float JumpValue { get; private set; }
    public bool JumpHeld { get; private set; }
    public bool JumpReleased { get; private set; }
    public bool JumpPressed { get; private set; }

    public InputAction Jump { get; private set; }
        
    public float InteractValue { get; private set; }
    public bool Interact { get; private set; }

    #endregion
        
    private void Update()
    {
        MoveVector = _inputActions.Player.Move.ReadValue<Vector2>();

        JumpValue = _inputActions.Player.Jump.ReadValue<float>();
        JumpHeld = _inputActions.Player.Jump.inProgress;
        JumpPressed = _inputActions.Player.Jump.triggered;
        JumpReleased = _inputActions.Player.Jump.WasReleasedThisFrame();

        Jump = _inputActions.Player.Jump;
    } 
    
}
