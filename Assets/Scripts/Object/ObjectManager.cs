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
    public void CreateNetworkObjectServerRpc(string name, string spriteName, EntityType entityType, ServerRpcParams rpcParams = default)
    {
        // owner client id
        ulong clientId = rpcParams.Receive.SenderClientId;

        // Instantiate the character prefab
        GameObject obj = Instantiate(entityType == EntityType.Player ? playerPrefab : testPrefab, transform);
        
        // Network spawning
        NetworkObject networkObject = obj.GetComponent<NetworkObject>();
        networkObject.SpawnWithOwnership(clientId); // Assigns the player object to the specific client

        // Initialize details
        InitializeObject(networkObject.gameObject, name, spriteName, entityType, new Vector3(0,0,0)); // initialize on server
        InitializeNetworkObjectClientRpc(networkObject.NetworkObjectId, name, spriteName, entityType); // initialize on clients
    }

    public GameObject CreateLocalObject(string name, string spriteName, EntityType entityType, Vector3 location)
    {
        // Instantiate the character prefab
        GameObject obj = Instantiate(entityType == EntityType.MetaEntity ? metaEntityPrefab : testPrefab, location, Quaternion.identity, transform);
        InitializeObject(obj, name, spriteName, entityType, location); // initialize locally
        return obj;
    }

    public void InitializeObject(GameObject obj, string name, string spriteName, EntityType entityType, Vector3 position)
    {
        // Set common object properties
        obj.name = name;                                      // Set the object name
        obj.transform.position = position;                    // choose object location
        obj.tag = entityType.ToString();                      // Set tag

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
            InitializeObject(networkObject.gameObject, name, spriteName, entityType, new Vector3(0,0,0));
        }
    }
}