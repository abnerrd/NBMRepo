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
    #endregion

    Dictionary<string, AdventurerData> adventurerList;

    #region Adventurers
    //Return 
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

	void Awake () {
        adventurerList = new Dictionary<string, AdventurerData>();
		LoadAdventurers();
	}

    #region Loading
    void LoadAdventurers () {
        List<AdventurerData> adventurerInfo = CSVImporter.GenerateList<AdventurerData>(ADVENTURER_INFO_PATH);
        for(int i=0; i<adventurerInfo.Count; i++){
            string creatureID = adventurerInfo[i].name;
            adventurerList[creatureID] = adventurerInfo[i];
        }
	}
    #endregion
}
