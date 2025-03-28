using UnityEngine;
using Unity.Netcode;

public class ChibMoveRemote : NetworkBehaviour
{
    [SerializeField] private float arrivalDistance = 0.1f; // When to stop moving
    
    // components
    private ChibMovement movement;
    private ChibNetwork network;
    
    private void Start()
    {
        movement = GetComponent<ChibMovement>();
        network = GetComponent<ChibNetwork>();

        if(movement == null) Debug.LogError("Movement component not found.");
        if(network == null) Debug.LogError("Network component not found.");
    }
    
    public override void OnNetworkSpawn()
    {
        // check if you are the owner of this object
        if (IsOwner) enabled = false;

        base.OnNetworkSpawn();
    }

    private void Update() 
    {
        if (IsOwner) return;
        
        // Calculate direction to position actual
        Vector3 moveDirection = network.Position.Value - transform.position;
        moveDirection.y = 0; // Keep movement horizontal
        float distance = moveDirection.magnitude;
        
        if (distance > arrivalDistance) {
            
            // move toward position actual
            movement.SetMovementDirection(moveDirection.normalized);
        } 
        else 
        {
            // close enough
            movement.StopMovement();
        }
    }
}