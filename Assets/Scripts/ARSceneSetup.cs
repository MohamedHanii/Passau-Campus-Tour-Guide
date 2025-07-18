using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;

public class ARSceneSetup : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject validationUIPrefab;
    
    [Header("Components")]
    [SerializeField] private ARTrackedImageManager trackedImageManager;
    [SerializeField] private BuildingRoomValidator buildingValidator;
    
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private GameObject greenIndicator;
    [SerializeField] private GameObject redIndicator;
    
    [Header("Room Input UI")]
    [SerializeField] private GameObject roomInputCanvas;
    [SerializeField] private TMP_InputField roomInputField;
    [SerializeField] private UnityEngine.UI.Button startARButton;
    private bool arStarted = false;
    
    void Start()
    {
        if (roomInputCanvas != null && roomInputField != null && startARButton != null)
        {
            roomInputCanvas.SetActive(true);
            // Hide the ValidationUI GameObject in the scene at start
            GameObject validationUIObj = GameObject.Find("ValidationUI");
            if (validationUIObj != null)
                validationUIObj.SetActive(false);
            if (trackedImageManager != null)
                trackedImageManager.enabled = false;
            startARButton.onClick.AddListener(OnStartARClicked);

            // Set panel background to white if possible
            var panel = roomInputCanvas.GetComponentInChildren<UnityEngine.UI.Image>();
            if (panel != null)
                panel.color = Color.white;
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
            buildingValidator.SetCurrentRoom(roomNumber); // Save the entered room number
        }
        roomInputCanvas.SetActive(false); // Close the input panel
        if (trackedImageManager != null)
            trackedImageManager.enabled = true;
        // Do NOT enable validation UI here; let AR tracking handle it
    }
    
    private void SetupARScene()
    {
        // Do not instantiate or enable validation UI here
        // Only set up references if needed
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
        // Configure the building validator as before
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
            // Add some rooms inside the building (Building 40)
            buildingValidator.AddRoomToBuilding("101");
            buildingValidator.AddRoomToBuilding("102");
            buildingValidator.AddRoomToBuilding("103");
            buildingValidator.AddRoomToBuilding("104");
            buildingValidator.AddRoomToBuilding("105");
            buildingValidator.AddRoomToBuilding("201");
            buildingValidator.AddRoomToBuilding("202");
            buildingValidator.AddRoomToBuilding("203");
            buildingValidator.AddRoomToBuilding("204");
            buildingValidator.AddRoomToBuilding("205");
            
            // Add some rooms outside the building
            buildingValidator.AddRoomOutsideBuilding("301");
            buildingValidator.AddRoomOutsideBuilding("302");
            buildingValidator.AddRoomOutsideBuilding("303");
            buildingValidator.AddRoomOutsideBuilding("401");
            buildingValidator.AddRoomOutsideBuilding("402");
            buildingValidator.AddRoomOutsideBuilding("403");
            
            Debug.Log("Default rooms configured for Building 40");
        }
    }
} 