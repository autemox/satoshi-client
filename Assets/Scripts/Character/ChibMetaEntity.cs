using UnityEngine;
using Unity.Netcode;
using Unity.Collections;

public class ChibMetaEntity : NetworkBehaviour
{
    private float transitionDistance = 1.5f;
    public NetworkVariable<FixedString128Bytes> followTarget; // patrols near this target if set
    public NetworkVariable<FixedString128Bytes> transitionSprite; // use this sprite if set
    public NetworkVariable<FixedString128Bytes> defaultSprite; // otherwise use this
    public NetworkVariable<Vector3> position; // only if they are not following target, also they may patrol near this position without sync

    private void Update()
    {
        // Check if we have access to the player
        if (Main.instance == null || Main.instance.player == null) return;
        
        // Get player's movement component
        ChibMoveLocal playerMovement = Main.instance.player.GetComponent<ChibMoveLocal>();
        if (playerMovement == null) return;
        
        // Calculate distance to player
        float distance = Vector3.Distance(transform.position, Main.instance.player.transform.position);
        
        // If player is close enough, trigger transition
        string playerSprite = Main.instance.player.GetComponent<ChibAnimated>().currentSpriteName;
        if (distance < transitionDistance && transitionSprite.GetString() != playerSprite)
        {
            Debug.Log("Transitioning entity " + name + " to follow player " + Main.instance.player.name);

            // Tell server to update this meta entity to follow the player and change sprite
            MetaEntityManager.instance.TransitionEntityServerRpc(
                name,                                                               // Entity name
                Main.instance.player.name,                                          // Follow target
                playerSprite                                                        // Player's sprite
            );
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        
        // Subscribe to NetworkVariable change events
        transitionSprite.OnValueChanged += OnTransitionSpriteChanged;
        defaultSprite.OnValueChanged += OnDefaultSpriteChanged;
        position.OnValueChanged += OnPositionChanged;
        followTarget.OnValueChanged += OnFollowTargetChanged;
    }

    public override void OnNetworkDespawn()
    {
        // Unsubscribe from NetworkVariable change events
        transitionSprite.OnValueChanged -= OnTransitionSpriteChanged;
        defaultSprite.OnValueChanged -= OnDefaultSpriteChanged;
        position.OnValueChanged -= OnPositionChanged;
        followTarget.OnValueChanged -= OnFollowTargetChanged;
        
        base.OnNetworkDespawn();
    }
    private void OnTransitionSpriteChanged(FixedString128Bytes previous, FixedString128Bytes current)
    {
        // set the sprite
        Debug.Log("Transitioning entity " + name + " to sprite " + transitionSprite.GetString());
        ChibSprite chibSprite = GetComponent<ChibSprite>();
        if (chibSprite != null) chibSprite.SetSprite(transitionSprite.GetString());
    }

    private void OnDefaultSpriteChanged(FixedString128Bytes previous, FixedString128Bytes current)
    {
        if(transitionSprite.GetString() == "")
        {
            // set the sprite if no transition sprite is set
            ChibSprite chibSprite = GetComponent<ChibSprite>();
            if (chibSprite != null) chibSprite.SetSprite(defaultSprite.GetString());
        }
    }

    private void OnPositionChanged(Vector3 previous, Vector3 current)
    {
        // set the position to destination
        ChibMovePatrol chibMovePatrol = GetComponent<ChibMovePatrol>();
        if (chibMovePatrol != null) chibMovePatrol.SetDestination(position.Value);
    }

    private void OnFollowTargetChanged(FixedString128Bytes previous, FixedString128Bytes current)
    {
        // set the follow target
        ChibMovePatrol patrol = GetComponent<ChibMovePatrol>();
        if (patrol != null) 
        {
            patrol.SetFollowTarget(followTarget.GetString());
        }
    }
}