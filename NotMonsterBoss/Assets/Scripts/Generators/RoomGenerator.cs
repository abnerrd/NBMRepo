using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Utilities;

/// <summary>
/// This class uses the database to generate room
/// GameObjects based on specified criteria
/// </summary>
public class RoomGenerator : MonoBehaviour
{
    public static RoomGenerator instance = null;

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
    }

    void Start ()
    {
        dataBase = GameObject.Find ("Database").GetComponent<Database> ();
    }

    public RoomScript Generate (string name = "Default", string description = "Default Description", 
                                string success = "You succeeded", string failure = "You failed", 
                                Enums.UnitRarity rarity = Enums.UnitRarity.e_rarity_COMMON,
                                int dex = 1, int str = 1, int wis = 1, int atk = 0, int timer = 10, int passReq = 1)
    {
        GameObject newRoom = new GameObject ();
        RoomScript roomScript = newRoom.AddComponent<RoomScript> ();

        roomScript.room_name = name;
        roomScript.description = description;
        roomScript.rarity = rarity;
        roomScript.success = success;
        roomScript.failure = failure;
        roomScript.dexterity_challenge = dex;
        roomScript.strength_challenge = str;
        roomScript.wisdom_challenge = wis;
        roomScript.room_attack = atk;
        roomScript._isBossRoom = false;
        roomScript.timer_frequency = timer;
        roomScript.pass_req = passReq;

        return roomScript;
    }

    public BossRoomScript GenerateBoss (string name = "Default", string description = "Default Description",
                                    string success = "You succeeded", string failure = "You failed",
                                    Enums.UnitRarity rarity = Enums.UnitRarity.e_rarity_COMMON,
                                    int dex = 1, int str = 1, int wis = 1, int atk = 0, int timer = 10, int passReq = 1)
    {
        GameObject newRoom = new GameObject ();
        BossRoomScript roomScript = newRoom.AddComponent<BossRoomScript> ();
        BossScript boss = newRoom.AddComponent<BossScript> ();

        roomScript._isBossRoom = true;
        roomScript.boss = boss;

        roomScript.room_name = name;
        roomScript.description = description;
        roomScript.rarity = rarity;
        roomScript.success = success;
        roomScript.failure = failure;
        roomScript.dexterity_challenge = dex;
        roomScript.strength_challenge = str;
        roomScript.wisdom_challenge = wis;
        roomScript.room_attack = atk;
        roomScript.timer_frequency = timer;
        roomScript.pass_req = passReq;

        return roomScript;
    }

    public RoomScript GenerateUnique ()
    {
        GameObject newRoom = new GameObject ();
        RoomScript roomScript = newRoom.AddComponent<RoomScript> ();
        RoomData roomData = dataBase.GetRandomRoom ();

        roomScript.room_name = roomData.room_name;
        roomScript.description = roomData.description;
        roomScript.rarity = EnumUtility.StringToRarity(roomData.rarity);
        roomScript.success = roomData.success;
        roomScript.failure = roomData.failure;
        roomScript.dexterity_challenge = roomData.cr_dex;
        roomScript.strength_challenge = roomData.cr_str;
        roomScript.wisdom_challenge = roomData.cr_wis;
        roomScript.room_attack = roomData.attackDamage;
        roomScript._isBossRoom = roomData.is_boss;
        roomScript.timer_frequency = roomData.timer;
        roomScript.pass_req = roomData.pass_req;

        newRoom.name = roomScript.room_name + " - " + roomScript.description;

        return roomScript;
    }

    public BossRoomScript GenerateUniqueBoss ()
    {
        GameObject newRoom = new GameObject ();
        BossRoomScript roomScript = newRoom.AddComponent<BossRoomScript> ();
        RoomData roomData = dataBase.GetRandomBossRoom ();

        roomScript.room_name = roomData.room_name;
        roomScript.description = roomData.description;
        roomScript.rarity = EnumUtility.StringToRarity (roomData.rarity);
        roomScript.success = roomData.success;
        roomScript.failure = roomData.failure;
        roomScript.dexterity_challenge = roomData.cr_dex;
        roomScript.strength_challenge = roomData.cr_str;
        roomScript.wisdom_challenge = roomData.cr_wis;
        roomScript.room_attack = roomData.attackDamage;
        roomScript._isBossRoom = roomData.is_boss;
        roomScript.timer_frequency = roomData.timer;
        roomScript.pass_req = roomData.pass_req;

        newRoom.name = roomScript.room_name + " - " + roomScript.description;

        return roomScript;
    }
}