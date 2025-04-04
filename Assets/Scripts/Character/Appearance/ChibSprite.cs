using UnityEngine;

public class ChibSprite : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public string currentSpriteName = "undefined";
    private int currentSpriteIndex = 9;
    [SerializeField] private GameObject billboardObject;
    
    protected virtual void Awake()
    {
        // Set up billboarding
        spriteRenderer = billboardObject.GetComponent<SpriteRenderer>();
        spriteRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        spriteRenderer.receiveShadows = false;

        if(billboardObject == null) Debug.LogError("Billboard object not found.");
    }

    public bool SetSprite(string spriteName)
    {
        Debug.Log("Setting sprite to: " + spriteName);
        
        currentSpriteName = spriteName; // update current sprite name
        
        // Get the sprite array from SpriteSheetManager
        Sprite[] sprites = SpriteSheetManager.instance.GetSpriteArray(spriteName.ToString());
        
        if (sprites != null && sprites.Length > 0)
        {
            spriteRenderer.sprite = sprites[currentSpriteIndex];
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

    protected virtual void Update() 
    {

    }

    private void OnDrawGizmos()
    {
        // Draw a small sphere at the pivot point
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.3f);
    }
}