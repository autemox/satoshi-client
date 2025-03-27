using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Threading.Tasks;

public class UiCharacterCreation : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private TMP_InputField characterDescriptionInput;
    [SerializeField] private Button submitButton;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private GameObject presetButtonsContainer; 
    [SerializeField] private GameObject presetButtonPrefab;
    [SerializeField] private GameObject loadingWarningObject; 
    [SerializeField] private TextMeshProUGUI loadingWarningText; 
    private Coroutine loadingWarningCoroutine;
    private bool presetsPopulated = false; // created all the buttons for the presets
    private string spriteSheetName;
    private bool isWaitingForGeneration = false; // currently waiting for retrodiffusion generation

    // singleton
    public static UiCharacterCreation instance { get; private set; }
    void Awake()
    {
        if (instance != null && instance != this) { Destroy(gameObject); return; }
        instance = this;
    }
    
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

        // hide the UI until needed
        HideWindow();
    }
    
    void Update()
    {
        // widen screen for mobile
        if(Main.instance.isPortrait) 
        {
            // 98% width
            RectTransform rt = GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(Screen.width * 0.98f, rt.sizeDelta.y);
        }
    }

    public void ShowWindow()
    {
        if (gameObject.activeSelf) return;

        // load list of recently created characters
        if(!presetsPopulated) PopulatePresetButtons();

        // show the window
        gameObject.SetActive(true);
    }

    public void HideWindow()
    {
        gameObject.SetActive(false);
    }

    private void PopulatePresetButtons()
    {
        string[] spriteSheetNames = SpriteSheetManager.instance.GetSpriteSheetNames();
        if (spriteSheetNames.Length == 0) Debug.LogError("No sprite sheet names found");
        
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
                    OnPresetButtonClicked(sheetName);
                });
                
                Debug.Log($"Successfully added click listener to button for {sheetName}");
            }
            else
            {
                Debug.LogError($"Button component not found on any child of the prefab for {name}");
            }
        }
        
        presetsPopulated = true;
    }

    private void OnPresetButtonClicked(string spriteSheetName)
    {
        // Validate inputs
        if (string.IsNullOrEmpty(usernameInput.text))
        {
            loadingWarningText.text = "Username cannot be empty";
            loadingWarningObject.SetActive(true);
            return;
        }

        Debug.Log($"Handling click for {spriteSheetName}");
        Main.instance.PlayerSelectedCharacter(usernameInput.text, spriteSheetName);
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
        isWaitingForGeneration = true;

        // Start the warning timer (warning reminds user to wait a little longer)
        if(loadingWarningCoroutine != null) StopCoroutine(loadingWarningCoroutine);
        loadingWarningCoroutine = StartCoroutine(ShowLoadingWarningAfterDelay(10f));
        
        // Make API call to generate sprite sheet
        string filename = await new ApiClient().GenerateSpriteSheet(characterDescriptionInput.text);
        
        if (!string.IsNullOrEmpty(filename))
        {
            buttonText.text = "Downloading...";
            await Task.WhenAll(new List<Task>() { SpriteSheetManager.instance.DownloadSpriteSheet(filename) });
            Main.instance.PlayerSelectedCharacter(usernameInput.text, filename);
        }
        else
        {
            // Error handling
            Debug.LogError("Failed to generate sprite sheet");
            buttonText.text = "Failed! Try again";
            submitButton.interactable = true;
        }

        isWaitingForGeneration = false;
    }

    private IEnumerator ShowLoadingWarningAfterDelay(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        
        // Show the warning object after the delay
        loadingWarningObject.SetActive(true);
        
        Debug.Log("Showing loading warning after " + delayInSeconds + " seconds");
    }
}