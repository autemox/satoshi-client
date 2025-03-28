using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;
using Unity.Multiplayer.Playmode;
using System.Linq; 

public class Main : NetworkBehaviour
{
    [Header("Production vs Development Variables")]
    public bool isProduction = false;
    [NonSerialized] public bool isPortrait = false; // set automatically based on height/width
    private string DOMAIN_DEVELOPMENT = "http://localhost:3016"; // http
    private string DOMAIN_PRODUCTION = "https://lysle.net";
    private string PATH_DEVELOPEMENT = "";
    private string PATH_PRODUCTION = "/satoshi";
    public string baseDomain { get { return isProduction ? DOMAIN_PRODUCTION : DOMAIN_DEVELOPMENT; } }
    public string basePath { get { return isProduction ? PATH_PRODUCTION : PATH_DEVELOPEMENT; } }
    public string baseUrl { get { return baseDomain + basePath; } }

    [Header("Http Networking")]
    public string clientId; // a unique client id generated every time the game starts

    // other variables
    public GameState gameState { get; private set; } = GameState.None;
    public static Main instance { get; private set; }
    public bool IsConnected => !NetworkManager.Singleton.IsClient || NetworkManager.Singleton.IsConnectedClient;
    public GameObject player => FindFirstObjectByType<ChibMoveLocal>()?.gameObject;
    void Awake()
    {
        // singleton
        if (instance != null && instance != this) { Destroy(gameObject); return; }
        instance = this;

        // select a 4 digit alphanumeric id for network communication
        clientId = System.Guid.NewGuid().ToString().Substring(0, 4);
    }

    async void Start()
    {
            // Use the Api to get the sprite sheet files
            gameState = GameState.Loading;
            
            if(CurrentPlayer.ReadOnlyTags().Contains("Host")) // linq
            {
                NetworkManager.Singleton.StartHost(); // start netcode
                Debug.Log("Host started");
            }
            else if (CurrentPlayer.ReadOnlyTags().Contains("Client")) // linq
            {
                NetworkManager.Singleton.StartClient();
                Debug.Log("Client started");
            }
            else Debug.LogError("No valid player tag found. Assign 'Host' or 'Client' tag to the player.");
            
            // monitor network
            if(NetworkManager.Singleton) {
                NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
                NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
            }
            else Debug.LogError("NetworkManager not found."); 
            
            string[] resultArr = await new ApiClient().LoadSpriteSheetFiles(clientId);
            List<string> spriteSheetFiles = new List<string>(resultArr); // get the list of image urls
            Debug.Log($"Loaded {spriteSheetFiles.Count} spritesheet filenames");
            await SpriteSheetManager.instance.LoadSpriteSheets(spriteSheetFiles); // download the images
            Debug.Log("Spritesheets downloaded");
            UiCharacterCreation.instance.ShowWindow(); // show the character creation UI
            Debug.Log("Waiting for user to select spritesheet");
    }

    private void OnClientConnected(ulong clientId)
    {
        Debug.Log($"Client connected with ID: {clientId}");
    }

    private void OnClientDisconnected(ulong clientId)
    {
        Debug.Log($"Client disconnected with ID: {clientId}");
    }

    public void PlayerSelectedCharacter(string playerName, string spriteSheetName) // UiCharacterCreation calls this
    {
        // Start the game
        if(IsConnected) StartGame(playerName, spriteSheetName);
        else if(UiCharacterCreation.instance) UiCharacterCreation.instance.ShowWarning("Not connected to server.");
    }
    private void StartGame(string playerName, string spriteSheetName)
    {
        gameState = GameState.Playing;

        // close all windows
        UiCharacterCreation.instance.HideWindow();
        
        // create a character
        Debug.Log("Starting game... Calling CreateNetworkObject");
        ObjectManager.instance.CreateNetworkObjectServerRpc(playerName, spriteSheetName, EntityType.Player, default, default);

        // initialize the meta entity manager
        MetaEntityManager.instance.InitializeMetaEntityManager();
    }

    void Update()
    {
        // check for portrait/mobile
        isPortrait = Screen.width < Screen.height;
    }
}
