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
    public GameObject BossRoomPrefab;

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
        GameObject playerDungeon = Instantiate (DungeonPrefab);
        RectTransform mc = MainCanvas.GetComponent<RectTransform> ();
        m_playerDungeon = playerDungeon.GetComponent<DungeonModel> ();
        m_playerDungeon.init (mc);
        m_playerDungeon.name = "DUNGEON_1";

        GameObject newRoomPrefab = Instantiate (BossRoomPrefab);
        RoomModel newBossRoom = RoomGenerator.instance.GenerateUniqueBoss();

        newRoomPrefab.name = newBossRoom.room_name;
        newRoomPrefab.AddComponent<RoomModel> (newBossRoom);
        m_playerDungeon.AddRoomToDungeon(newRoomPrefab);
    }

    public void AddRandomRoom()
    {
        GameObject newRoomPrefab = Instantiate (RoomPrefab);
        RoomModel newRoom = RoomGenerator.instance.GenerateUnique();
        newRoomPrefab.name = newRoom.room_name;
        newRoomPrefab.AddComponent<RoomModel>(newRoom);
        m_playerDungeon.AddRoomToDungeon(newRoomPrefab);
    }

    /// <summary>
    /// Start a "Party" at the entrance of a Dungeon.
    /// ! ONLY METHOD THAT USES 'ref AdventurerPacket' IN PARAMETER
    /// </summary>
    /// <param name="new_packet">Data of the party</param>
    public void AddAndStartPacket(ref AdventurerPacket new_packet)
    {
        DebugLogger.DebugSystemMessage("Inserting " + new_packet.adventureTitle + " Party to Dungeon");

        new_packet.RegisterToEvent_TimerComplete(HandlePacketChallenge);
        string packet_key = m_playerDungeon.AddPartyToDungeon(new_packet);
        new_packet.StartPacket(packet_key);
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

    public void RemovePacketFromDungeon(string packet_key)
    {
        AdventurerPacket packet_to_remove = m_playerDungeon.RemovePartyFromDungeon(packet_key);

        //  RISK aherrera : is this gonna work? Like, since we're not really returning a reference
        //                      to packet_to_remove, will it unregister from the Packet correctly?
        packet_to_remove.UnregisterToEvent_TimerComplete(HandlePacketChallenge);
    }

    protected void UpdatePartyToNextRoom(string party_key)
    {
        DebugLogger.DebugSystemMessage("UPDATING PARTY TO NEXT ROOM");
        if(m_playerDungeon.UpdatePacketToNextRoom(party_key))
        {
            //  STATE :: Packet has defeated dungeon!
            //  celebrate
        }
    }

    /// <summary>
    /// Listener to packet's "Event_TimerComplete"
    /// </summary>
    /// <param name="packetReference"></param>
    protected void HandlePacketChallenge(string packet_key)
    {
        //  TODO aherrera : Challenge room, update accordingly

        if(m_playerDungeon.ChallengePacketInCurrentRoom(packet_key))
        {
            //  STATE :: Party should still be ALIVE
            //  Actually progress Packet
            UpdatePartyToNextRoom(packet_key);
        }
    }

}
