using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Manages and updates the Dungeon and it's inhabitants
 * 
 * Has a list of current Adventurers
 * Has a list of current Minions
 * Has a list of Rooms
 */


public class DungeonManager : MonoBehaviour
{
    public static DungeonManager instance = null;

    public List<RoomScript> m_roomsList;
    public int totalRooms { get { return m_roomsList.Count; } }

    //  TODO aherrera : should this be private???
    public List<AdventurerPacket> m_adventurersList;
    public int AdventurerPacketCount { get { return m_adventurersList.Count; } }

    GameObject _roomsParent;

    Database _dataBase;
    public RoomGenerator _roomGen;

    // TODO aherrera: minions list


    // TODO aherrera: score??

    void Awake ()
    {
        if (instance == null)
            instance = this;
        else {
            DestroyObject (this.gameObject);
        }

        m_roomsList = new List<RoomScript> ();
        m_adventurersList = new List<AdventurerPacket> ();
        _roomsParent = new GameObject ();
        _roomsParent.name = "Parent -- Rooms";
        _dataBase = GameObject.Find ("Database").GetComponent<Database> ();
    }

    // Use this for initialization
    void Start ()
    {
        initializeDungeon ();
    }

    void initializeDungeon ()
    {
        BossRoomScript newRoom = _roomGen.GenerateUniqueBoss ().GetComponent<BossRoomScript>();
        addRoom (newRoom);
    }



    // Update is called once per frame
    void Update ()
    {
        updateManager ();
    }

    void FixedUpdate()
    {
        //  TODO aherrera : Should this manager be responsible for Updating them, or should they Update on their own..?
        UpdatePackets();
    }

    void updateManager ()
    {
        //  TODO aherrera : is it bad to automatically remove a packet from the list as soon as it's found dead? ref packet in end of coroutine attemptRoom..?
        // Cleaning up dead packets
        for (int i = 0; i < m_adventurersList.Count; i++) {
            if (m_adventurersList [i].failedExpedition) {
                Debug.Log ("DungeonManager::updateManager -- Cleaning up dead packet: " + m_adventurersList [i].adventureTitle);
                m_adventurersList.RemoveAt (i);
                i--;
                continue;
            }
        }
    }

    //  TODO aherrera : this will come more into play when we figure out how to save & serialize data
    /// <summary>
    /// This function resolves, resumes, and basically "catches up" all the AdventurerPackets when this
    /// game is reopened.
    /// Depends heavily on timestamps
    /// </summary>
    void OnResumeDungeon()
    {
        //  TODO aherrera : check if this is indeed a dungeon that isn't "new" (new game?) condition
    }

    /// <summary>
    /// Removes ALL adventurers from the Dungeon.
    /// </summary>
    public void removeAllAdventurers ()
    {
        //  TODO aherrera : SO packet.triggerAsFailed() doesn't actually change the boolean value? Because I think in the coroutine, we're using a reference..?

        Debug.Log ("DungeonManager::removeAllAdventurers");
        foreach (AdventurerPacket packet in m_adventurersList) {
            Debug.Log ("DungeonManager -- removing " + packet.adventureTitle);
            packet.triggerAsFailed ();
        }
    }

    /// <summary>
    /// Update an adventurer to the next room in this expedition.
    /// </summary>
    /// <param name="expedition"></param>           
    void updateExpeditionToNextRoom (AdventurerPacket expedition, bool toEntrance = false)//, int nextRoomIndex = -1);
    {
        int index = (expedition.currentRoom != null ? m_roomsList.IndexOf (expedition.currentRoom) : 99);
        index--;

        if (index < 0 && !expedition.PartyDead)
        {
            // TODO aherrera: Adventurer wins! Churn out data and do appropriate stuff
            Debug.Log ("Adventurer wins...?");
        }
        else
        {
            expedition.currentRoom = (toEntrance ? getDungeonEntrance () : m_roomsList [index]);
            //StartCoroutine (attemptRoom (expedition));    //  11/12 - previous "Start packet"
            expedition.StartPacket();
            Debug.Log (expedition.adventurers [0]._unitName + " has now entered " + expedition.currentRoom.room_name);
        }
    }

    public void addRoom (RoomScript newRoom)
    {
        m_roomsList.Add (newRoom);
        newRoom.gameObject.transform.SetParent (_roomsParent.transform);
        newRoom.gameObject.name = "Room " + m_roomsList.Count;
    }

    /// <summary>
    /// Set in Rooms list as blank room
    /// </summary>
    /// <param name="destroyedRoom"></param>
    public void destroyRoom (RoomScript destroyedRoom)
    {
        if (m_roomsList.Contains (destroyedRoom)) {
            destroyedRoom.blankRoom ();
        }
    }

    public void enterDungeon (AdventurerScript newAdventurer)
    {
        List<AdventurerScript> solo_party = new List<AdventurerScript> ();
        solo_party.Add (newAdventurer);
        enterDungeon (solo_party, newAdventurer.name + "'s Quest for Glory");
    }

    public void enterDungeon (List<AdventurerScript> adventureParty, string expeditionTitle = "SuperQuest")
    {
        Debug.Log ("And so begins " + expeditionTitle);

        //  TODO aherrera : remove the AdventurerPacket as a GameObject -- only set like that for Editting/Debugging purposes
        GameObject dumb_pack = new GameObject();
        AdventurerPacket newExpedition = dumb_pack.AddComponent<AdventurerPacket>();
        newExpedition.initializePacket (expeditionTitle);
        foreach (AdventurerScript ad in adventureParty) {
            newExpedition.addAdventurerToParty (ad);
        }

        m_adventurersList.Add (newExpedition);

        updateExpeditionToNextRoom (newExpedition, true);
    }

    // Returns the RoomScript at the end of the list -- m_roomsList[0] == BOSSROOM
    public RoomScript getDungeonEntrance ()
    {
        return m_roomsList [m_roomsList.Count - 1];
    }

    #region Previous Coroutine version for "UpdatePacket"
    //IEnumerator attemptRoom (AdventurerPacket packet)
    //{
    //    yield return new WaitForSeconds (packet.timer);

    //    //  We have this check here in case for SOME reason, outside of the coroutine, the party was obliterated.
    //    //  TODO aherrera : We should avoid deleting these objects before this coroutine is finished.
    //    if (!packet.failedExpedition) {
    //        endRoomAttempt (ref packet);
    //    } else {
    //        Debug.Log ("COROUTINE attemptRoom -- time expired, but packet has already failed.");
    //    }
    //}
    #endregion

    void UpdatePackets()
    {

        foreach(AdventurerPacket packet in m_adventurersList)
        {
            packet.UpdatePacket();

            if(packet.TimerComplete)
            {
                if(!packet.PacketCompleteAcknowledged)
                {
                    //  We have this check here in case for SOME reason, outside of the coroutine, the party was obliterated.
                    //  TODO aherrera : We should avoid deleting these objects before this coroutine is finished.
                    if (!packet.failedExpedition)
                    {
                        endRoomAttempt(packet);
                    }
                    else
                    {
                        Debug.Log("COROUTINE attemptRoom -- time expired, but packet has already failed.");
                    }
                }
                packet.PacketCompleteAcknowledged = true;

                if (packet.PartyDead)
                {
                    packet.triggerAsFailed();

                    Debug.Log("Everyone died, the end.");

                }
                else
                {
                    // TODO aherrera: update to next room and any effects that happen here!!
                    updateExpeditionToNextRoom(packet);
                }

            }
        }
    }


    // update the packet with the results
    private void endRoomAttempt (AdventurerPacket packet)
    {
        if (packet.currentRoom.challengeParty (packet.adventurers)) {
            // CONDITION :: Adventurer beat room challenge

            string partyPreamble = "The " + packet.adventureTitle + " group";
            if (packet.PartyCount == 1) {
                partyPreamble = packet.adventurers [0]._unitName;
            }
            Debug.Log (partyPreamble + " " + packet.currentRoom.success);

            // TODO aherrera: update to next room and any effects that happen here!!
            updateExpeditionToNextRoom (packet);
        } else {
            // CONDITION :: Adventurer failed the challenge

            Debug.Log (packet.adventurers[0]._unitName + " " + packet.currentRoom.failure);

            // TODO aherrera: post-mortem on what happens, and set if packet is still alive
            foreach (AdventurerScript ad in packet.adventurers) {
                AdventurerScript finn = ad;
                packet.currentRoom.onFailRoom (ref finn);
            }

            

        }
    }

}