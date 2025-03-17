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
        
        Debug.Log($"Loaded {SpriteSheetCache.Count} SpriteSheets into cache");
    }
    
    private async Task DownloadSpriteSheet(string filename)
    {
        if (SpriteSheetCache.ContainsKey(filename))
        {
            Debug.Log($"SpriteSheet {filename} already cached, skipping download");
            return;
        }
        
        string url = Main.instance.baseUrl + "/images/spritesheets/" + filename;
        
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            Debug.Log($"Downloading SpriteSheet: {filename}");
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
            
            Debug.Log($"Downloaded and split SpriteSheet: {filename}");
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
                sprites[index] = Sprite.Create(
                    texture,
                    rect,
                    new Vector2(0.5f, 0.5f), // Pivot at center
                    100f // Pixels per unit
                );
            }
        }
        
        return sprites;
    }
    
    // Get a specific SpriteSheet's sprite array by filename
    public Sprite[] GetSpriteArray(string name)
    {
        if (SpriteSheetCache.TryGetValue(name, out SpriteSheetAsset asset))
            return asset.sprites;
            
        Debug.LogWarning($"SpriteSheet {name} not found in cache");
        return null;
    }
}