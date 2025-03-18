using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Player : CharacterMovement
{
    // Input references
    private InputAction moveAction;
    private InputAction jumpAction;
    private float MAX_JUMP_TIME = 0.2f;
    private float jumpCounter = 0;
    
    // Camera reference for movement relative to camera
        
    private Transform cameraTransform;
    
    protected override void Start()
    {
        base.Start();
        
        // Get the PlayerInput component
        PlayerInput playerInput = GetComponent<PlayerInput>();
        
        // Get the move and jump actions
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        
        // Set up camera reference
        if(Camera.main == null || Camera.main.transform == null) Debug.LogError("No camera found. Player movement will be in world space.");
        else cameraTransform = Camera.main.transform;
    }
    
    protected override void FixedUpdate()
    {
        // Process input
        ProcessMoveInput();

        // Start Jumping
        if (Input.GetButton("Jump") && isGrounded) 
        {
            jumpPressed = true;
            jumpCounter = 0; // let player jump
        }
        else if(!Input.GetButton("Jump") && !isGrounded) 
        {
            jumpPressed = false; // player let go of jump key
        }

        // Continue Jumping
        if (jumpPressed && jumpCounter < MAX_JUMP_TIME) 
        {
            jumpCounter += Time.fixedDeltaTime; // player is still holding jump key, allow for more jump
            Jump(); // CharacterMovement.cs will apply force upward
        }
        
        // Let the base class handle movement and animation
        base.FixedUpdate();
    }
    
    private void ProcessMoveInput()
    {
        // Get move input from action
        Vector2 input = moveAction.ReadValue<Vector2>();
        
        // Convert input to 3D movement vector
        Vector3 move = new Vector3(input.x, 0, input.y);
        
        if (move.magnitude > 0.1f)
        {
            // If we have a camera, make movement relative to camera angle
            if (cameraTransform != null)
            {
                // Calculate camera forward and right vectors projected onto XZ plane
                Vector3 forward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
                Vector3 right = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;
                
                // Recalculate move direction relative to camera
                move = forward * input.y + right * input.x;
            }
            
            // Set the movement direction
            SetMovementDirection(move);
        }
        else
        {
            // No input, stop movement
            StopMovement();
        }
    }
}