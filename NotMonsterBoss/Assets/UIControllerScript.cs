using UnityEngine;
using System.Collections;

/*
 * https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93controller
 * 
 *  The purpose of the UI Controller is to receive input from the user.
 *  
 *  Buttons will be attached to functions in this Controller.
 */

public class UIControllerScript : MonoBehaviour
{
    public static UIControllerScript instance = null;

    void Awake ()
    {
        if (instance == null)
            instance = this;
        else {
            DestroyObject (this.gameObject);
        }
    }

    // Use this for initialization
    void Start ()
    {

    }

    // Update is called once per frame
    void Update ()
    {
    }

    public void OnAddRoomButton ()
    {
        if (DungeonManager.instance.totalRooms == 0) {
            Debug.LogWarning ("UIControllerScript::OnAddRoomButton -- No rooms yet in Dungeon; generating raNdoM Boss Room");
            DungeonManager.instance.addRoom (RoomGenerator.instance.GenerateUniqueBoss().GetComponent<BossRoomScript>());
        } else {
            DungeonManager.instance.addRoom (RoomGenerator.instance.GenerateUnique ());
        }

    }

    public void OnAddAdventurerButton ()
    {
        DungeonManager.instance.enterDungeon (AdventurerGenerator.instance.GenerateUnique ());
    }

    public void OnRemoveAllAdventurersButton ()
    {
        DungeonManager.instance.removeAllAdventurers ();
    }


}