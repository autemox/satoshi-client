# Scripts

> CodeMap Source: Local directory: `/Users/lysle/Workspace/satoshi-client/Assets/Scripts`

This markdown document provides a comprehensive overview of the directory structure and file contents. It aims to give viewers (human or AI) a complete view of the codebase in a single file for easy analysis.

## Document Table of Contents

The table of contents below is for navigational convenience and reflects this document's structure, not the actual file structure of the repository.

<!-- TOC -->

- [Scripts](#scripts)
  - [Document Table of Contents](#document-table-of-contents)
  - [Repo File Tree](#repo-file-tree)
  - [Repo File Contents](#repo-file-contents)
    - [Character.meta](#charactermeta)
    - [Character/Appearance.meta](#characterappearancemeta)
    - [Character/Appearance/CharacterAnimate.cs](#characterappearancecharacteranimatecs)
    - [Character/Appearance/CharacterAnimate.cs.meta](#characterappearancecharacteranimatecsmeta)
    - [Character/Appearance/CharacterAnimation.cs](#characterappearancecharacteranimationcs)
    - [Character/Appearance/CharacterAnimation.cs.meta](#characterappearancecharacteranimationcsmeta)
    - [Character/Appearance/CharacterAppearance.cs](#characterappearancecharacterappearancecs)
    - [Character/Appearance/CharacterAppearance.cs.meta](#characterappearancecharacterappearancecsmeta)
    - [Character/Controls.meta](#charactercontrolsmeta)
    - [Character/Controls/CharacterMovement.cs](#charactercontrolscharactermovementcs)
    - [Character/Controls/CharacterMovement.cs.meta](#charactercontrolscharactermovementcsmeta)
    - [Character/Controls/Player.cs](#charactercontrolsplayercs)
    - [Character/Controls/Player.cs.meta](#charactercontrolsplayercsmeta)
    - [Comm.meta](#commmeta)
    - [Comm/GameInitData.cs](#commgameinitdatacs)
    - [Comm/GameInitData.cs.meta](#commgameinitdatacsmeta)
    - [Comm/LoadJsonResource.cs](#commloadjsonresourcecs)
    - [Comm/LoadJsonResource.cs.meta](#commloadjsonresourcecsmeta)
    - [GameState.cs](#gamestatecs)
    - [GameState.cs.meta](#gamestatecsmeta)
    - [Main.cs](#maincs)
    - [Main.cs.meta](#maincsmeta)
    - [ObjectManager.cs](#objectmanagercs)
    - [ObjectManager.cs.meta](#objectmanagercsmeta)
    - [SpriteSheet.meta](#spritesheetmeta)
    - [SpriteSheet/SpriteSheetAsset.cs](#spritesheetspritesheetassetcs)
    - [SpriteSheet/SpriteSheetAsset.cs.meta](#spritesheetspritesheetassetcsmeta)
    - [SpriteSheet/SpriteSheetManager.cs](#spritesheetspritesheetmanagercs)
    - [SpriteSheet/SpriteSheetManager.cs.meta](#spritesheetspritesheetmanagercsmeta)
    - [Ui.meta](#uimeta)
    - [Ui/UiCharacterCreation.cs](#uiuicharactercreationcs)
    - [Ui/UiCharacterCreation.cs.meta](#uiuicharactercreationcsmeta)

<!-- /TOC -->

## Repo File Tree

This file tree represents the actual structure of the repository. It's crucial for understanding the organization of the codebase.

```tree
.
├── Character/
│   ├── Appearance/
│   │   ├── CharacterAnimate.cs
│   │   ├── CharacterAnimate.cs.meta
│   │   ├── CharacterAnimation.cs
│   │   ├── CharacterAnimation.cs.meta
│   │   ├── CharacterAppearance.cs
│   │   └── CharacterAppearance.cs.meta
│   ├── Controls/
│   │   ├── CharacterMovement.cs
│   │   ├── CharacterMovement.cs.meta
│   │   ├── Player.cs
│   │   └── Player.cs.meta
│   ├── Appearance.meta
│   └── Controls.meta
├── Comm/
│   ├── GameInitData.cs
│   ├── GameInitData.cs.meta
│   ├── LoadJsonResource.cs
│   └── LoadJsonResource.cs.meta
├── SpriteSheet/
│   ├── SpriteSheetAsset.cs
│   ├── SpriteSheetAsset.cs.meta
│   ├── SpriteSheetManager.cs
│   └── SpriteSheetManager.cs.meta
├── Ui/
│   ├── UiCharacterCreation.cs
│   └── UiCharacterCreation.cs.meta
├── Character.meta
├── Comm.meta
├── GameState.cs
├── GameState.cs.meta
├── Main.cs
├── Main.cs.meta
├── ObjectManager.cs
├── ObjectManager.cs.meta
├── SpriteSheet.meta
└── Ui.meta

6 directories, 32 files
```

## Repo File Contents

The following sections present the content of each file in the repository. Large and binary files are acknowledged but their contents are not displayed.

### ObjectManager.cs

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager instance { get; private set; }

    [Header("Required Prefabs")]
    [SerializeField] private GameObject characterPrefab; // Assign this in inspector
    
    public Dictionary<string, GameObject> objects { get; private set; } = new Dictionary<string, GameObject>();
    
    void Awake()
    {
        // Singleton setup
        if (instance != null && instance != this) { Destroy(gameObject); return; }
        instance = this;

        if(characterPrefab == null) Debug.LogError("Character prefab not assigned in ObjectManager!");
    }
    
    public GameObject CreateObject(string name, string spriteName, bool isPlayer)
    {
        // Instantiate the character prefab
        GameObject newObject = Instantiate(characterPrefab);
        newObject.name = name;

        // place character center screen
        newObject.transform.position = new Vector3(0, 0, 0);
        
        // Get or add required components
        CharacterAppearance appearance = newObject.GetComponent<CharacterAppearance>();
        appearance.SetSprite(spriteName);

        // Set tag based on player or NPC
        if (isPlayer) newObject.tag = "Player";
        else newObject.tag = "NPC";
        
        // Add to active objects dictionary
        objects[name] = newObject;
        
        return newObject;
    }
    
    public void DestroyObject(string name)
    {
        if (objects.TryGetValue(name, out GameObject obj))
        {
            Destroy(obj);
            objects.Remove(name);
        }
    }
}
```

### GameState.cs

```csharp
public enum GameState
{
    None,       // Initial state
    Loading,    // Loading resources/assets
    Playing,    // Gameplay active
}
```

### Main.cs.meta

```txt
fileFormatVersion: 2
guid: d97a1a7207dc34e8496c7726c18c243b
MonoImporter:
  externalObjects: {}
  serializedVersion: 2
  defaultReferences: []
  executionOrder: 0
  icon: {instanceID: 0}
  userData: 
  assetBundleName: 
  assetBundleVariant:
```

### Ui.meta

```txt
fileFormatVersion: 2
guid: 769a458c98e7942df90471a6eca5f210
folderAsset: yes
DefaultImporter:
  externalObjects: {}
  userData: 
  assetBundleName: 
  assetBundleVariant:
```

### SpriteSheet.meta

```txt
fileFormatVersion: 2
guid: f6919c528466c4a19817b2c33468f2ed
folderAsset: yes
DefaultImporter:
  externalObjects: {}
  userData: 
  assetBundleName: 
  assetBundleVariant:
```

### Character.meta

```txt
fileFormatVersion: 2
guid: f5cc0e698b25347a3b89200b64153154
folderAsset: yes
DefaultImporter:
  externalObjects: {}
  userData: 
  assetBundleName: 
  assetBundleVariant:
```

### Main.cs

```csharp
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
```

### Comm.meta

```txt
fileFormatVersion: 2
guid: d0d672151b33d476e9fc55859490ac43
folderAsset: yes
DefaultImporter:
  externalObjects: {}
  userData: 
  assetBundleName: 
  assetBundleVariant:
```

### GameState.cs.meta

```txt
fileFormatVersion: 2
guid: df90db321c5314bb1ab24292cafb3c9b
MonoImporter:
  externalObjects: {}
  serializedVersion: 2
  defaultReferences: []
  executionOrder: 0
  icon: {instanceID: 0}
  userData: 
  assetBundleName: 
  assetBundleVariant:
```

### ObjectManager.cs.meta

```txt
fileFormatVersion: 2
guid: 9997b213693ae49d5804844582c25e91
MonoImporter:
  externalObjects: {}
  serializedVersion: 2
  defaultReferences: []
  executionOrder: 0
  icon: {instanceID: 0}
  userData: 
  assetBundleName: 
  assetBundleVariant:
```

### Ui/UiCharacterCreation.cs.meta

```txt
fileFormatVersion: 2
guid: a8f095b6d6c7a4e0d91c9ad2b78bb5fd
MonoImporter:
  externalObjects: {}
  serializedVersion: 2
  defaultReferences: []
  executionOrder: 0
  icon: {instanceID: 0}
  userData: 
  assetBundleName: 
  assetBundleVariant:
```

### Ui/UiCharacterCreation.cs

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Threading.Tasks;

public class UiCharacterCreation : MonoBehaviour
{
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private TMP_InputField characterDescriptionInput;
    [SerializeField] private Button submitButton;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private GameObject presetButtonsContainer; // Assign this in the inspector
    [SerializeField] private GameObject presetButtonPrefab; // Assign preset button prefab in inspector
    [SerializeField] private GameObject loadingWarningObject; // Assign this in the inspector
    [SerializeField] private TextMeshProUGUI loadingWarningText; // Assign this in the inspector
    private Coroutine loadingWarningCoroutine;
    private bool isPopulatingPresets = false;

    private string spriteSheetName;
    private bool isWaitingForSpriteSheet = false;
    
    void Start()
    {
        if (usernameInput == null) Debug.LogError("Username Input Field is not assigned in the inspector");
        if (characterDescriptionInput == null) Debug.LogError("Character Description Input Field is not assigned in the inspector");
        if (submitButton == null) Debug.LogError("Submit Button is not assigned in the inspector");
        if (buttonText == null) Debug.LogError("Button Text is not assigned in the inspector");
        if (presetButtonsContainer == null) Debug.LogError("Preset Buttons Container is not assigned in the inspector");
        if (presetButtonPrefab == null)  Debug.LogError("Preset Button Prefab is not assigned in the inspector");
        if (loadingWarningObject == null) Debug.LogError("Loading Warning Object is not assigned in the inspector");
        if (loadingWarningText == null) Debug.LogError("Loading Warning Text is not assigned in the inspector");

        loadingWarningObject.SetActive(false);
        
        submitButton.onClick.AddListener(OnSubmitButtonClicked);

        // Start populating preset buttons
        StartCoroutine(PopulatePresetButtons());
    }
    
    void Update()
    {
        if (isWaitingForSpriteSheet && !string.IsNullOrEmpty(spriteSheetName))
        {
            var spriteArray = SpriteSheetManager.instance.GetSpriteArray(spriteSheetName);
            if (spriteArray != null)
            {
                isWaitingForSpriteSheet = false;
                gameObject.SetActive(false); // Hide character creation UI
                Main.instance.PlayerCharacterGeneratedSuccessfully(spriteSheetName);
            }
        }
        
        // widen screen for mobile
        if(Main.instance.isPortrait) 
        {
            // 98% width
            RectTransform rt = GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(Screen.width * 0.98f, rt.sizeDelta.y);
        }
    }

    private IEnumerator PopulatePresetButtons()
    {
        if (isPopulatingPresets) yield break;
        
        isPopulatingPresets = true;
        
        // Keep checking until we get sprite sheet names
        string[] spriteSheetNames = new string[0];
        while (spriteSheetNames.Length == 0)
        {
            spriteSheetNames = SpriteSheetManager.instance.GetSpriteSheetNames();
            if (spriteSheetNames.Length == 0)
                yield return new WaitForSeconds(0.5f);
        }
        
        // Clear existing buttons
        foreach (Transform child in presetButtonsContainer.transform)
            if(child.name != "Header") Destroy(child.gameObject);
        
        // Create a button for each sprite sheet
        for (int i = 0; i < spriteSheetNames.Length; i++)
        {
            string name = spriteSheetNames[i];
            GameObject buttonObj = Instantiate(presetButtonPrefab, presetButtonsContainer.transform);
            
            // Find the TextMeshProUGUI component (could be on child object)
            TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
                buttonText.text = name;
            
            // Find the Button component which is on a child of the prefab
            Button button = buttonObj.GetComponentInChildren<Button>();
            if (button != null)
            {
                // Store name in local variable to avoid closure issues
                string sheetName = name;
                
                // Add click handler
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => {
                    Debug.Log($"Button clicked: {sheetName}");
                    HandlePresetButtonClick(sheetName);
                });
                
                Debug.Log($"Successfully added click listener to button for {sheetName}");
            }
            else
            {
                Debug.LogError($"Button component not found on any child of the prefab for {name}");
            }
        }
        
        isPopulatingPresets = false;
    }

    private void HandlePresetButtonClick(string spriteSheetName)
    {
        Debug.Log($"Handling click for {spriteSheetName}");
        Main.instance.PlayerSelectedPreSetCharacter(spriteSheetName);
        gameObject.SetActive(false);
    }

    private void OnPresetButtonClicked(string spriteSheetName)
    {
        Debug.Log($"Preset button clicked: {spriteSheetName}");

        // Call to main instance with the selected preset
        Main.instance.PlayerSelectedPreSetCharacter(spriteSheetName);
        
        // Hide character creation UI
        gameObject.SetActive(false);
    }
    
    private async void OnSubmitButtonClicked()
    {
        // Validate inputs
        if (string.IsNullOrEmpty(usernameInput.text))
        {
            loadingWarningText.text = "Username cannot be empty";
            loadingWarningObject.SetActive(true);
            return;
        }
        if (string.IsNullOrEmpty(characterDescriptionInput.text)) 
        {
            Debug.LogError("Character description cannot be empty");
            loadingWarningText.text = "Character description cannot be empty";
            loadingWarningObject.SetActive(true);
            return;
        }
        
        // Update button text
        buttonText.text = "Please wait...";
        submitButton.interactable = false;

        // Start the warning timer
        if(loadingWarningCoroutine != null)
            StopCoroutine(loadingWarningCoroutine);
        loadingWarningCoroutine = StartCoroutine(ShowLoadingWarningAfterDelay(10f));
        
        
        // Make API call to generate sprite sheet
        string filename = await new LoadJsonResource().GenerateSpriteSheet(characterDescriptionInput.text);
        
        if (!string.IsNullOrEmpty(filename))
        {
            // Update button text
            buttonText.text = "Downloading...";
            
            // Store sprite sheet name
            spriteSheetName = filename;
            
            // Start downloading the sprite sheet
            SpriteSheetManager.instance.DownloadSpriteSheet(spriteSheetName);
            
            // Start checking for sprite sheet to be loaded
            isWaitingForSpriteSheet = true;
        }
        else
        {
            // Error handling
            Debug.LogError("Failed to generate sprite sheet");
            buttonText.text = "Failed! Try again";
            submitButton.interactable = true;
        }
    }

    private IEnumerator ShowLoadingWarningAfterDelay(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        
        // Show the warning object after the delay
        loadingWarningObject.SetActive(true);
        
        Debug.Log("Showing loading warning after " + delayInSeconds + " seconds");
    }
}
```

### Character/Appearance.meta

```txt
fileFormatVersion: 2
guid: df9dbdce84b5340e990a53bad3a8d312
folderAsset: yes
DefaultImporter:
  externalObjects: {}
  userData: 
  assetBundleName: 
  assetBundleVariant:
```

### Character/Controls.meta

```txt
fileFormatVersion: 2
guid: 6913e144a92de4963a5c4f20ec28c5cf
folderAsset: yes
DefaultImporter:
  externalObjects: {}
  userData: 
  assetBundleName: 
  assetBundleVariant:
```

### Character/Appearance/CharacterAnimate.cs

```csharp
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimate : CharacterAppearance
{
    // Current animation state
    [SerializeField] private CharacterAnimation currentAnimation = CharacterAnimation.StandFront;
    
    // Animation frame data - maps animation states to sequences of sprite indices
    private Dictionary<CharacterAnimation, int[]> animations = new Dictionary<CharacterAnimation, int[]>()
    {
        { CharacterAnimation.WalkLeft, new int[] { 12, 13, 14, 15 } },
        { CharacterAnimation.WalkRight, new int[] { 4, 5, 6, 7 } },
        { CharacterAnimation.WalkFront, new int[] { 8, 9, 10, 11 } },
        { CharacterAnimation.WalkBack, new int[] { 0, 1, 2, 3 } },
        { CharacterAnimation.StandBack, new int[] { 1 } },
        { CharacterAnimation.StandRight, new int[] { 5 } },
        { CharacterAnimation.StandLeft, new int[] { 13 } },
        { CharacterAnimation.StandFront, new int[] { 9 } },
        { CharacterAnimation.FallBack, new int[] { 2 } },
        { CharacterAnimation.FallRight, new int[] { 6 } },
        { CharacterAnimation.FallLeft, new int[] { 14 } },
        { CharacterAnimation.FallFront, new int[] { 10 } }
    };
    
    // Animation properties
    [SerializeField] private float frameRate; // 6
    private float frameTimer = 0f;
    private int currentFrame = 0;
    
    // Animation enabled flag
    [SerializeField] private bool animationEnabled = true;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start() 
    {
        base.Start();
    }
    
    protected override void Update()
    {
        // Handle V key for debugging
        if (Input.GetKeyDown(KeyCode.V) && Main.instance.vChangesCharacterAnimation)
        {
            // random animation
            CharacterAnimation[] animations = (CharacterAnimation[])System.Enum.GetValues(typeof(CharacterAnimation));
            SetAnimation(animations[Random.Range(0, animations.Length)]);
        }
        
        // Handle animation if enabled
        if (animationEnabled)
        {
            UpdateAnimation();
        }

        base.Update();
    }
    
    // Update the animation frame based on time
    private void UpdateAnimation()
    {
        // Only animate if we have frames for the current animation
        if (animations.TryGetValue(currentAnimation, out int[] frames) && frames.Length > 1)
        {
            // Update timer
            frameTimer += Time.deltaTime;
            
            // Time to change to the next frame?
            if (frameTimer >= 1f / frameRate)
            {
                // Reset timer and advance to next frame
                frameTimer = 0f;
                currentFrame = (currentFrame + 1) % frames.Length;
                
                // Apply the new sprite index
                SetSpriteIndex(frames[currentFrame]);
            }
        }
    }
    
    // Change the current animation state
    public void SetAnimation(CharacterAnimation animation)
    {
        // Only change if it's different
        if (currentAnimation != animation)
        {
            currentAnimation = animation;
            currentFrame = 0; // Reset to first frame
            frameTimer = 0f;  // Reset timer
            
            // Apply first frame of new animation
            if (animations.TryGetValue(animation, out int[] frames) && frames.Length > 0)
            {
                SetSpriteIndex(frames[0]);
            }
        }
    }
    
    // Get the current animation state
    public CharacterAnimation GetCurrentAnimation()
    {
        return currentAnimation;
    }
    
    // Enable or disable animation
    public void SetAnimationEnabled(bool enabled)
    {
        animationEnabled = enabled;
    }
    
    public void StandInDirection(Vector2 direction)
    {
        // Determine closest cardinal direction
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0) SetAnimation(CharacterAnimation.StandRight);
            else SetAnimation(CharacterAnimation.StandLeft);
        }
        else
        {
            if (direction.y > 0) SetAnimation(CharacterAnimation.StandBack);
            else SetAnimation(CharacterAnimation.StandFront);
        }
    }

    public void FallInDirection(Vector2 direction)
    {
        // Determine closest cardinal direction
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) 
        {
            if (direction.x > 0) SetAnimation(CharacterAnimation.FallRight);
            else SetAnimation(CharacterAnimation.FallLeft);
        }
        else
        {
            if (direction.y > 0) SetAnimation(CharacterAnimation.FallBack);
            else SetAnimation(CharacterAnimation.FallFront);
        }
    }
    
    public void WalkInDirection(Vector2 direction)
    {
        // Convert direction to the closest cardinal direction
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // Horizontal movement dominant
            if (direction.x > 0)
                SetAnimation(CharacterAnimation.WalkRight);
            else
                SetAnimation(CharacterAnimation.WalkLeft);
        }
        else
        {
            // Vertical movement dominant
            if (direction.y > 0)
                SetAnimation(CharacterAnimation.WalkBack);
            else
                SetAnimation(CharacterAnimation.WalkFront);
        }
    }
}
```

### Character/Appearance/CharacterAnimate.cs.meta

```txt
fileFormatVersion: 2
guid: 3d0487b8254ea42baa66bbe0b8d9cd87
MonoImporter:
  externalObjects: {}
  serializedVersion: 2
  defaultReferences: []
  executionOrder: 0
  icon: {instanceID: 0}
  userData: 
  assetBundleName: 
  assetBundleVariant:
```

### Character/Appearance/CharacterAnimation.cs

```csharp
public enum CharacterAnimation
{
    StandFront,
    StandBack,
    StandLeft,
    StandRight,
    WalkFront,
    WalkBack,
    WalkLeft,
    WalkRight,
    FallFront,
    FallBack,
    FallLeft,
    FallRight
}
```

### Character/Appearance/CharacterAnimation.cs.meta

```txt
fileFormatVersion: 2
guid: 1371df3eb334c40cdabb4f07c87b36fc
MonoImporter:
  externalObjects: {}
  serializedVersion: 2
  defaultReferences: []
  executionOrder: 0
  icon: {instanceID: 0}
  userData: 
  assetBundleName: 
  assetBundleVariant:
```

### Character/Appearance/CharacterAppearance.cs

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAppearance : MonoBehaviour
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
```

### Character/Appearance/CharacterAppearance.cs.meta

```txt
fileFormatVersion: 2
guid: 44d5ab3fa8c8d4bc883d1ca864b90762
MonoImporter:
  externalObjects: {}
  serializedVersion: 2
  defaultReferences: []
  executionOrder: 0
  icon: {instanceID: 0}
  userData: 
  assetBundleName: 
  assetBundleVariant:
```

### Character/Controls/Player.cs.meta

```txt
fileFormatVersion: 2
guid: 5b4cf9c83a9254b558a4ceec7d3daa66
MonoImporter:
  externalObjects: {}
  serializedVersion: 2
  defaultReferences: []
  executionOrder: 0
  icon: {instanceID: 0}
  userData: 
  assetBundleName: 
  assetBundleVariant:
```

### Character/Controls/CharacterMovement.cs.meta

```txt
fileFormatVersion: 2
guid: 243a6ff73ec4e4a27acbd663005bd97e
MonoImporter:
  externalObjects: {}
  serializedVersion: 2
  defaultReferences: []
  executionOrder: 0
  icon: {instanceID: 0}
  userData: 
  assetBundleName: 
  assetBundleVariant:
```

### Character/Controls/CharacterMovement.cs

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : CharacterAnimate
{
    // Movement properties
    [Header("Movement Settings")]
    [SerializeField] private float MOVE_SPEED = 5f;
    [SerializeField] private float ROTATION_SPEED = 10f;
    [SerializeField] protected float JUMP_FORCE = 5f;
    [SerializeField] private float GRAVITY = 20f;
    
    // Movement state
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 velocity = Vector3.zero;
    protected bool isGrounded = false;
    protected bool jumpPressed = false;
    private Vector2 lastMovementDirection = Vector2.down; 

    
    // Components
    private CharacterController characterController;
    
    protected override void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (characterController == null) Debug.LogError("CharacterController component required for CharacterMovement");

        base.Start();

        characterController.height = 0.5f;
        characterController.radius = 0.08f;
    }
    
    protected void MoveCharacter()
    {
        // Calculate movement vector (horizontal only)
        Vector3 horizontalMovement = moveDirection * MOVE_SPEED * Time.fixedDeltaTime;
        
        // Combine with vertical velocity for complete movement
        Vector3 finalMovement = new Vector3(horizontalMovement.x, velocity.y * Time.fixedDeltaTime, horizontalMovement.z);
        
        // Apply movement (includes both horizontal and vertical)
        characterController.Move(finalMovement);
        
        // Adjust rotation to face movement direction
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, ROTATION_SPEED * Time.fixedDeltaTime);
        }
    }

    // Modify HandleGravity to not move the character
    protected void HandleGravity()
    {   
        // Apply gravity when not grounded
        if (isGrounded) velocity.y = -0.5f; // keep character grounded
        else velocity.y -= GRAVITY * Time.fixedDeltaTime; // full gravity
    }

    // Modify FixedUpdate to ensure MoveCharacter is always called
    protected virtual void FixedUpdate()
    {
        // Check if character is grounded
        isGrounded = characterController.isGrounded;

        // Handle gravity and ground detection
        if(!jumpPressed) HandleGravity();
        
        // Apply movement (always, not just when horizontal movement exists)
        MoveCharacter();
        
        // Animation logic stays the same
        if (moveDirection.magnitude > 0.1f)
        {
            // Character is moving
            // Set walking animation based on movement direction
            Vector2 moveDir2D = new Vector2(moveDirection.x, moveDirection.z);
            
            // Store this as our last significant movement direction
            lastMovementDirection = moveDir2D;
            
            WalkInDirection(moveDir2D);
        }
        else
        {
            // Character is standing still
            if (isGrounded)
            {
                // Use the stored last movement direction instead of transform.forward
                StandInDirection(lastMovementDirection);
            }
            else
            {
                // Character is falling or jumping
                FallInDirection(lastMovementDirection);
            }
        }
    }
    
    // Set the movement direction (to be called by derived classes)
    public virtual void SetMovementDirection(Vector3 direction)
    {
        // Normalize horizontal components of direction
        Vector3 horizontalDir = new Vector3(direction.x, 0, direction.z);
        
        if (horizontalDir.magnitude > 1f)
            horizontalDir.Normalize();
            
        moveDirection = horizontalDir;
    }
    
    // Initiate or continue a jump
    public virtual void Jump()
    {
        Debug.Log("Jumping");
        jumpPressed = true;
        if (isGrounded) velocity.y = JUMP_FORCE;
    }
    
    // Stop all movement
    public virtual void StopMovement()
    {
        moveDirection = Vector3.zero;
    }
    
    // Check if character is moving
    public bool IsMoving()
    {
        return moveDirection.magnitude > 0.1f;
    }
    
    // Get current movement speed
    public float GetCurrentSpeed()
    {
        return moveDirection.magnitude * MOVE_SPEED;
    }

    protected override void Update()
    {
        base.Update();
    }
}
```

### Character/Controls/Player.cs

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Player : CharacterMovement
{
    // Input references
    private InputAction moveAction;
    private InputAction jumpAction;
    private float MAX_JUMP_TIME = 0.2f;
    private float jumpCounter = 0;
    
    // Camera reference for movement relative to camera
        
    private Transform cameraTransform;
    
    protected override void Start()
    {
        base.Start();
        
        // Get the PlayerInput component
        PlayerInput playerInput = GetComponent<PlayerInput>();
        
        // Get the move and jump actions
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        
        // Set up camera reference
        if(Camera.main == null || Camera.main.transform == null) Debug.LogError("No camera found. Player movement will be in world space.");
        else cameraTransform = Camera.main.transform;
    }
    
    protected override void FixedUpdate()
    {
        // Process input
        ProcessMoveInput();

        // Start Jumping
        if (Input.GetButton("Jump") && isGrounded) 
        {
            jumpPressed = true;
            jumpCounter = 0; // let player jump
        }
        else if(!Input.GetButton("Jump") && !isGrounded) 
        {
            jumpPressed = false; // player let go of jump key
        }

        // Continue Jumping
        if (jumpPressed && jumpCounter < MAX_JUMP_TIME) 
        {
            jumpCounter += Time.fixedDeltaTime; // player is still holding jump key, allow for more jump
            Jump(); // CharacterMovement.cs will apply force upward
        }
        
        // Let the base class handle movement and animation
        base.FixedUpdate();
    }
    
    private void ProcessMoveInput()
    {
        // Get move input from action
        Vector2 input = moveAction.ReadValue<Vector2>();
        
        // Convert input to 3D movement vector
        Vector3 move = new Vector3(input.x, 0, input.y);
        
        if (move.magnitude > 0.1f)
        {
            // If we have a camera, make movement relative to camera angle
            if (cameraTransform != null)
            {
                // Calculate camera forward and right vectors projected onto XZ plane
                Vector3 forward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
                Vector3 right = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;
                
                // Recalculate move direction relative to camera
                move = forward * input.y + right * input.x;
            }
            
            // Set the movement direction
            SetMovementDirection(move);
        }
        else
        {
            // No input, stop movement
            StopMovement();
        }
    }
}
```

### Comm/GameInitData.cs.meta

```txt
fileFormatVersion: 2
guid: 15ce5412f38c74007b18e61ae4258a21
MonoImporter:
  externalObjects: {}
  serializedVersion: 2
  defaultReferences: []
  executionOrder: 0
  icon: {instanceID: 0}
  userData: 
  assetBundleName: 
  assetBundleVariant:
```

### Comm/LoadJsonResource.cs

```csharp
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
```

### Comm/LoadJsonResource.cs.meta

```txt
fileFormatVersion: 2
guid: 00f342d794fcc4ddb89529a107c92598
MonoImporter:
  externalObjects: {}
  serializedVersion: 2
  defaultReferences: []
  executionOrder: 0
  icon: {instanceID: 0}
  userData: 
  assetBundleName: 
  assetBundleVariant:
```

### Comm/GameInitData.cs

```csharp
public class GameInitData
{
    public string[] spriteSheetFiles;
}
```

### SpriteSheet/SpriteSheetAsset.cs

```csharp
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;


public class SpriteSheetAsset
{
    public string name { get; private set; }
    public Texture2D texture { get; private set; }
    public Sprite[] sprites { get; private set; }
    public int rows { get; private set; }
    public int columns { get; private set; }
    
    public SpriteSheetAsset(string name, Texture2D texture, Sprite[] sprites, int rows, int columns)
    {
        this.name = name;
        this.texture = texture;
        this.sprites = sprites;
        this.rows = rows;
        this.columns = columns;
    }
}
```

### SpriteSheet/SpriteSheetManager.cs.meta

```txt
fileFormatVersion: 2
guid: b16292cac2c724ce29fe3266d8601960
MonoImporter:
  externalObjects: {}
  serializedVersion: 2
  defaultReferences: []
  executionOrder: 0
  icon: {instanceID: 0}
  userData: 
  assetBundleName: 
  assetBundleVariant:
```

### SpriteSheet/SpriteSheetAsset.cs.meta

```txt
fileFormatVersion: 2
guid: 150b26b15074645758e12d4b9a40d8dd
MonoImporter:
  externalObjects: {}
  serializedVersion: 2
  defaultReferences: []
  executionOrder: 0
  icon: {instanceID: 0}
  userData: 
  assetBundleName: 
  assetBundleVariant:
```

### SpriteSheet/SpriteSheetManager.cs

```csharp
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
        
        Debug.Log($"Loaded {SpriteSheetCache.Count} SpriteSheets into cache");
    }
    
    public async Task DownloadSpriteSheet(string filename)
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
    
    // Get a specific SpriteSheet's sprite array by filename or name
    public Sprite[] GetSpriteArray(string name)
    {
        // convert filename to name's
        if (name.EndsWith(".png") || name.EndsWith(".jpg") || name.EndsWith(".jpeg") || name.EndsWith(".gif") || name.EndsWith(".bmp")) name = name.Substring(0, name.LastIndexOf('.'));

        // check fo asset in cache
        if (SpriteSheetCache.TryGetValue(name, out SpriteSheetAsset asset))
            return asset.sprites;
            
        Debug.LogWarning($"SpriteSheet {name} not found in cache");
        return null;
    }

    public String[] GetSpriteSheetNames()
    {
        return new List<string>(SpriteSheetCache.Keys).ToArray();
    }
}
```

> This concludes the repository's file contents. Please review thoroughly for a comprehensive understanding of the codebase.
