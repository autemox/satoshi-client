using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager instance { get; private set; }

    [Header("Required Prefabs")]
    [SerializeField] private GameObject characterPrefab; // Assign this in inspector
    
    public Dictionary<string, GameObject> objects { get; private set; } = new Dictionary<string, GameObject>();
    
    void Awake()
    {
        // Singleton setup
        if (instance != null && instance != this) { Destroy(gameObject); return; }
        instance = this;

        if(characterPrefab == null) Debug.LogError("Character prefab not assigned in ObjectManager!");
    }
    
    public GameObject CreateObject(string name, string spriteName, bool isPlayer)
    {
        // Instantiate the character prefab
        GameObject newObject = Instantiate(characterPrefab);
        newObject.name = name;

        // place character center screen
        newObject.transform.position = new Vector3(0, 0, 0);
        
        // Get or add required components
        CharacterAppearance appearance = newObject.GetComponent<CharacterAppearance>();
        appearance.SetSprite(spriteName);

        // Set tag based on player or NPC
        if (isPlayer) newObject.tag = "Player";
        else newObject.tag = "NPC";
        
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