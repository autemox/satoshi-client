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
            Debug.LogError("Username cannot be empty");
            return;
        }
        if (string.IsNullOrEmpty(characterDescriptionInput.text)) 
        {
            Debug.LogError("Character description cannot be empty");
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