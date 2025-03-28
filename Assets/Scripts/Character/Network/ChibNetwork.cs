using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(ChibMovement))]
public class ChibNetwork : NetworkBehaviour
{
    private ChibMovement movement; // Reference to the movement component

    // Network position for syncing
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>(
        writePerm: NetworkVariableWritePermission.Owner);

    private void Start()
    {
        movement = GetComponent<ChibMovement>();
        if (movement == null) Debug.LogError("ChibMovement component required for ChibNetwork");
    }

    private void FixedUpdate()
    {
        // Update network position if this is the owner and it's moved
        if (IsOwner && Vector3.Distance(transform.position, Position.Value) > 0.01f)
            Position.Value = transform.position;
            
        // If not the owner, update position based on network data
        if (!IsOwner)
            transform.position = Position.Value;
    }
}