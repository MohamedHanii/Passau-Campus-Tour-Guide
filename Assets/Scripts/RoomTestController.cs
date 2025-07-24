using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomTestController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_InputField roomInputField;
    [SerializeField] private Button testButton;
    
    [Header("Building Validator")]
    [SerializeField] private BuildingRoomValidator buildingValidator;

    void Start()
    {
        // Set up button listeners
        if (testButton != null)
        {
            testButton.onClick.AddListener(TestCurrentRoom);
        }
        // Set initial room number
        if (roomInputField != null)
        {
            roomInputField.text = "101";
        }
    }

    /// <summary>
    /// Tests the current room number entered in the input field using the BuildingRoomValidator.
    /// </summary>
    public void TestCurrentRoom()
    {
        if (buildingValidator != null && roomInputField != null)
        {
            string roomNumber = roomInputField.text;
            buildingValidator.SetCurrentRoom(roomNumber);
            Debug.Log($"Testing room: {roomNumber}");
        }
    }
} 