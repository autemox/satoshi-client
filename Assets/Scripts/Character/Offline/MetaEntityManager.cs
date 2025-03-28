using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

// a meta entity is a player that is logged offline, or could be online
// when a player is offline their metaentity persists, chilling out and occasionally walking around
// if another player touches the meta entity, it will start following that player, and its sprite will change to match that players

public class MetaEntityManager : NetworkBehaviour
{
    private Dictionary<string, MetaEntity> metaEntities = new Dictionary<string, MetaEntity>();
    public static MetaEntityManager instance { get; private set; }

    void Awake()
    {
        if (instance != null && instance != this) { Destroy(gameObject); return; }
        instance = this;
    }

    public void InitializeMetaEntityManager()
    {
        if (IsServer || IsHost)
        {
            // dummy data
            AddOrUpdateEntity(new MetaEntity("Trump", new Vector3(-8, 0, 4), "trump"));
            AddOrUpdateEntity(new MetaEntity("Jesus", new Vector3(10, 0, -2), "jesus"));

            // Start update loop
            InvokeRepeating(nameof(UpdateClients), 0f, 10f);
        }
    }

    public void AddOrUpdateEntity(MetaEntity entity)
    {
        metaEntities[entity.name] = entity;
        entity.changed = true;
    }

    private void UpdateClients()
    {
        if (!IsServer) return;

        List<MetaEntity> changedEntities = new List<MetaEntity>();

        // Collect all changed entities
        foreach (var entity in metaEntities.Values)
        {
            if (entity.changed)
            {
                changedEntities.Add(entity);
                entity.changed = false;
            }
        }

        if (changedEntities.Count > 0) SendEntityUpdatesClientRpc(changedEntities.ToArray());
    }

    [ClientRpc]
    private void SendEntityUpdatesClientRpc(MetaEntity[] updatedEntities)
    {
        foreach (MetaEntity entity in updatedEntities)
        {
            // find the object (a child of ObjectManager.instance with name entity.name)
            Transform objTransform = ObjectManager.instance.transform.Find(entity.name);

            if (objTransform != null)
            {
                // update the metaentity data to the objects ChibMovePatrol component
                ChibMovePatrol patrolComponent = objTransform.GetComponent<ChibMovePatrol>();
                if (patrolComponent != null) patrolComponent.UpdateMetaEntity(entity);
            }
            else
            {
                // create an object and apply the entity data
                GameObject obj = ObjectManager.instance.CreateLocalObject(entity.name, entity.defaultSprite, EntityType.MetaEntity, entity.position);
                obj.GetComponent<ChibMovePatrol>().UpdateMetaEntity(entity);
            }
        }
    }
}