using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager instance { get; private set; }

    [Header("Required Prefabs")]
    [SerializeField] private GameObject playerPrefab; // assigned in inspector
    public Dictionary<string, GameObject> objects { get; private set; } = new Dictionary<string, GameObject>();
    
    void Awake()
    {
        // Singleton setup
        if (instance != null && instance != this) { Destroy(gameObject); return; }
        instance = this;

        if(playerPrefab == null) Debug.LogError("Player prefab not assigned in ObjectManager!");
    }
    
    public GameObject CreateObject(string name, string spriteName, bool isPlayer)
    {
        // Instantiate the character prefab
        GameObject newObject = Instantiate(isPlayer ? playerPrefab : null);
        newObject.name = name;

        // choose character location
        newObject.transform.position = new Vector3(0, 0, 0);
        
        // Set the characters appearance
        newObject.GetComponent<SheetCharacter>().SetSprite(spriteName);

        // Set tag
        newObject.tag = isPlayer ? "Player" : "NPC";
        
        // Add to active objects dictionary
        objects[name] = newObject;
        
        return newObject;
    }
    
    public void DestroyObject(string name)
    {
        if (objects.TryGetValue(name, out GameObject obj))
        {
            Destroy(obj);
            objects.Remove(name);
        }
    }
}