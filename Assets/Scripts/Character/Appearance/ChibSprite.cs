using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChibSprite : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private string currentSpriteName = "red_sorceress";
    private int currentSpriteIndex = 9;
    
    protected virtual void Awake()
    {
        // Get or add a SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            
        // Set up sprite renderer for Octopath-like appearance
        spriteRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        spriteRenderer.receiveShadows = false;
    }

    protected virtual void Start()
    {

    }
    
    protected virtual void Update()
    {
        
    }
    
    public bool SetSprite(string spriteName)
    {
        currentSpriteName = spriteName;
        
        // Get the sprite array from SpriteSheetManager
        Sprite[] sprites = SpriteSheetManager.instance.GetSpriteArray(spriteName);
        
        if (sprites != null && sprites.Length > 0)
        {
            spriteRenderer.sprite = sprites[currentSpriteIndex];
            transform.localScale = new Vector3(4f, 4f, 4f); // scale to normal character height in a unity game
            return true;
        }
        else
        {
            Debug.LogError($"Failed to load sprites for {spriteName}");
            return false;
        }
    }
    
    public bool SetSpriteIndex(int index)
    {
        if (string.IsNullOrEmpty(currentSpriteName)) return false;
            
        Sprite[] sprites = SpriteSheetManager.instance.GetSpriteArray(currentSpriteName);
        if (sprites != null && index >= 0 && index < sprites.Length)
        {
            currentSpriteIndex = index;
            spriteRenderer.sprite = sprites[currentSpriteIndex];
            return true;
        }
        return false;
    }
    
    void LateUpdate()
    {
        // Billboard effect - make sprite always face camera
        if (Camera.main != null)
        {
            transform.rotation = Camera.main.transform.rotation;
        }
    }
}