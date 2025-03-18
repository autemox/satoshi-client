using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : CharacterAnimate
{
    // Movement properties
    [Header("Movement Settings")]
    [SerializeField] private float MOVE_SPEED = 5f;
    [SerializeField] private float ROTATION_SPEED = 10f;
    [SerializeField] protected float JUMP_FORCE = 5f;
    [SerializeField] private float GRAVITY = 20f;
    
    // Movement state
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 velocity = Vector3.zero;
    protected bool isGrounded = false;
    protected bool jumpPressed = false;
    private Vector2 lastMovementDirection = Vector2.down; 

    
    // Components
    private CharacterController characterController;
    
    protected override void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (characterController == null) Debug.LogError("CharacterController component required for CharacterMovement");

        base.Start();

        characterController.height = 0.5f;
        characterController.radius = 0.08f;
    }
    
    protected void MoveCharacter()
    {
        // Calculate movement vector (horizontal only)
        Vector3 horizontalMovement = moveDirection * MOVE_SPEED * Time.fixedDeltaTime;
        
        // Combine with vertical velocity for complete movement
        Vector3 finalMovement = new Vector3(horizontalMovement.x, velocity.y * Time.fixedDeltaTime, horizontalMovement.z);
        
        // Apply movement (includes both horizontal and vertical)
        characterController.Move(finalMovement);
        
        // Adjust rotation to face movement direction
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, ROTATION_SPEED * Time.fixedDeltaTime);
        }
    }

    // Modify HandleGravity to not move the character
    protected void HandleGravity()
    {   
        // Apply gravity when not grounded
        if (isGrounded) velocity.y = -0.5f; // keep character grounded
        else velocity.y -= GRAVITY * Time.fixedDeltaTime; // full gravity
    }

    // Modify FixedUpdate to ensure MoveCharacter is always called
    protected virtual void FixedUpdate()
    {
        // Check if character is grounded
        isGrounded = characterController.isGrounded;

        // Handle gravity and ground detection
        if(!jumpPressed) HandleGravity();
        
        // Apply movement (always, not just when horizontal movement exists)
        MoveCharacter();
        
        // Animation logic stays the same
        if (moveDirection.magnitude > 0.1f)
        {
            // Character is moving
            // Set walking animation based on movement direction
            Vector2 moveDir2D = new Vector2(moveDirection.x, moveDirection.z);
            
            // Store this as our last significant movement direction
            lastMovementDirection = moveDir2D;
            
            WalkInDirection(moveDir2D);
        }
        else
        {
            // Character is standing still
            if (isGrounded)
            {
                // Use the stored last movement direction instead of transform.forward
                StandInDirection(lastMovementDirection);
            }
            else
            {
                // Character is falling or jumping
                FallInDirection(lastMovementDirection);
            }
        }
    }
    
    // Set the movement direction (to be called by derived classes)
    public virtual void SetMovementDirection(Vector3 direction)
    {
        // Normalize horizontal components of direction
        Vector3 horizontalDir = new Vector3(direction.x, 0, direction.z);
        
        if (horizontalDir.magnitude > 1f)
            horizontalDir.Normalize();
            
        moveDirection = horizontalDir;
    }
    
    // Initiate or continue a jump
    public virtual void Jump()
    {
        Debug.Log("Jumping");
        jumpPressed = true;
        if (isGrounded) velocity.y = JUMP_FORCE;
    }
    
    // Stop all movement
    public virtual void StopMovement()
    {
        moveDirection = Vector3.zero;
    }
    
    // Check if character is moving
    public bool IsMoving()
    {
        return moveDirection.magnitude > 0.1f;
    }
    
    // Get current movement speed
    public float GetCurrentSpeed()
    {
        return moveDirection.magnitude * MOVE_SPEED;
    }

    protected override void Update()
    {
        base.Update();
    }
}