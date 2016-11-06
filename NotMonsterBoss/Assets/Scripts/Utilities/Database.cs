using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Utilities;


/// <summary>
/// This class loads in all csv files specified
/// It stores each entry in respective dictionaries
/// It loads the csv files when created
/// </summary>

public class Database : MonoBehaviour{

	#region Constants
    //Path to the CSV containing all adventurer info
	const string ADVENTURER_INFO_PATH = "Assets/Resources/Data/AdventurerData.csv";
    //Path to the CSV containing all room info
    const string ROOM_INFO_PATH = "Assets/Resources/Data/RoomData.csv";
    #endregion

    Dictionary<string, AdventurerData> adventurerList;
    Dictionary<string, RoomData> roomList;

    #region Adventurers
    public AdventurerData GetAdventurerByName(string name){
    	if(adventurerList.ContainsKey(name))return adventurerList[name];
        else return null;}
    public AdventurerData GetRandomAdventurer(){
    	List<string> keys = new List<string>(adventurerList.Keys);
		string randomKey = keys[Random.Range(0,adventurerList.Count)];
    	return adventurerList[randomKey];
    }
    public AdventurerData GetRandomAdventurerByRarity(Enums.UnitRarity rarity){
        List<AdventurerData> adventurers = new List<AdventurerData>();
        foreach(KeyValuePair<string,AdventurerData> entry in adventurerList){
            if(EnumUtility.StringToRarity(entry.Value.rarity) == rarity) adventurers.Add(entry.Value);
        }
        if(adventurers.Count>0)return adventurers[Random.Range(0,adventurers.Count)];
        else return null;
    }
    #endregion Adventurers

    #region Room
    public RoomData GetRoomByName (string name)
    {
        if (roomList.ContainsKey (name)) return roomList [name];
        else return null;
    }
    public RoomData GetRandomRoom ()
    {
       List<RoomData> rooms = new List<RoomData> ();
        foreach (KeyValuePair<string, RoomData> entry in roomList) {
            if (!entry.Value.is_boss) rooms.Add (entry.Value);
        }
        if (rooms.Count > 0) return rooms [Random.Range (0, rooms.Count)];
        else return null;
    }
    public RoomData GetRandomRoomByRarity (Enums.UnitRarity rarity)
    {
        List<RoomData> rooms = new List<RoomData> ();
        foreach (KeyValuePair<string, RoomData> entry in roomList) {
            if (EnumUtility.StringToRarity (entry.Value.rarity) == rarity && !entry.Value.is_boss) rooms.Add (entry.Value);
        }
        if (rooms.Count > 0) return rooms [Random.Range (0, rooms.Count)];
        else return null;
    }
    public RoomData GetRandomRoomByTyoe (Enums.RoomType type)
    {
        List<RoomData> rooms = new List<RoomData> ();
        foreach (KeyValuePair<string, RoomData> entry in roomList) {
            if (EnumUtility.StringToRoomType (entry.Value.type) == type && !entry.Value.is_boss) rooms.Add (entry.Value);
        }
        if (rooms.Count > 0) return rooms [Random.Range (0, rooms.Count)];
        else return null;
    }

    // Boss Rooms
    public RoomData GetRandomBossRoom ()
    {
        List<RoomData> rooms = new List<RoomData> ();
        foreach (KeyValuePair<string, RoomData> entry in roomList) {
            if (entry.Value.is_boss) rooms.Add (entry.Value);
        }
        if (rooms.Count > 0) return rooms [Random.Range (0, rooms.Count)];
        else return null;
    }
    public RoomData GetRandomBossRoomByRarity (Enums.UnitRarity rarity)
    {
        List<RoomData> rooms = new List<RoomData> ();
        foreach (KeyValuePair<string, RoomData> entry in roomList) {
            if (EnumUtility.StringToRarity (entry.Value.rarity) == rarity && entry.Value.is_boss) rooms.Add (entry.Value);
        }
        if (rooms.Count > 0) return rooms [Random.Range (0, rooms.Count)];
        else return null;
    }
    #endregion




	void Awake () {
        adventurerList = new Dictionary<string, AdventurerData> ();
        roomList = new Dictionary<string, RoomData> ();
		LoadAdventurers ();
        LoadRooms ();
	}

    #region Loading
    void LoadAdventurers () {
        List<AdventurerData> adventurerInfo = CSVImporter.GenerateList<AdventurerData>(ADVENTURER_INFO_PATH);
        for(int i=0; i<adventurerInfo.Count; i++){
            string creatureID = adventurerInfo[i].name;
            adventurerList[creatureID] = adventurerInfo[i];
        }
	}
    void LoadRooms ()
    {
        List<RoomData> roomInfo = CSVImporter.GenerateList<RoomData> (ROOM_INFO_PATH);
        for (int i = 0; i < roomInfo.Count; i++) {
            string roomID = roomInfo [i].name;
            roomList [roomID] = roomInfo [i];
            Debug.Log ("Loading Room called: " + roomInfo [i].name);
        }
    }
    #endregion
}
