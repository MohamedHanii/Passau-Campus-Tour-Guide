using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestSceneManager : MonoBehaviour
{
    [Header("Test UI")]
    [SerializeField] private GameObject testPanel;
    [SerializeField] private TMP_InputField roomInputField;
    [SerializeField] private Button testButton;
    [SerializeField] private Button addInsideButton;
    [SerializeField] private Button addOutsideButton;
    [SerializeField] private Button toggleARButton;
    
    [Header("AR Simulation")]
    [SerializeField] private BuildingRoomValidator buildingValidator;
    [SerializeField] private GameObject arSimulationObject;
    
    private bool isARSimulated = false;
    
    void Start()
    {
        SetupTestUI();
        SetupDefaultRooms();
    }
    
    void Update()
    {
        // Simulate AR tracking for testing
        if (isARSimulated && buildingValidator != null)
        {
            // Simulate that the image is being tracked
            SimulateARImageTracking();
        }
    }
    
    private void SetupTestUI()
    {
        // Create test panel if it doesn't exist
        if (testPanel == null)
        {
            CreateTestPanel();
        }
        
        // Set up button listeners
        if (testButton != null)
        {
            testButton.onClick.AddListener(TestCurrentRoom);
        }
        
        if (addInsideButton != null)
        {
            addInsideButton.onClick.AddListener(AddRoomToBuilding);
        }
        
        if (addOutsideButton != null)
        {
            addOutsideButton.onClick.AddListener(AddRoomOutsideBuilding);
        }
        
        if (toggleARButton != null)
        {
            toggleARButton.onClick.AddListener(ToggleARSimulation);
        }
        
        // Set initial room number
        if (roomInputField != null)
        {
            roomInputField.text = "101";
        }
    }
    
    private void CreateTestPanel()
    {
        // Create a simple test panel
        GameObject panel = new GameObject("TestPanel");
        panel.transform.SetParent(transform);
        
        Canvas canvas = panel.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;
        
        panel.AddComponent<CanvasScaler>();
        panel.AddComponent<GraphicRaycaster>();
        
        // Create background
        GameObject background = new GameObject("Background");
        background.transform.SetParent(panel.transform);
        
        RectTransform bgRect = background.AddComponent<RectTransform>();
        bgRect.anchorMin = new Vector2(0, 0.8f);
        bgRect.anchorMax = new Vector2(1, 1);
        bgRect.offsetMin = Vector2.zero;
        bgRect.offsetMax = Vector2.zero;
        
        Image bgImage = background.AddComponent<Image>();
        bgImage.color = new Color(0, 0, 0, 0.8f);
        
        // Create input field
        GameObject inputGO = new GameObject("RoomInput");
        inputGO.transform.SetParent(background.transform);
        
        RectTransform inputRect = inputGO.AddComponent<RectTransform>();
        inputRect.anchorMin = new Vector2(0.1f, 0.5f);
        inputRect.anchorMax = new Vector2(0.4f, 0.8f);
        inputRect.offsetMin = Vector2.zero;
        inputRect.offsetMax = Vector2.zero;
        
        TMP_InputField input = inputGO.AddComponent<TMP_InputField>();
        input.text = "101";
        
        // Create buttons
        CreateButton("TestButton", "Test Room", new Vector2(0.45f, 0.5f), new Vector2(0.7f, 0.8f), background.transform, TestCurrentRoom);
        CreateButton("AddInsideButton", "Add Inside", new Vector2(0.75f, 0.5f), new Vector2(0.9f, 0.65f), background.transform, AddRoomToBuilding);
        CreateButton("AddOutsideButton", "Add Outside", new Vector2(0.75f, 0.65f), new Vector2(0.9f, 0.8f), background.transform, AddRoomOutsideBuilding);
        CreateButton("ToggleARButton", "Toggle AR", new Vector2(0.1f, 0.1f), new Vector2(0.3f, 0.4f), background.transform, ToggleARSimulation);
        
        testPanel = panel;
        roomInputField = input;
    }
    
    private void CreateButton(string name, string text, Vector2 anchorMin, Vector2 anchorMax, Transform parent, System.Action onClick)
    {
        GameObject buttonGO = new GameObject(name);
        buttonGO.transform.SetParent(parent);
        
        RectTransform buttonRect = buttonGO.AddComponent<RectTransform>();
        buttonRect.anchorMin = anchorMin;
        buttonRect.anchorMax = anchorMax;
        buttonRect.offsetMin = Vector2.zero;
        buttonRect.offsetMax = Vector2.zero;
        
        Image buttonImage = buttonGO.AddComponent<Image>();
        buttonImage.color = Color.gray;
        
        Button button = buttonGO.AddComponent<Button>();
        button.onClick.AddListener(() => onClick?.Invoke());
        
        // Create text
        GameObject textGO = new GameObject("Text");
        textGO.transform.SetParent(buttonGO.transform);
        
        RectTransform textRect = textGO.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;
        
        TextMeshProUGUI textComponent = textGO.AddComponent<TextMeshProUGUI>();
        textComponent.text = text;
        textComponent.color = Color.white;
        textComponent.alignment = TextAlignmentOptions.Center;
        textComponent.fontSize = 12;
    }
    
    private void SetupDefaultRooms()
    {
        if (buildingValidator != null)
        {
            // No longer adding default rooms dynamically
            Debug.Log("Default rooms are now hardcoded for building 43");
        }
    }
    
    public void TestCurrentRoom()
    {
        if (buildingValidator != null && roomInputField != null)
        {
            string roomNumber = roomInputField.text;
            buildingValidator.SetCurrentRoom(roomNumber);
            Debug.Log($"Testing room: {roomNumber}");
        }
    }
    
    public void AddRoomToBuilding()
    {
        if (buildingValidator != null && roomInputField != null)
        {
            string roomNumber = roomInputField.text;
            // buildingValidator.AddRoomToBuilding(roomNumber); // Method removed
            Debug.Log($"(No longer adding) room {roomNumber} to building");
        }
    }
    
    public void AddRoomOutsideBuilding()
    {
        if (buildingValidator != null && roomInputField != null)
        {
            string roomNumber = roomInputField.text;
            // buildingValidator.AddRoomOutsideBuilding(roomNumber); // Method removed
            Debug.Log($"(No longer adding) room {roomNumber} outside building");
        }
    }
    
    public void ToggleARSimulation()
    {
        isARSimulated = !isARSimulated;
        Debug.Log($"AR Simulation: {(isARSimulated ? "ON" : "OFF")}");
        
        if (toggleARButton != null)
        {
            TextMeshProUGUI buttonText = toggleARButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = isARSimulated ? "AR: ON" : "AR: OFF";
            }
        }
    }
    
    private void SimulateARImageTracking()
    {
        // This simulates the AR image being tracked
        // In a real scenario, this would be handled by the AR Tracked Image Manager
        if (buildingValidator != null)
        {
            // Force the validation to run
            buildingValidator.SendMessage("ValidateCurrentRoom", SendMessageOptions.DontRequireReceiver);
        }
    }
} 