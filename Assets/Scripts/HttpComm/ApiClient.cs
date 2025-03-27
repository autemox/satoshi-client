using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

// Contains 2 calls to http server:
// 1. GenerateSpriteSheet: POST /api/generate-spritesheet and RESPONDS SpriteSheetResponse
// 2. LoadGameInitDataAsync: GET /api/game-init and RESPONDS GameInitData

// Data objects to be used in request and response API calls.  add more here as needed
[Serializable] public class StringData { public string value; }
[Serializable] public class StringArrayData { public string[] values; }

public class ApiClient
{
    public async Task<string> GenerateSpriteSheet(string characterDescription)
    {
        // Generate a new sprite sheet: returns the filename if successful
        var requestData = new StringData { value = characterDescription };
        var response = await MakeApiRequest<StringData, StringData>("/api/generate-spritesheet", requestData);
        if (response == null) throw new Exception("Failed to generate sprite sheet");
        return response.value;
    }

    public async Task<string[]> LoadSpriteSheetFiles(string deviceId)
    {
        // Load the array of sprite sheet files
        var requestData = new StringData { value = deviceId };
        var response = await MakeApiRequest<StringArrayData, StringData>("/api/game-init", requestData);
        if (response == null) throw new Exception("Failed to load sprite sheet files");
        return response.values;
    }
    
    public async Task<T> MakeApiRequest<T, R>(string urlPath, R requestData)
    {
        // Generic API request method
        string url = Main.instance.baseUrl + urlPath;
        string jsonContent = JsonUtility.ToJson(requestData);

        T response = default;
        using (var request = new UnityWebRequest(url, "POST")) 
        {
            request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonContent));
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            
            var operation = request.SendWebRequest();
            while (!operation.isDone) await Task.Yield();
            
            if (request.result != UnityWebRequest.Result.Success) 
            { 
                Debug.LogError($"POST to {url} Failed: {request.error}"); 
                return default; 
            }
            
            response = JsonUtility.FromJson<T>(request.downloadHandler.text);
        }

        return response;
    }
}