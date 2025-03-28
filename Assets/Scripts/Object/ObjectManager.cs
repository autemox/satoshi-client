using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ObjectManager : NetworkBehaviour
{
    public static ObjectManager instance { get; private set; }

    [Header("Required Prefabs")]
    [SerializeField] private GameObject playerPrefab; // assigned in inspector
    [SerializeField] private GameObject metaEntityPrefab; // assigned in inspector
    [SerializeField] private GameObject testPrefab; // assigned in inspector
    
    void Awake()
    {
        // Singleton setup
        if (instance != null && instance != this) { Destroy(gameObject); return; }
        instance = this;

        if(playerPrefab == null) Debug.LogError("Player prefab not assigned in ObjectManager!");
        if(metaEntityPrefab == null) Debug.LogError("MetaEntity prefab not assigned in ObjectManager!");
        if(testPrefab == null) Debug.LogError("Test prefab not assigned in ObjectManager!");
    }

    void Start()
    {

    }

    [ServerRpc(RequireOwnership = false)]
    public void CreateNetworkObjectServerRpc(string name, string spriteName, EntityType entityType, Vector3 position = default, ServerRpcParams rpcParams = default)
    {
        if(position == default) position = new Vector3(0,0,0);
        
        // owner client id
        ulong clientId = rpcParams.Receive.SenderClientId;

        // Instantiate the character prefab
        GameObject obj = Instantiate(
            entityType == EntityType.Player ? playerPrefab : 
            entityType == EntityType.MetaEntity ? metaEntityPrefab :
            testPrefab, position, Quaternion.identity, transform);

        // Network spawning
        NetworkObject networkObject = obj.GetComponent<NetworkObject>();
        if(clientId != 0) networkObject.SpawnWithOwnership(clientId); 
        else networkObject.Spawn(); // Server owns the object

        // Initialize details
        InitializeObject(networkObject.gameObject, name, spriteName, entityType); // initialize on server
        InitializeNetworkObjectClientRpc(networkObject.NetworkObjectId, name, spriteName, entityType); // initialize on clients
    }

    public GameObject CreateLocalObject(string name, string spriteName, EntityType entityType, Vector3 location)
    {
        // Instantiate the character prefab
        GameObject obj = Instantiate(entityType == EntityType.MetaEntity ? metaEntityPrefab : testPrefab, location, Quaternion.identity, transform);
        InitializeObject(obj, name, spriteName, entityType); // initialize locally
        return obj;
    }

    public void InitializeObject(GameObject obj, string name, string spriteName, EntityType entityType)
    {
        // Set common object properties
        obj.name = name;                                      // Set the object name
        obj.tag = entityType.ToString();                      // Set tag
        
        // Pool objects under common parent
        if(entityType == EntityType.Player) obj.transform.parent = transform;
        else if(entityType == EntityType.MetaEntity) obj.transform.parent = MetaEntityManager.instance.transform;
        else obj.transform.parent = transform;

        // Handle chib objects
        ChibSprite sprite = obj.GetComponent<ChibSprite>();
        if(sprite != null) sprite.SetSprite(spriteName);            // Set sprite
    }

    [ClientRpc]
    private void InitializeNetworkObjectClientRpc(ulong networkObjectId, string name, string spriteName, EntityType entityType)
    {
        NetworkObject networkObject = NetworkManager.Singleton.SpawnManager.SpawnedObjects[networkObjectId];
        if (networkObject != null)
        {
            InitializeObject(networkObject.gameObject, name, spriteName, entityType);
        }
    }
}