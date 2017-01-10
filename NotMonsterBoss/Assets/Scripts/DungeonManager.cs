﻿using UnityEngine;
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

    public List<RoomModel> m_roomsList;
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

        m_roomsList = new List<RoomModel> ();
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
            if (m_adventurersList [i].State != AdventurerPacket.PacketState.PARTY_IN_PROGRESS) {
                DebugLogger.DebugSystemMessage ("DungeonManager::updateManager -- Cleaning up " + m_adventurersList[i].State.ToString() + " packet: " + m_adventurersList [i].adventureTitle);
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
        Debug.Log ("DungeonManager::removeAllAdventurers");
        foreach (AdventurerPacket packet in m_adventurersList) {
            DebugLogger.DebugSystemMessage ("DungeonManager -- removing " + packet.adventureTitle);
            packet.SetPacketFailure ();
        }
    }

    /// <summary>
    /// Update an adventurer to the next room in this expedition.
    /// </summary>
    /// <param name="expedition"></param>           
    void updateExpeditionToNextRoom (AdventurerPacket expedition, bool toEntrance = false)//, int nextRoomIndex = -1);
    {
        int index = expedition.current_room_index;//(expedition.currentRoom != null ? m_roomsList.IndexOf (expedition.currentRoom) : 99);
        //index--;

        if (index < 0 && !expedition.PartyDead)
        {
            // TODO aherrera: Adventurer wins! Churn out data and do appropriate stuff
            Debug.Log ("Adventurer wins...?");
            expedition.SetPacketSuccess();
        }
        else
        {
        //    expedition.currentRoom = (toEntrance ? getDungeonEntrance () : m_roomsList [index]);
            //StartCoroutine (attemptRoom (expedition));    //  11/12 - previous "Start packet"
        //    expedition.StartPacket();
       //     DebugLogger.DebugRoomTransition (expedition.currentRoom, expedition.adventurers [0]);
        }
    }

    public void addRoom (RoomModel newRoom)
    {
        m_roomsList.Add (newRoom);
        newRoom.gameObject.transform.SetParent (_roomsParent.transform);
        newRoom.gameObject.name = "Room " + m_roomsList.Count;
    }

    /// <summary>
    /// Set in Rooms list as blank room
    /// </summary>
    /// <param name="destroyedRoom"></param>
    public void destroyRoom (RoomModel destroyedRoom)
    {
        if (m_roomsList.Contains (destroyedRoom)) {
            destroyedRoom.blankRoom ();
        }
    }

    public void enterDungeon (AdventurerModel newAdventurer)
    {
        List<AdventurerModel> solo_party = new List<AdventurerModel> ();
        solo_party.Add (newAdventurer);
        enterDungeon (solo_party, newAdventurer.name + "'s Quest for Glory");
    }

    public void enterDungeon (List<AdventurerModel> adventureParty, string expeditionTitle = "SuperQuest")
    {
        Debug.Log ("And so begins " + expeditionTitle);

        //  TODO aherrera : remove the AdventurerPacket as a GameObject -- only set like that for Editting/Debugging purposes
        GameObject dumb_pack = new GameObject();
        AdventurerPacket newExpedition = dumb_pack.AddComponent<AdventurerPacket>();
        newExpedition.initializePacket (expeditionTitle);
        foreach (AdventurerModel ad in adventureParty) {
            newExpedition.addAdventurerToParty (ad);
        }

        m_adventurersList.Add (newExpedition);

        updateExpeditionToNextRoom (newExpedition, true);
    }

    // Returns the RoomScript at the end of the list -- m_roomsList[0] == BOSSROOM
    public RoomModel getDungeonEntrance ()
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

        //foreach(AdventurerPacket packet in m_adventurersList)
        //{
        //    packet.UpdatePacket();

        //    if(packet.TimerComplete)
        //    {
        //        if(!packet.PacketCompleteAcknowledged)
        //        {
        //            if (packet.State == AdventurerPacket.PacketState.PARTY_IN_PROGRESS)
        //            {
        //                endRoomAttempt(packet);
        //            }
        //            else
        //            {
        //                DebugLogger.DebugSystemMessage("DungeonManager::UpdatePackets -- time expired, but packet state is " + packet.State.ToString());
        //            }
        //        }
        //        packet.PacketCompleteAcknowledged = true;


        //        //  Update packets to next step
        //        if (packet.PartyDead)
        //        {
        //            packet.SetPacketFailure();
        //            DebugLogger.DebugGameObver ();
        //        }
        //        else
        //        {
        //            updateExpeditionToNextRoom(packet);
        //        }

        //    }
        //}
    }


    // update the packet with the results
    private void endRoomAttempt (AdventurerPacket packet)
    {
 //       if (packet.currentRoom.challengeParty (packet.adventurers))
        {
            // CONDITION :: Adventurer beat room challenge

            string partyPreamble = "The " + packet.adventureTitle + " group";
            if (packet.PartyCount == 1)
            {
                partyPreamble = packet.adventurers [0]._unitName;
            }

           //  TODO aherrera : onPacketRoomChallengeSuccess
        }
//        else
        {
            // CONDITION :: Packet failed the challenge

            // TODO aherrera: post-mortem on what happens, and set if packet is still alive
            foreach (AdventurerModel ad in packet.adventurers)
            {
                AdventurerModel finn = ad;
    //            packet.currentRoom.onFailRoom (ref finn);
            }

            // TODO aherrera : onPacketRoomChallengeFail


        }
    }

}