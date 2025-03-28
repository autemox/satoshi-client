using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChibMovePatrol : MonoBehaviour
{
    [SerializeField] private float patrolRadius = 5.0f;
    [SerializeField] private MetaEntity metaEntity;

    private ChibMovement movement;
    private Vector3 currentDestination;

    private void Start()
    {
        movement = GetComponent<ChibMovement>();

        if (movement == null) Debug.LogError("Movement component not found.");

        movement.SetMovementSpeed(1); // slower walking speed for patrollers
        ChooseNewDestination();
    }

    private void Update()
    {
        // randomly wander
        if (Random.value < 0.0001f || currentDestination == Vector3.zero)
        {
            ChooseNewDestination();
        }

        // move toward destination
        Vector3 moveDirection = currentDestination - transform.position;
        moveDirection.y = 0;

        if (moveDirection.magnitude < 0.5f) movement.StopMovement();
        else movement.SetMovementDirection(moveDirection.normalized);
    }

    private void ChooseNewDestination()
    {
        Vector2 randomOffset = Random.insideUnitCircle * patrolRadius;
        currentDestination = metaEntity.position + new Vector3(randomOffset.x * 4, 0, randomOffset.y);
    }

    public void UpdateMetaEntity(MetaEntity newMetaEntity)
    {
        metaEntity = newMetaEntity;
    }
}
