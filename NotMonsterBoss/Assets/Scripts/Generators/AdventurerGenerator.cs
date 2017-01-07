using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This class uses the database to generate adventurer
/// GameObjects based on specified criteria
/// </summary>
public class AdventurerGenerator : MonoBehaviour
{
    public static AdventurerGenerator instance = null;

    Database dataBase;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        dataBase = GameObject.Find ("Database").GetComponent<Database> ();
    }

    public AdventurerModel Generate (string name = "Default", string description = "Default Description", 
                                Enums.UnitRarity rarity = Enums.UnitRarity.e_rarity_COMMON,
                                int totalHealth =10, int dex=1, int str=1, int wis=1, int atk=0, int level=1)
    {
        GameObject newAdventurer = new GameObject ();
        AdventurerModel adventurerScript = newAdventurer.AddComponent<AdventurerModel> ();

        adventurerScript._unitName = name;
        adventurerScript._unitDescription = description;
        adventurerScript.rarity = rarity;
        adventurerScript.totalHealth = totalHealth;
        adventurerScript.dexterity = dex;
        adventurerScript.strength = str;
        adventurerScript.wisdom = wis;
        adventurerScript.attack_damage = atk;
        adventurerScript.level = level;

        return adventurerScript;
    }

    public AdventurerModel GenerateRandom (int level, Enums.UnitRarity rarity)
    {
        GameObject newAdventurer = new GameObject ();
        AdventurerModel adventurerScript = newAdventurer.AddComponent<AdventurerModel> ();

        NameData name = dataBase.GetRandomName ();

        adventurerScript._unitName = name.title;
        adventurerScript._unitDescription = name.subtitle;
        adventurerScript._unitNameDelim = (name.delimiter == "/" ? ", " : " ");

        int statMod = 0,
            healthMod = 0;
        float statMult = 1.0f,
              healthMult = 1.0f;

        statMod += level;
        healthMod += level * 5;

        switch (rarity) 
        {
            case Enums.UnitRarity.e_rarity_RARE:
            statMult = 1.1f;
            healthMult = 1.1f;
            break;
        case Enums.UnitRarity.e_rarity_EPIC:
            statMult = 1.3f;
            healthMult = 1.3f;
            break;
        case Enums.UnitRarity.e_rarity_LEGEND:
            statMult = 1.5f;
            healthMult = 1.5f;
            break;
        }



        adventurerScript.totalHealth = (int)(Random.Range(healthMod, healthMod+healthMod) * healthMult);
        adventurerScript.dexterity = (int)(Random.Range (statMod, statMod + statMod) * statMult);
        adventurerScript.strength = (int)(Random.Range (statMod, statMod + statMod) * statMult);
        adventurerScript.wisdom = (int)(Random.Range (statMod, statMod + statMod) * statMult);
        adventurerScript.attack_damage = (int)(Random.Range (statMod, statMod + statMod) * statMult);

        adventurerScript.rarity = rarity;
        adventurerScript.level = level;
        adventurerScript.currentHealth = adventurerScript.totalHealth;


        newAdventurer.name = adventurerScript._unitName + adventurerScript._unitNameDelim + adventurerScript._unitDescription;

        return adventurerScript;
    }



    public AdventurerModel GenerateUnique ()
    {
        GameObject newAdventurer = new GameObject ();
        AdventurerModel adventurerScript = newAdventurer.AddComponent<AdventurerModel> ();
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

    public AdventurerModel GenerateByName (string name)
    {
        return Generate (name, "Default Description", Enums.UnitRarity.e_rarity_COMMON, 10, 1, 1, 0, 1);
    }
}