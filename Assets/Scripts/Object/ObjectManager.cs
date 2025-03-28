using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ObjectManager : NetworkBehaviour
{
    public static ObjectManager instance { get; private set; }

    [Header("Required Prefabs")]
    [SerializeField] private GameObject playerPrefab; // assigned in inspector
    [SerializeField] private GameObject testPrefab; // assigned in inspector
    
    void Awake()
    {
        // Singleton setup
        if (instance != null && instance != this) { Destroy(gameObject); return; }
        instance = this;

        if(playerPrefab == null) Debug.LogError("Player prefab not assigned in ObjectManager!");
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
        Debug.Log($"Creating {name}, sprite {spriteName}, {entityType} object for client {clientId}");
        GameObject obj = Instantiate(entityType == EntityType.Player ? playerPrefab : testPrefab);
        
        // Network spawning
        NetworkObject networkObject = obj.GetComponent<NetworkObject>();
        Debug.Log($"Spawning player for client {clientId}");
        networkObject.SpawnWithOwnership(clientId); // Assigns the player object to the specific client
        Debug.Log($"Spawned Player '{spriteName}' for client {clientId}");

        // Initialize details
        InitializeNetworkObject(networkObject, name, spriteName, entityType); // initialize on server
        InitializeNetworkObjectClientRpc(networkObject.NetworkObjectId, name, spriteName, entityType); // initialize on clients
    }

    public void InitializeNetworkObject(NetworkObject networkObject, string name, string spriteName, EntityType entityType)
    {
        // Set common object properties
        GameObject obj = networkObject.gameObject;
        obj.name = name;                                      // Set the object name
        obj.transform.position = new Vector3(0, 0, 0);        // choose object location
        obj.tag = entityType.ToString();                      // Set tag

        // Handle chib objects
        ChibSprite sprite = obj.GetComponent<ChibSprite>();
        Debug.Log($"Setting sprite to {spriteName} via {sprite}");
        if(sprite != null) sprite.SetSprite(spriteName);            // Set sprite
        Debug.Log($"Set sprite to {spriteName} via {sprite}");

        // Add to objects container (after network spawning)
        obj.transform.parent = transform;
    }

    [ClientRpc]
    private void InitializeNetworkObjectClientRpc(ulong networkObjectId, string name, string spriteName, EntityType entityType)
    {
        NetworkObject networkObject = NetworkManager.Singleton.SpawnManager.SpawnedObjects[networkObjectId];
        if (networkObject != null)
        {
            InitializeNetworkObject(networkObject, name, spriteName, entityType);
        }
    }
}