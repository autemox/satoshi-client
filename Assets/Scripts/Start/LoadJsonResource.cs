using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

// 2 calls to http server are handled in LoadJsonResource
// 1. GenerateSpriteSheet: POST /api/generate-spritesheet and RESPONDS SpriteSheetResponse
// 2. LoadGameInitDataAsync: GET /api/game-init and RESPONDS GameInitData


public class LoadJsonResource
{
    public async Task<string> GenerateSpriteSheet(string characterDescription)
    {
        string url = Main.instance.baseUrl + "/api/generate-spritesheet";
        
        // Create JSON payload
        var requestData = new SpriteSheetRequest { prompt = characterDescription };
        string jsonContent = JsonUtility.ToJson(requestData);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonContent);
        
        Debug.Log($"Generating sprite sheet for: {characterDescription} to url: {url}");
        using (var request = new UnityWebRequest(url, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            
            var operation = request.SendWebRequest();
            
            while (!operation.isDone) await Task.Yield();
                
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Failed to generate sprite sheet: {request.error}");
                return null;
            }
            
            // Parse response to get the filename
            var responseJson = request.downloadHandler.text;
            var response = JsonUtility.FromJson<SpriteSheetResponse>(responseJson);
            
            return response.filename;
        }
    }

    [System.Serializable]
    private class SpriteSheetRequest
    {
        public string prompt;
    }

    [System.Serializable]
    private class SpriteSheetResponse
    {
        public string filename;
    }

    public async Task<GameInitData> LoadGameInitDataAsync()
    {
        string url = Main.instance.baseUrl + "/api/game-init";
        
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            var operation = request.SendWebRequest();
            
            while (!operation.isDone)
                await Task.Yield();
                
            if (request.result != UnityWebRequest.Result.Success)
                throw new Exception($"Network error: {request.error}");
                
            return JsonUtility.FromJson<GameInitData>(request.downloadHandler.text);
        }
    }
}