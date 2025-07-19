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
        // Removed addInsideButton and addOutsideButton listeners
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
    
    // Removed AddRoomToBuilding and AddRoomOutsideBuilding methods
    
    // Pre-configure some test rooms
    public void SetupTestRooms()
    {
        if (buildingValidator != null)
        {
            // No longer adding test rooms dynamically
            Debug.Log("Test rooms configured!");
        }
    }
} 