using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ObjectManager : NetworkBehaviour
{
    public static ObjectManager instance { get; private set; }

    [Header("Required Prefabs")]
    [SerializeField] private GameObject playerPrefab; // assigned in inspector
    
    void Awake()
    {
        // Singleton setup
        if (instance != null && instance != this) { Destroy(gameObject); return; }
        instance = this;

        if(playerPrefab == null) Debug.LogError("Player prefab not assigned in ObjectManager!");
    }

    [ServerRpc]
    public void CreatePlayerServerRpc(string playerName, string spriteName, ServerRpcParams rpcParams = default)
    {
        ulong clientId = rpcParams.Receive.SenderClientId;
        if(!NetworkManager.Singleton.ConnectedClients.ContainsKey(clientId)) Debug.LogError("Sender client not found in connected clients list.");
        if(!NetworkManager.Singleton.IsServer) Debug.LogError("CreatePlayerServerRpc called on non-server.");

        // create the player
        GameObject player = CreateObject(playerName, spriteName, EntityType.Player);
        Debug.Log($"Player {playerName} created for client {clientId} on Server");
        
        // spawn to clients
        NetworkObject networkObject = player.GetComponent<NetworkObject>();
        networkObject.SpawnAsPlayerObject(clientId); // Assigns the player object to the specific client
        Debug.Log($"Spawned player object for client {clientId}");
    }
    
    public GameObject CreateObject(string name, string spriteName, EntityType entityType, ulong clientId=0)
    {
        // Instantiate the character prefab
        GameObject newObject = Instantiate(entityType == EntityType.Player ? playerPrefab : null);
        
        newObject.name = name;

        // choose object location
        newObject.transform.position = new Vector3(0, 0, 0);
        
        // Set the object appearance
        newObject.GetComponent<ChibSprite>().SetSprite(spriteName);

        // Set tag
        newObject.tag = entityType.ToString();
        
        // Add to objects container
        newObject.transform.parent = transform;
        
        return newObject;
    }
}