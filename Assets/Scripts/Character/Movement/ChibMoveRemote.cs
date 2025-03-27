using UnityEngine;
using Unity.Netcode;

public class ChibMoveRemote : MonoBehaviour
{
    [SerializeField] private float arrivalDistance = 0.1f; // When to stop moving
    
    // components
    private ChibMovement movement;
    
    private void Start()
    {
        movement = GetComponent<ChibMovement>();

        if(movement == null) Debug.LogError("Movement component not found.");
    }
    
    private void Update() 
    {
        if (movement.IsOwner) return;
        
        // Calculate direction to position actual
        Vector3 moveDirection = movement.Position.Value - transform.position;
        moveDirection.y = 0; // Keep movement horizontal
        float distance = moveDirection.magnitude;
        
        if (distance > arrivalDistance) {
            
            // move toward position actual
            //movement.SetMovementDirection(moveDirection.normalized);
        } 
        else 
        {
            // close enough
            movement.StopMovement();
        }
    }
}