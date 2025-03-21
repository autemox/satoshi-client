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
    public string id;

    // other variables
    public GameState gameState { get; private set; } = GameState.None;
    public static Main instance { get; private set; }
    void Awake()
    {
        // singleton
        if (instance != null && instance != this) { Destroy(gameObject); return; }
        instance = this;

        // select a 4 digit alphanumeric id for network communication
        id = System.Guid.NewGuid().ToString().Substring(0, 4);
    }

    async void Start()
    {
            // Use the Api to get the sprite sheet files
            gameState = GameState.Loading;
            string[] resultArr = await new ApiClient().LoadSpriteSheetFiles(id);
            List<string> spriteSheetFiles = new List<string>(resultArr); // get the list of image urls
            Debug.Log($"Loaded {spriteSheetFiles.Count} spritesheet filenames");
            await SpriteSheetManager.instance.LoadSpriteSheets(spriteSheetFiles); // download the images
            Debug.Log("Spritesheets downloaded");
    }

    void Update()
    {
        // check for portrait/mobile
        isPortrait = Screen.width < Screen.height ? true : false;
    }

    public void PlayerCharacterGeneratedSuccessfully(string spriteSheetName) // UiCharacterCreation calls this
    {
        // Start the game
        StartGame(spriteSheetName);
    }

    public void PlayerSelectedPreSetCharacter(string spriteSheetName) // UiCharacterCreation calls this
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
