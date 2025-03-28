using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChibMovePatrol : MonoBehaviour
{
    [SerializeField] private float patrolRadius = 5.0f;

    private ChibMovement movement;
    private Vector3 offset;
    private GameObject followTarget;

    private void Start()
    {
        movement = GetComponent<ChibMovement>();

        if (movement == null) Debug.LogError("Movement component not found.");

        ChooseNewOffset();
    }

    private void Update()
    {
        // randomly wander
        if (Random.value < 0.0001f || offset == Vector3.zero)
        {
            ChooseNewOffset();
        }

        // move toward destination
        Vector3 destination = followTarget ? 
            followTarget.transform.position + offset :                  // we have a follow target walk toward it
            GetComponent<ChibMetaEntity>().position.Value + offset; // no follow target, wander near start point
        Vector3 moveDirection = destination - transform.position;
        moveDirection.y = 0;

        if(followTarget == null) movement.SetMovementSpeed(1); // slower walking speed for patrollers
        else movement.SetMovementSpeed(9); // faster for follow targets

        if (moveDirection.magnitude < 0.5f) movement.StopMovement();
        else movement.SetMovementDirection(moveDirection.normalized);
    }

    private void ChooseNewOffset()
    {
        Vector2 randomOffset = Random.insideUnitCircle * patrolRadius;
        SetDestination(GetComponent<ChibMetaEntity>().position.Value + new Vector3(randomOffset.x * 4, 0, randomOffset.y));
    }

    public void SetDestination(Vector3 destination)
    {
        offset = destination;
    }

    public void SetFollowTarget(string targetName)
    {
        followTarget = GameObject.Find(targetName);
    }
}
