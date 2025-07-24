using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingRoomConfig", menuName = "CampusTour/BuildingRoomConfig")]
public class BuildingRoomConfig : ScriptableObject
{
    [System.Serializable]
    public class BuildingRooms
    {
        public string buildingLabelName;
        public List<string> roomsInsideBuilding = new List<string>();
    }
    public List<BuildingRooms> buildingConfigs = new List<BuildingRooms> {
        new BuildingRooms {
            buildingLabelName = "43",
            roomsInsideBuilding = new List<string> { "S011", "S001", "S010" }
        }
    };
} 