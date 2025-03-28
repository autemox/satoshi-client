using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class SpriteSheetManager : MonoBehaviour
{
    // singleton variables
    public static SpriteSheetManager instance { get; private set; }
    
    // Dictionary to cache sprite sheet assets by filename
    private Dictionary<string, SpriteSheetAsset> SpriteSheetCache = new Dictionary<string, SpriteSheetAsset>();
    
    // Default grid size for sprite sheets (4x4 grid = 16 sprites)
    private const int DEFAULT_ROWS = 4;
    private const int DEFAULT_COLS = 4;
    
    void Awake()
    {
        // singleton
        if (instance != null && instance != this) { Destroy(gameObject); return; }
        instance = this;
    }
    
    public async Task LoadSpriteSheets(List<string> SpriteSheetFiles)
    {
        List<Task> downloadTasks = new List<Task>();
        
        foreach (string filename in SpriteSheetFiles)
        {
            // Start all downloads in parallel
            downloadTasks.Add(DownloadSpriteSheet(filename));
        }
        
        // Wait for all downloads to complete
        await Task.WhenAll(downloadTasks);
    }
    
    public async Task DownloadSpriteSheet(string filename)
    {
        if (SpriteSheetCache.ContainsKey(filename)) return;
        
        string url = Main.instance.baseUrl + "/images/spritesheets/" + filename;
        
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            var operation = request.SendWebRequest();
            
            while (!operation.isDone)
                await Task.Yield();
                
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Failed to download SpriteSheet {filename}: {request.error}");
                return;
            }
            
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            texture.filterMode = FilterMode.Point; // Prevent blurry pixel art
            
            // Create sprite array and cache the asset
            Sprite[] sprites = CreateSpriteArray(texture, DEFAULT_ROWS, DEFAULT_COLS);

            // name is the filename without extension
            string name = filename.Substring(0, filename.LastIndexOf('.'));
            
            // Create and cache the sprite sheet asset
            SpriteSheetAsset asset = new SpriteSheetAsset(
                name,
                texture, 
                sprites, 
                DEFAULT_ROWS, 
                DEFAULT_COLS
            );
            
            SpriteSheetCache[name] = asset;
        }
    }
    
    private Sprite[] CreateSpriteArray(Texture2D texture, int rows, int cols)
    {
        int spriteWidth = texture.width / cols;
        int spriteHeight = texture.height / rows;
        Sprite[] sprites = new Sprite[rows * cols];
        
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                // Calculate the sprite position - starting from top-left (Unity's default is bottom-left)
                Rect rect = new Rect(x * spriteWidth, texture.height - (y + 1) * spriteHeight, spriteWidth, spriteHeight);
                
                // Create the sprite and add it to the array
                int index = y * cols + x;
                sprites[index] = 
                    Sprite.Create(
                        texture,
                        rect,
                        new Vector2(0.5f, 0.12f), // Pivot at bottom but leave a little room for transparent pixels
                        15f // Pixels per unit
                    );
            }
        }
        
        return sprites;
    }
    
    // Get a specific SpriteSheet's sprite array by filename or name
    public Sprite[] GetSpriteArray(string name)
    {
        // convert filename to name's
        if (name.EndsWith(".png") || name.EndsWith(".jpg") || name.EndsWith(".jpeg") || name.EndsWith(".gif") || name.EndsWith(".bmp")) name = name.Substring(0, name.LastIndexOf('.'));

        // check fo asset in cache
        if (SpriteSheetCache.TryGetValue(name, out SpriteSheetAsset asset))
            return asset.sprites;
            
        Debug.LogWarning($"SpriteSheet {name} not found in cache (length: {SpriteSheetCache.Count}) Cache: {string.Join(", ", SpriteSheetCache.Keys)}");
        return null;
    }

    public String[] GetSpriteSheetNames()
    {
        return new List<string>(SpriteSheetCache.Keys).ToArray();
    }
}