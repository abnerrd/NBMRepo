using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Utilities;

/// <summary>
/// This class uses the database to generate adventurer
/// GameObjects based on specified criteria
/// </summary>
public class AdventurerGenerator : MonoBehaviour
{
    Database dataBase;

    void Start ()
    {
        dataBase = GameObject.Find ("Database").GetComponent<Database> ();
    }

    public GameObject Generate (string name = "Default", string description = "Default Description", 
                                Enums.UnitRarity rarity = Enums.UnitRarity.e_rarity_COMMON,
                                int totalHealth =10, int dex=1, int str=1, int wis=1, int atk=0)
    {
        GameObject newAdventurer = new GameObject ();
        AdventurerScript adventurerScript = newAdventurer.AddComponent<AdventurerScript> ();

        adventurerScript._unitName = name;
        adventurerScript._unitDescription = description;
        adventurerScript.rarity = rarity;
        adventurerScript.totalHealth = totalHealth;
        adventurerScript.dexterity = dex;
        adventurerScript.strength = str;
        adventurerScript.wisdom = wis;
        adventurerScript.attack_damage = atk;

        return newAdventurer;
    }

    public AdventurerScript GenerateUnique ()
    {
        GameObject newAdventurer = new GameObject ();
        AdventurerScript adventurerScript = newAdventurer.AddComponent<AdventurerScript> ();
        AdventurerData adventurerData = dataBase.GetRandomAdventurer ();

        adventurerScript._unitName = adventurerData.name;
        adventurerScript._unitDescription = adventurerData.description;
        adventurerScript.rarity = EnumUtility.StringToRarity(adventurerData.rarity);
        adventurerScript.totalHealth = adventurerData.max_health;
        adventurerScript.dexterity = adventurerData.dex_mod;
        adventurerScript.strength = adventurerData.str_mod;
        adventurerScript.wisdom = adventurerData.wis_mod;
        adventurerScript.attack_damage = adventurerData.attackDamage;

        newAdventurer.name = adventurerScript._unitName + ", " + adventurerScript._unitDescription;

        return adventurerScript;
    }

    public GameObject GenerateByName (string name)
    {
        return Generate (name, "Default Description", Enums.UnitRarity.e_rarity_COMMON, 10, 1, 1, 0);
    }
}