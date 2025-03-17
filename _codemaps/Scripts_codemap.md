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
    - [Character/CharacterAnimate.cs](#charactercharacteranimatecs)
    - [Character/CharacterAnimate.cs.meta](#charactercharacteranimatecsmeta)
    - [Character/CharacterAnimation.cs](#charactercharacteranimationcs)
    - [Character/CharacterAnimation.cs.meta](#charactercharacteranimationcsmeta)
    - [Character/CharacterAppearance.cs](#charactercharacterappearancecs)
    - [Character/CharacterAppearance.cs.meta](#charactercharacterappearancecsmeta)
    - [Character/Controls.meta](#charactercontrolsmeta)
    - [Character/Controls/CharacterMovement.cs](#charactercontrolscharactermovementcs)
    - [Character/Controls/CharacterMovement.cs.meta](#charactercontrolscharactermovementcsmeta)
    - [Character/Controls/Player.cs](#charactercontrolsplayercs)
    - [Character/Controls/Player.cs.meta](#charactercontrolsplayercsmeta)
    - [Main.cs](#maincs)
    - [Main.cs.meta](#maincsmeta)
    - [ObjectManager.cs](#objectmanagercs)
    - [ObjectManager.cs.meta](#objectmanagercsmeta)
    - [SpriteSheet.meta](#spritesheetmeta)
    - [SpriteSheet/SpriteSheetAsset.cs](#spritesheetspritesheetassetcs)
    - [SpriteSheet/SpriteSheetAsset.cs.meta](#spritesheetspritesheetassetcsmeta)
    - [SpriteSheet/SpriteSheetManager.cs](#spritesheetspritesheetmanagercs)
    - [SpriteSheet/SpriteSheetManager.cs.meta](#spritesheetspritesheetmanagercsmeta)
    - [Start.meta](#startmeta)
    - [Start/GameInitData.cs](#startgameinitdatacs)
    - [Start/GameInitData.cs.meta](#startgameinitdatacsmeta)
    - [Start/GameState.cs](#startgamestatecs)
    - [Start/GameState.cs.meta](#startgamestatecsmeta)
    - [Start/LoadJsonResource.cs](#startloadjsonresourcecs)
    - [Start/LoadJsonResource.cs.meta](#startloadjsonresourcecsmeta)

<!-- /TOC -->

## Repo File Tree

This file tree represents the actual structure of the repository. It's crucial for understanding the organization of the codebase.

```tree
.
├── Character/
│   ├── Controls/
│   │   ├── CharacterMovement.cs
│   │   ├── CharacterMovement.cs.meta
│   │   ├── Player.cs
│   │   └── Player.cs.meta
│   ├── CharacterAnimate.cs
│   ├── CharacterAnimate.cs.meta
│   ├── CharacterAnimation.cs
│   ├── CharacterAnimation.cs.meta
│   ├── CharacterAppearance.cs
│   ├── CharacterAppearance.cs.meta
│   └── Controls.meta
├── SpriteSheet/
│   ├── SpriteSheetAsset.cs
│   ├── SpriteSheetAsset.cs.meta
│   ├── SpriteSheetManager.cs
│   └── SpriteSheetManager.cs.meta
├── Start/
│   ├── GameInitData.cs
│   ├── GameInitData.cs.meta
│   ├── GameState.cs
│   ├── GameState.cs.meta
│   ├── LoadJsonResource.cs
│   └── LoadJsonResource.cs.meta
├── Character.meta
├── Main.cs
├── Main.cs.meta
├── ObjectManager.cs
├── ObjectManager.cs.meta
├── SpriteSheet.meta
└── Start.meta

4 directories, 28 files
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

### Start.meta

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

            // Start the game once everything is laded
            StartGame();
    }

    private void StartGame()
    {
        gameState = GameState.Playing;
        Debug.Log("Game initialized and ready to play!");

        // create a character
        ObjectManager.instance.CreateObject("Player", "red-sorceress", true);
    }
}
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

### Character/CharacterAnimate.cs

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
        { CharacterAnimation.StandFront, new int[] { 9 } }
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
    
    // Helper methods for common state changes
    public void StandInDirection(Vector2 direction)
    {
        // Convert direction to the closest cardinal direction
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // Horizontal movement dominant
            if (direction.x > 0)
                SetAnimation(CharacterAnimation.StandRight);
            else
                SetAnimation(CharacterAnimation.StandLeft);
        }
        else
        {
            // Vertical movement dominant
            if (direction.y > 0)
                SetAnimation(CharacterAnimation.StandBack);
            else
                SetAnimation(CharacterAnimation.StandFront);
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

### Character/CharacterAnimate.cs.meta

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

### Character/CharacterAnimation.cs

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
    WalkRight
}
```

### Character/CharacterAnimation.cs.meta

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

### Character/CharacterAppearance.cs

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

### Character/CharacterAppearance.cs.meta

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
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = 20f;
    
    // Movement state
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 velocity = Vector3.zero;
    private bool isGrounded = false;
    
    // Components
    private CharacterController characterController;
    
    protected override void Start()
    {
        // Get components
        characterController = GetComponent<CharacterController>();
        
        if (characterController == null)
        {
            Debug.LogError("CharacterController component required for CharacterMovement");
            enabled = false;
        }

        base.Start();
    }
    
    protected virtual void FixedUpdate()
    {
        // Handle gravity and ground detection
        HandleGravity();
        
        // Apply movement
        if (moveDirection.magnitude > 0.1f)
        {
            // Character is moving
            MoveCharacter();
            
            // Set walking animation based on movement direction
            Vector2 moveDir2D = new Vector2(moveDirection.x, moveDirection.z);
            WalkInDirection(moveDir2D);
        }
        else
        {
            // Character is standing still
            if (isGrounded)
            {
                // Set standing animation based on last movement direction
                Vector2 lastDir2D = new Vector2(transform.forward.x, transform.forward.z);
                StandInDirection(lastDir2D);
            }
        }
    }
    
    protected void HandleGravity()
    {
        // Check if character is grounded
        isGrounded = characterController.isGrounded;
        
        // Apply gravity when not grounded
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -0.5f; // Small negative value to keep character grounded
        }
        else
        {
            velocity.y -= gravity * Time.fixedDeltaTime;
        }
        
        // Apply velocity
        characterController.Move(velocity * Time.fixedDeltaTime);
    }
    
    // Move the character based on moveDirection
    protected void MoveCharacter()
    {
        // Calculate movement vector
        Vector3 movement = moveDirection * moveSpeed * Time.fixedDeltaTime;
        
        // Apply movement
        characterController.Move(movement);
        
        // Adjust rotation to face movement direction
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
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
    
    // Initiate a jump (to be called by derived classes)
    public virtual void Jump()
    {
        if (isGrounded)
        {
            velocity.y = jumpForce;
        }
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
        return moveDirection.magnitude * moveSpeed;
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
        ProcessJumpInput();
        
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
    
    private void ProcessJumpInput()
    {
        // Check for jump input
        if (jumpAction.triggered)
        {
            Jump();
        }
    }
    
    // These methods can be called by Unity Input System events
    public void OnMove(InputValue value)
    {
        // Optional: You can handle input here instead of in FixedUpdate
        // Vector2 input = value.Get<Vector2>();
    }
    
    public void OnJump(InputValue value)
    {
        // Optional: You can handle jump here
        // if (value.isPressed) Jump();
    }
}
```

### Start/GameInitData.cs.meta

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

### Start/GameState.cs

```csharp
public enum GameState
{
    None,       // Initial state
    Loading,    // Loading resources/assets
    Playing,    // Gameplay active
}
```

### Start/LoadJsonResource.cs

```csharp
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
```

### Start/LoadJsonResource.cs.meta

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

### Start/GameInitData.cs

```csharp
public class GameInitData
{
    public string[] spriteSheetFiles;
}
```

### Start/GameState.cs.meta

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
```

> This concludes the repository's file contents. Please review thoroughly for a comprehensive understanding of the codebase.
