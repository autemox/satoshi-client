using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

[RequireComponent(typeof(PlayerInput))]
public class ChibMoveInput : NetworkBehaviour
{
    // Input 
    private InputAction moveAction;
    private InputAction jumpAction;
    
    // components
    private IMovement movement;
    
    // Other 
    private Transform cameraTransform;
    protected void Start()
    {
        // get components
        movement = GetComponent<ChibMovement>();
        cameraTransform = Camera.main.transform;
        
        // Get the move and jump actions
        PlayerInput playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        
        // Debug
        if(moveAction == null) Debug.LogError("Move action not found.");
        if(jumpAction == null) Debug.LogError("Jump action not found.");    
        if(movement == null) Debug.LogError("Movement component not found.");
        if(cameraTransform == null) Debug.LogError("Camera transform not found.");
    }
    
    protected void FixedUpdate()
    {
        if(!IsOwner) return;

        // Process input
        ProcessJoystick();

        // Start Jumping
        if (jumpAction.triggered) 
        {
            Debug.Log("Jump action triggered.");
            movement.Jump();
        }
    }
    
    private void ProcessJoystick() // left joystick moves the character
    {
        if(cameraTransform == null) Debug.LogError("No camera found.");
        if(moveAction == null) Debug.LogError("Move action not found.");

        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        
        if (move.magnitude > 0.1f)
        {
            // input detected, set movement direction
            movement.SetMovementDirection(move);
        }
        else
        {
            // No input, stop movement
            movement.StopMovement();
        }
    }
}