using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Main : MonoBehaviour
{
    [Header("Production vs Development Variables")]
    public bool isProduction = false;
    public bool isPortrait = false;
    private string DOMAIN_DEVELOPMENT = "http://localhost:3016"; // http
    private string DOMAIN_PRODUCTION = "https://lysle.net";
    private string PATH_DEVELOPEMENT = "";
    private string PATH_PRODUCTION = "/satoshi";
    public string baseDomain { get { return isProduction ? DOMAIN_PRODUCTION : DOMAIN_DEVELOPMENT; } }
    public string basePath { get { return isProduction ? PATH_PRODUCTION : PATH_DEVELOPEMENT; } }
    public string baseUrl { get { return baseDomain + basePath; } }

    [Header("Testing")]
    public bool vChangesCharacterAnimation = false;

    // other variables
    public GameState gameState { get; private set; } = GameState.None;
    public static Main instance { get; private set; }
    void Awake()
    {
        // singleton
        if (instance != null && instance != this) { Destroy(gameObject); return; }
        instance = this;
    }

    async void Start()
    {
            // Create and use the resource loader
            gameState = GameState.Loading;
            GameInitData gameData = await new LoadJsonResource().LoadGameInitDataAsync();
            List<string> spriteSheetFiles = new List<string>(gameData.spriteSheetFiles);
            Debug.Log($"Loaded {spriteSheetFiles.Count} SpriteSheets");
            await SpriteSheetManager.instance.LoadSpriteSheets(spriteSheetFiles);
            Debug.Log("SpriteSheets loaded");
    }

    void Update()
    {
        // check portrait status
        if (Screen.width < Screen.height) isPortrait = true;
        else isPortrait = false;
    }

    public void PlayerCharacterGeneratedSuccessfully(string spriteSheetName)
    {
        // Start the game
        StartGame(spriteSheetName);
    }

    public void PlayerSelectedPreSetCharacter(string spriteSheetName)
    {
        // Start the game
        StartGame(spriteSheetName);
    }

    private void StartGame(string spriteSheetName)
    {
        gameState = GameState.Playing;
        Debug.Log("Game initialized and ready to play!");

        // create a character
        ObjectManager.instance.CreateObject("Player", spriteSheetName, true);
    }
}
