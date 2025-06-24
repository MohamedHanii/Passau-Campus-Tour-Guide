using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomTestController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_InputField roomInputField;
    [SerializeField] private Button testButton;
    [SerializeField] private Button addInsideButton;
    [SerializeField] private Button addOutsideButton;
    
    [Header("Building Validator")]
    [SerializeField] private BuildingRoomValidator buildingValidator;
    
    void Start()
    {
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
        
        // Set initial room number
        if (roomInputField != null)
        {
            roomInputField.text = "101";
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
            buildingValidator.AddRoomToBuilding(roomNumber);
            Debug.Log($"Added room {roomNumber} to building");
        }
    }
    
    public void AddRoomOutsideBuilding()
    {
        if (buildingValidator != null && roomInputField != null)
        {
            string roomNumber = roomInputField.text;
            buildingValidator.AddRoomOutsideBuilding(roomNumber);
            Debug.Log($"Added room {roomNumber} outside building");
        }
    }
    
    // Pre-configure some test rooms
    public void SetupTestRooms()
    {
        if (buildingValidator != null)
        {
            // Add some rooms inside the building
            buildingValidator.AddRoomToBuilding("101");
            buildingValidator.AddRoomToBuilding("102");
            buildingValidator.AddRoomToBuilding("103");
            buildingValidator.AddRoomToBuilding("201");
            buildingValidator.AddRoomToBuilding("202");
            
            // Add some rooms outside the building
            buildingValidator.AddRoomOutsideBuilding("301");
            buildingValidator.AddRoomOutsideBuilding("302");
            buildingValidator.AddRoomOutsideBuilding("401");
            buildingValidator.AddRoomOutsideBuilding("402");
            
            Debug.Log("Test rooms configured!");
        }
    }
} 