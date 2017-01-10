using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Utilities;

/*
 *  Purpose of this class it to handle logic and push along the flow of the game.
 *  It'll recieve inputs & actions from the user and perform the appropriate commands and methods.
 */

//  TODO aherrera : instead of passing parameter of AdventuerPacket in 'SetPartyToCurrentRoom', etc., should use an index? prevent loss of data b/t parameters

public class ControllerScript : MonoBehaviour
{
    //  TODO aherrera, wspier : eventually replace with List of Dungeons for multiple Dungeons?
    DungeonModel m_playerDungeon;

    public GameObject RoomPrefab;
    public GameObject DungeonPrefab;

    public GameObject MainCanvas;

    // Use this for initialization
    void Start()
    {
        if (m_playerDungeon == null)
        {
            InitializeDungeon();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //  TODO aherrera : clean up... dead packets?
    }

    void InitializeDungeon()
    {
        GameObject playerDungeon = GameObject.Instantiate (DungeonPrefab);
        m_playerDungeon= playerDungeon.AddComponent<DungeonModel>();
        m_playerDungeon.SetView (playerDungeon.GetComponent<DungeonView> ());
        m_playerDungeon.GetComponent<RectTransform> ().SetParent (MainCanvas.GetComponent<RectTransform> ());
        m_playerDungeon.GetComponent<RectTransform> ().localPosition = Vector2.zero;
        m_playerDungeon.name = "DUNGEON_1";

        GameObject newRoomPrefab = GameObject.Instantiate (RoomPrefab);
        RoomModel newBossRoom = RoomGenerator.instance.GenerateUniqueBoss();

        newRoomPrefab.name = newBossRoom.room_name;
        newRoomPrefab.AddComponent<RoomModel> (newBossRoom);
        m_playerDungeon.AddRoomToDungeon(newRoomPrefab);
    }

    public void AddRandomRoom()
    {
        GameObject newRoomPrefab = GameObject.Instantiate (RoomPrefab);
        RoomModel newRoom = RoomGenerator.instance.GenerateUnique();
        newRoomPrefab.name = newRoom.room_name;
        newRoomPrefab.AddComponent<RoomModel>(newRoom);
        m_playerDungeon.AddRoomToDungeon(newRoomPrefab);
    }

    /// <summary>
    /// Start a "Party" at the entrance of a Dungeon.
    /// </summary>
    /// <param name="new_packet">Data of the party</param>
    public void AddAndStartPacket(ref AdventurerPacket new_packet)
    {
        DebugLogger.DebugSystemMessage("Inserting " + new_packet.adventureTitle + " Party to Dungeon");
     
        new_packet.Event_TimerComplete += HandlePacketChallenge;
        m_playerDungeon.AddPartyToDungeon(new_packet);
        new_packet.current_room_index = m_playerDungeon.GetEntrance();
        SetPartyToCurrentRoom(ref new_packet);

        new_packet.StartPacket();
    }

    //  DEBUGGING
    public void AddRandomPacket()
    {
        //  TODO AHERRERA : WOW SHIT REPLACE THIS
         AdventurerModel new_model = AdventurerGenerator.instance.GenerateRandom(1, Enums.UnitRarity.e_rarity_COMMON);
        GameObject go_adventurerpacket = new GameObject();
        AdventurerPacket newpackofcigs = go_adventurerpacket.AddComponent<AdventurerPacket>();
        go_adventurerpacket.name = "AD_PACK: " + newpackofcigs.adventureTitle;
        newpackofcigs.initializePacket("cig crew");
        newpackofcigs.adventurers.Add(new_model);

        AddAndStartPacket(ref newpackofcigs);
    }

    public void RemovePacketFromDungeon(ref AdventurerPacket packet_to_remove)
    {
        packet_to_remove.Event_TimerComplete -= HandlePacketChallenge;
        m_playerDungeon.RemovePartyFromDungeon(packet_to_remove);
    }

    /// <summary>
    /// Initializes Room data to Packet Data
    /// </summary>
    /// <param name="packet"></param>
    protected void SetPartyToCurrentRoom(ref AdventurerPacket packet)
    {
        DebugLogger.DebugSystemMessage("SETTING PARTY TO ROOM");

        RoomModel partys_current_room = m_playerDungeon.GetRoom(packet.current_room_index);
        if (partys_current_room)
        {
            //  TODO aherrera, wspier : Architecture question - should the actions performed here be in a method within AdventurerPacket? AdventurerPacket.ReadyPartyForNextRoom?
            packet.SetTimer(partys_current_room.timer_frequency);
        }
        else
        {
            Debug.LogError("ControllerScript::SetPartyToCurrentRoom -- index for room is invalid: " + packet.current_room_index);
        }
    }

    protected void UpdatePartyToNextRoom(ref AdventurerPacket packet)
    {
        DebugLogger.DebugSystemMessage("UPDATING PARTY TO NEXT ROOM");

        //  We move in the list from last->first
        int next_room_index = packet.current_room_index--;
        packet.current_room_index = next_room_index;

        if (next_room_index < 0)
        {
            //  TODO aherrera : Party has defeated the boss? 

            packet.SetPacketSuccess();
        }
        else
        {
            if (next_room_index == 0) DebugLogger.DebugSystemMessage("Packet " + packet.adventureTitle + " has entered the BOSS ROOM");

            SetPartyToCurrentRoom(ref packet);
        }
    }

    /// <summary>
    /// Listener to packet's "Event_TimerComplete"
    /// </summary>
    /// <param name="packetReference"></param>
    protected void HandlePacketChallenge(ref AdventurerPacket packetReference)
    {
        //  TODO aherrera : Challenge room, update accordingly

        RoomModel party_current_room = m_playerDungeon.GetRoom(packetReference.current_room_index);
        if(party_current_room)
        {
            bool challenge_success = party_current_room.challengeParty(packetReference.adventurers);
            if(challenge_success)
            {
                //  STATE SHOULD BE: Party beat room challenge
                
            }
            else
            {
                //  STATE SHOULD BE: Party lost room challenge

                //  TODO aherrera : perform consequences here -- the Room should provide FAIL CONDITIONS (?)
                for(int i = 0; i < packetReference.adventurers.Count; ++i)
                {
                    // aherrera, RISK OF ERROR : last time I used the 'ref', the actual data was lost. 
                    AdventurerModel finn = packetReference.adventurers[i];
                    party_current_room.onFailRoom(ref finn);
                }

            }

            if(!packetReference.PartyDead)
            {
                //  Actually progress Packet
                UpdatePartyToNextRoom(ref packetReference);
            }
            else
            {
                packetReference.SetPacketFailure();
            }


        }
        else
        {
            Debug.LogError("ControllerScript::HandlePacketChallenge - current room from index [" + packetReference.current_room_index + "] is Invalid!");
        }

    }

}
