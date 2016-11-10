using UnityEngine;
using System.Collections;

/*
 * https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93controller
 * 
 * The purpose of this script is to be best friends with DungeonManager.
 * It will read in data to interpret and send updates to the UI View.
 * 
 */

public class UIModelScript : MonoBehaviour
{
    public static UIModelScript instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            DestroyObject(this.gameObject);
        }
    }

    // Use this for initialization
    void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        //  TODO aherrera : Either we can have a really fucked up Update check with tons of stuff to account for,
        //                  or setup events that the DungeonManager or UIControllerScript
        //                  will need to take care of...
        //                  I guess like an EventManager like Will said :I

        #region Add AddAdventurerButton

        UIViewScript.instance.enable_AddAdventurerButton(DungeonManager.instance.totalRooms > 2);

        #endregion

        #region RemoveAllAdventurersButton

        UIViewScript.instance.enable_RemoveAllAdventurersButton(DungeonManager.instance.AdventurerPacketCount > 0);

        #endregion

    }





}
