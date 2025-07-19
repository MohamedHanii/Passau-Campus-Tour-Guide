using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;

public class ARSceneSetup : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject validationPanelPrefab;
    
    [Header("Components")]
    [SerializeField] private ARTrackedImageManager trackedImageManager;
    [SerializeField] private BuildingRoomValidator buildingValidator;
    
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private TextMeshProUGUI roomNumberText;
    [SerializeField] private GameObject backButton;
    
    [Header("Room Input UI")]
    [SerializeField] private GameObject homePanel;
    [SerializeField] private TMP_InputField roomInputField;
    [SerializeField] private UnityEngine.UI.Button startARButton;
    private bool arStarted = false;
    
    void Start()
    {
        if (homePanel != null && roomInputField != null && startARButton != null)
        {
            homePanel.SetActive(true);
            GameObject validationPanelObj = GameObject.Find("ValidationPanel");
            if (validationPanelObj != null)
                validationPanelObj.SetActive(false);
            if (backButton != null)
                backButton.SetActive(false);
            if (trackedImageManager != null)
                trackedImageManager.enabled = false;
            startARButton.onClick.AddListener(OnStartARClicked);
        }
        else
        {
            // Do not instantiate or enable validation UI here
        }
    }

    private void OnStartARClicked()
    {
        if (arStarted) return;
        arStarted = true;
        string roomNumber = roomInputField.text;
        if (buildingValidator != null && !string.IsNullOrEmpty(roomNumber))
        {
            buildingValidator.SetCurrentRoom(roomNumber);
        }
        homePanel.SetActive(false);
        if (backButton != null)
            backButton.SetActive(true);
        if (trackedImageManager != null)
            trackedImageManager.enabled = true;
    }
    
    private void SetupARScene()
    {
        if (trackedImageManager == null)
        {
            trackedImageManager = FindObjectOfType<ARTrackedImageManager>();
        }
        if (buildingValidator == null)
        {
            buildingValidator = FindObjectOfType<BuildingRoomValidator>();
            if (buildingValidator == null)
            {
                GameObject validatorGO = new GameObject("BuildingRoomValidator");
                buildingValidator = validatorGO.AddComponent<BuildingRoomValidator>();
            }
        }
        if (buildingValidator != null)
        {
            var validatorField = typeof(BuildingRoomValidator).GetField("trackedImageManager", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (validatorField != null)
            {
                validatorField.SetValue(buildingValidator, trackedImageManager);
            }
        }
        Debug.Log("AR Scene setup completed!");
    }
    
    private void SetupDefaultRooms()
    {
        if (buildingValidator != null)
        {
            Debug.Log("Default rooms are now hardcoded for building 43");
        }
    }

    public void OnBackButtonPressed()
    {
        Debug.Log("Back button pressed!");
        GameObject validationPanelObj = GameObject.Find("ValidationPanel");
        if (validationPanelObj != null)
            validationPanelObj.SetActive(false);
        if (homePanel != null)
            homePanel.SetActive(true);
        if (backButton != null)
            backButton.SetActive(false);
        if (trackedImageManager != null)
            trackedImageManager.enabled = false;
        arStarted = false;
    }
} 