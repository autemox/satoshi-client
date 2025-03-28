using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

// a meta entity is a player that is logged offline, or could be online
// when a player is offline their metaentity persists, chilling out and occasionally walking around
// if another player touches the meta entity, it will start following that player, and its sprite will change to match that players

public class MetaEntityManager : NetworkBehaviour
{
    public static MetaEntityManager instance { get; private set; }

    void Awake()
    {
        if (instance != null && instance != this) { Destroy(gameObject); return; }
        instance = this;
    }

    public Vector3 RandomLocation() 
    {
        return new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
    }

    public void InitializeMetaEntityManager()
    {
        if (IsServer || IsHost)
        {
            // create metaentities.  later this will load from db
            ObjectManager.instance.CreateNetworkObjectServerRpc("Trump", "trump", EntityType.MetaEntity, RandomLocation(), default);
            ObjectManager.instance.CreateNetworkObjectServerRpc("Jesus", "jesus", EntityType.MetaEntity, RandomLocation(), default);
            ObjectManager.instance.CreateNetworkObjectServerRpc("blue-water-sorceress", "blue-water-sorceress", EntityType.MetaEntity, RandomLocation(), default);
            ObjectManager.instance.CreateNetworkObjectServerRpc("fruit-loops-toucan", "fruit-loops-toucan", EntityType.MetaEntity, RandomLocation(), default);
            ObjectManager.instance.CreateNetworkObjectServerRpc("red-sorceress", "red-sorceress", EntityType.MetaEntity, RandomLocation(), default);
            ObjectManager.instance.CreateNetworkObjectServerRpc("chicken", "chicken", EntityType.MetaEntity, RandomLocation(), default);
            
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void TransitionEntityServerRpc(string entityName, string followTarget, string transitionSprite)
    {
        Transform childTransform = transform.Find(entityName);
        if(childTransform == null) Debug.LogError("MetaEntity not found: " + entityName + "");

        ChibMetaEntity metaEntity = childTransform.GetComponent<ChibMetaEntity>();
        if(metaEntity == null) Debug.LogError("MetaEntity component not found: " + entityName + "");

        Debug.Log("Transitioning entity " + entityName + " to follow player " + followTarget+" as sprite "+ transitionSprite);
        metaEntity.followTarget.SetString(followTarget);
        metaEntity.transitionSprite.SetString(transitionSprite);
    }
}