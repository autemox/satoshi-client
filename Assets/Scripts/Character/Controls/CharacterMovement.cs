using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : CharacterAnimate
{
    // Movement properties
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = 20f;
    
    // Movement state
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 velocity = Vector3.zero;
    private bool isGrounded = false;
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
    
    protected virtual void FixedUpdate()
    {
        // Handle gravity and ground detection
        HandleGravity();
        
        // Apply movement
        if (moveDirection.magnitude > 0.1f)
        {
            // Character is moving
            MoveCharacter();
            
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
        }
    }
    
    protected void HandleGravity()
    {
        // Check if character is grounded
        isGrounded = characterController.isGrounded;
        
        // Debug ground detection
        Debug.Log($"Grounded: {isGrounded}, Position Y: {transform.position.y}");
        
        // Apply gravity when not grounded
        if (isGrounded)
        {
            velocity.y = -0.5f; // Small negative value to keep character grounded
        }
        else
        {
            // Increase gravity value if needed
            velocity.y -= gravity * Time.fixedDeltaTime;
            
            // Make sure gravity has enough effect
            Debug.Log($"Applying gravity, current velocity.y: {velocity.y}");
        }
        
        // Apply velocity
        characterController.Move(velocity * Time.fixedDeltaTime);
    }
    
    // Move the character based on moveDirection
    protected void MoveCharacter()
    {
        // Calculate movement vector
        Vector3 movement = moveDirection * moveSpeed * Time.fixedDeltaTime;
        
        // Apply movement
        characterController.Move(movement);
        
        // Adjust rotation to face movement direction
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
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
    
    // Initiate a jump (to be called by derived classes)
    public virtual void Jump()
    {
        if (isGrounded)
        {
            velocity.y = jumpForce;
        }
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
        return moveDirection.magnitude * moveSpeed;
    }

    protected override void Update()
    {
        base.Update();
    }
}