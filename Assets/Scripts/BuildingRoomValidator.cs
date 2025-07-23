using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;
using UnityEngine.UI;

public class BuildingRoomValidator : MonoBehaviour
{
    [Header("AR Components")]
    [SerializeField] private ARTrackedImageManager trackedImageManager;
    
    [Header("UI Components")]
    [SerializeField] private GameObject validationPanel;
    [SerializeField] private TextMeshProUGUI roomNumberText;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private Button startNavigationButton;
    // Removed debugText
    
    [System.Serializable]
    public class BuildingRooms
    {
        public string buildingLabelName;
        public List<string> roomsInsideBuilding = new List<string>();
        public List<string> roomsOutsideBuilding = new List<string>();
    }
    
    [Header("Building Configurations")]
    public static List<BuildingRooms> buildingConfigs = new List<BuildingRooms> {
        new BuildingRooms {
            buildingLabelName = "43",
            roomsInsideBuilding = new List<string> { "S001", "S002", "S004", "S011", "S252", "C005", "M017" },
            roomsOutsideBuilding = new List<string>()
        },
        new BuildingRooms {
            buildingLabelName = "40",
            roomsInsideBuilding = new List<string>(),
            roomsOutsideBuilding = new List<string>()
        },
        new BuildingRooms {
            buildingLabelName = "41",
            roomsInsideBuilding = new List<string>{"x001", "x002", "x003", "x004", "x005", "x006", "x007", "x008", "x009", "x010"},
            roomsOutsideBuilding = new List<string>()
        }
    };
    
    [Header("Current Room")]
    [SerializeField] private string currentRoom = "101"; // Hardcoded for now, can be changed in inspector
    
    private Dictionary<string, BuildingRooms> buildingLookup = new Dictionary<string, BuildingRooms>();
    
    void Awake()
    {
        buildingLookup.Clear();
        foreach (var config in buildingConfigs)
        {
            if (!string.IsNullOrEmpty(config.buildingLabelName))
                buildingLookup[config.buildingLabelName] = config;
        }
    }
    
    void OnEnable()
    {
        if (trackedImageManager != null)
        {
            trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
        }
    }
    
    void OnDisable()
    {
        if (trackedImageManager != null)
        {
            trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
        }
    }
    
    void Start()
    {
        // Hide UI initially
        if (validationPanel != null)
        {
            validationPanel.SetActive(false);
        }
        if (roomNumberText != null)
            roomNumberText.text = "";
        if (statusText != null)
            statusText.text = "";
        if (startNavigationButton != null)
            startNavigationButton.onClick.AddListener(OnStartNavigationClicked);
    }
    
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            HandleTrackedImage(trackedImage);
        }
        
        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            HandleTrackedImage(trackedImage);
        }
        
        // Check overall tracking status
        bool anyTracked = false;
        foreach (var trackedImage in trackedImageManager.trackables)
        {
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                anyTracked = true;
                break;
            }
        }
        if (!anyTracked)
        {
            HideValidationPanel();
        }
    }
    
    private void HandleTrackedImage(ARTrackedImage trackedImage)
    {
        if (trackedImage.referenceImage == null || string.IsNullOrEmpty(trackedImage.referenceImage.name))
        {
            return;
        }

        string label = trackedImage.referenceImage.name;

        // Normalize: treat "43", "43-1" to "43-6" as "43"
        if (label == "43" || (label.StartsWith("43-") && int.TryParse(label.Substring(3), out int n) && n >= 1 && n <= 6))
        {
            label = "43";
        }

        if (trackedImage.trackingState == TrackingState.Tracking)
        {
            ShowValidationPanel();
            ValidateCurrentRoom(label);
        }
        else // Covers Limited, None, etc.
        {
            HideValidationPanel();
        }
    }
    
    private void ValidateCurrentRoom(string buildingLabel)
    {
        if (!buildingLookup.ContainsKey(buildingLabel))
        {
            return;
        }

        bool isRoomInsideBuilding = IsRoomInsideBuilding(buildingLabel, currentRoom);
        // Update Room Number
        if (roomNumberText != null)
        {
            roomNumberText.text = $"<b>Room {currentRoom}</b>";
        }
        // Update Status with color and button visibility
        if (statusText != null)
        {
            if (isRoomInsideBuilding)
            {
                statusText.text = "<color=#27AE60><b>Room is in this building.</b></color>";
                if (startNavigationButton != null)
                {
                    startNavigationButton.gameObject.SetActive(true);
                    var btnText = startNavigationButton.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                    if (btnText != null)
                        btnText.text = "<b>Navigate to Room</b>";
                }
            }
            else
            {
                statusText.text = "<color=#E74C3C><b>Room not in this building.</b></color>";
                if (startNavigationButton != null)
                    startNavigationButton.gameObject.SetActive(false);
            }
        }
    }
    
    private bool IsRoomInsideBuilding(string buildingLabel, string roomNumber)
    {
        if (buildingLookup.TryGetValue(buildingLabel, out var config))
        {
            if (config.roomsInsideBuilding.Contains(roomNumber, StringComparer.OrdinalIgnoreCase)) return true;
            if (config.roomsOutsideBuilding.Contains(roomNumber, StringComparer.OrdinalIgnoreCase)) return false;
        }
        // Default: outside
        return false;
    }
    
    private void ShowValidationPanel()
    {
        if (validationPanel != null)
        {
            validationPanel.SetActive(true);
        }
    }
    
    private void HideValidationPanel()
    {
        if (validationPanel != null)
        {
            validationPanel.SetActive(false);
        }
        if (roomNumberText != null)
            roomNumberText.text = "";
        if (statusText != null)
            statusText.text = "";
    }
    
    // Public method to change the current room (for testing or future use)
    public void SetCurrentRoom(string roomNumber)
    {
        currentRoom = roomNumber.ToUpperInvariant();
        if (validationPanel != null && validationPanel.activeInHierarchy)
        {
            ValidateCurrentRoom(buildingConfigs[0].buildingLabelName); // Assuming the first building label
        }
    }
    
    public void OnStartNavigationClicked()
    {
        // TODO: Add your navigation logic here
    }
} 