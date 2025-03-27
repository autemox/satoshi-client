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

    [ServerRpc]
    public void CreatePlayerServerRpc(string playerName, string spriteName, ServerRpcParams rpcParams = default)
    {
        ulong clientId = rpcParams.Receive.SenderClientId;
        if(!NetworkManager.Singleton.ConnectedClients.ContainsKey(clientId)) Debug.LogError("Sender client not found in connected clients list.");
        if(!NetworkManager.Singleton.IsServer) Debug.LogError("CreatePlayerServerRpc called on non-server.");

        // create the player with a callback to spawn it first
        GameObject player = CreateObject(playerName, spriteName, EntityType.Player, clientId, (obj) => {
            
            NetworkObject networkObject = obj.GetComponent<NetworkObject>();
            networkObject.SpawnAsPlayerObject(clientId); // Assigns the player object to the specific client
            Debug.Log($"Spawned player object for client {clientId}");
        });
        
        Debug.Log($"Spawned player object for client {clientId}");
    }
    
    public GameObject CreateObject(string name, string spriteName, EntityType entityType, ulong clientId=0, System.Action<GameObject> afterInstantiateAction = null)
    {
        // Instantiate the character prefab
        GameObject newObject = Instantiate(entityType == EntityType.Player ? playerPrefab : testPrefab);
        newObject.name = name;                                      // Set the object name
        newObject.transform.position = new Vector3(0, 0, 0);        // choose object location
        newObject.tag = entityType.ToString();                      // Set tag

        // Handle chib objects
        ChibSprite sprite = newObject.GetComponent<ChibSprite>();
        if(sprite != null) sprite.SetSprite(spriteName);            // Set sprite
        
        // Execute passed function before parenting, e.g. network spawning
        afterInstantiateAction?.Invoke(newObject);
        
        // Add to objects container (after network spawning)
        newObject.transform.parent = transform;
        
        return newObject;
    }
}