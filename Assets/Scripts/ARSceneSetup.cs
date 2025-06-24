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
    
    void Start()
    {
        SetupARScene();
    }
    
    private void SetupARScene()
    {
        // Find AR components if not assigned
        if (trackedImageManager == null)
        {
            trackedImageManager = FindObjectOfType<ARTrackedImageManager>();
        }
        
        // Create validation UI if it doesn't exist
        if (validationUIPrefab != null)
        {
            GameObject validationUI = Instantiate(validationUIPrefab);
            
            // Find UI components
            if (statusText == null)
            {
                statusText = validationUI.GetComponentInChildren<TextMeshProUGUI>();
            }
            
            if (greenIndicator == null)
            {
                greenIndicator = validationUI.transform.Find("GreenIndicator")?.gameObject;
            }
            
            if (redIndicator == null)
            {
                redIndicator = validationUI.transform.Find("RedIndicator")?.gameObject;
            }
        }
        
        // Find or create building validator
        if (buildingValidator == null)
        {
            buildingValidator = FindObjectOfType<BuildingRoomValidator>();
            
            if (buildingValidator == null)
            {
                GameObject validatorGO = new GameObject("BuildingRoomValidator");
                buildingValidator = validatorGO.AddComponent<BuildingRoomValidator>();
            }
        }
        
        // Configure the building validator
        if (buildingValidator != null)
        {
            // Set up the validator with the found components
            var validatorField = typeof(BuildingRoomValidator).GetField("trackedImageManager", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (validatorField != null)
            {
                validatorField.SetValue(buildingValidator, trackedImageManager);
            }
            
            var uiField = typeof(BuildingRoomValidator).GetField("validationUI", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (uiField != null && validationUIPrefab != null)
            {
                uiField.SetValue(buildingValidator, validationUIPrefab);
            }
            
            var textField = typeof(BuildingRoomValidator).GetField("statusText", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (textField != null)
            {
                textField.SetValue(buildingValidator, statusText);
            }
            
            var greenField = typeof(BuildingRoomValidator).GetField("greenIndicator", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (greenField != null)
            {
                greenField.SetValue(buildingValidator, greenIndicator);
            }
            
            var redField = typeof(BuildingRoomValidator).GetField("redIndicator", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (redField != null)
            {
                redField.SetValue(buildingValidator, redIndicator);
            }
            
            // Set up some default rooms for testing
            SetupDefaultRooms();
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