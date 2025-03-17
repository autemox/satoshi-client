using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class LoadJsonResource
{
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