using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * The purpose of this class is to provide data for the Dungeons.
 * Contains getters/setters and ways to add onto the data.
 * This includes lists of Rooms, "PartyPackets", Dungeon Boss, etc.
 * 
 * Models should NOT perform any logic.
 */

public class DungeonModel : MonoBehaviour
{
    /// <summary>
    /// List of all the Rooms within the Dungeon. Boss Room should be [0]
    /// </summary>
    //  TODO aherrera : replace with RoomModels
    protected List<RoomModel> m_roomList;

    //  TODO aherrera, wspier : should this be handled by RoomModels only instead?
    /// <summary>
    /// List of all Minions within the Dungeon.
    /// </summary>
    protected List<MinionModel> m_allMinions;

    /// <summary>
    /// List of all groups of adventurers currently progressing through the Dungeon.
    /// </summary>
    protected Dictionary<string, AdventurerPacket> m_questingParties;

    private DungeonView m_view;
    public void SetView (DungeonView view)
    {
        m_view = view;
    }

    public int GetRoomCount ()
    {
        return m_roomList.Count;
    }

    private void Awake()
    {
        //  TODO aherrera : move these into an initialize script
        m_roomList = new List<RoomModel>();
        m_allMinions = new List<MinionModel>();
        m_questingParties = new Dictionary<string, AdventurerPacket>();
    }

    // Use this for initialization
	void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void init (RectTransform MainCanvas)
    {
        RectTransform pd_transform = GetComponent<RectTransform> ();
        pd_transform.SetParent (MainCanvas, false);
        pd_transform.SetAsFirstSibling ();
        pd_transform.rect.size.Set (MainCanvas.rect.size.x, MainCanvas.rect.size.y);
        m_view = GetComponent<DungeonView> ();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// Room Methods
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void AddRoomToDungeon(GameObject new_room)
    {
        m_roomList.Add(new_room.GetComponent<RoomModel>());
        m_view.AddRoom (new_room);
    }

    //  TODO aherrear, wspier : Should this "blank" the room, or explicitly destroy room and shorten Dungeon
    public void DestroyRoom(RoomModel room_to_destroy)
    {
        if (m_roomList.Contains(room_to_destroy))
        {
            room_to_destroy.blankRoom();            
        }
        else
        {
            DebugLogger.DebugSystemMessage("DungeonModel::DestroyRoom -- room to destroy not in List!");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="room_index">Put in terms of array index [0->n]</param>
    /// <returns></returns>
    public RoomModel GetRoom(int room_index)
    {
        if (m_roomList.Count > room_index)
        {
            return m_roomList[room_index];
        }
        else
        {
            Debug.LogWarning("DungeonModel::GetRoom -- given index exceeds list count!");
            return null;
        }
    }
    public RoomModel GetBossRoom()
    {
        if(m_roomList.Count > 0)
        {
            return m_roomList[0];
        }
        else
        {
            Debug.LogWarning("DungeonModel::GetBossRoom -- room list <= 0!");
            return null;
        }
    }
    public int GetEntrance()
    {
        return m_roomList.Count - 1;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// Party Methods
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////


    /// <summary>
    /// Call this method AFTER the packet is added!
    /// </summary>
    public void StartPartyPacket(string packet_key)
    {
        if(m_questingParties.ContainsKey(packet_key))
        {
            m_questingParties[packet_key].StartPacket(packet_key);
        }
        else { Debug.LogError("DungeonModel::StartPartyPacket -- packet not found with key: " + packet_key); }
    }

    public string AddPartyToDungeon(AdventurerPacket new_party)
    {
        //  TODO aherrera : HEY pls randomize me
        //  TODO aherrera : probably make sure i'm unique
        string packet_key = "BEST KEY";
        m_questingParties.Add(packet_key, new_party);
        
        InitializePacketToRoom(packet_key, GetEntrance());

        return packet_key;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="packet_key"></param>
    /// <returns>Returns reference to removed packet</returns>
    public AdventurerPacket RemovePartyFromDungeon(string packet_key)
    {
        AdventurerPacket return_value = null;
        if (m_questingParties.ContainsKey(packet_key))
        {
            return_value = m_questingParties[packet_key];
            m_questingParties.Remove(packet_key);
        }
        else
        {
            DebugLogger.DebugSystemMessage("DungeonModel::RemovePartyFromDungeon -- party to destroy not in List!");
        }
        return return_value;
    }

    public List<string> GetPacketKeysList()
    {
        return new List<string>(m_questingParties.Keys);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// Minion Methods
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void AddMinionToRoom(MinionModel new_minion)
    {
        m_allMinions.Add(new_minion);

        //  TODO aherrera : Add Minion to RoomModel here?
    }

    //  TODO aherrera, wspier : Are Minions going to have a reference to the Room they are currently in?
    /// <summary>
    /// 
    /// </summary>
    /// <param name="minion_to_remove"></param>
    /// <param name="room_to_remove_from">null=removes Minion from the room it is currently in</param>
    public void RemoveMinionFromRoom(MinionModel minion_to_remove, RoomModel room_to_remove_from = null)
    {
        if(m_allMinions.Contains(minion_to_remove))
        {
            m_allMinions.Remove(minion_to_remove);
        }
        else
        {
            Debug.LogWarning("DungeonModel::RemoveMinionFromRoom -- minion is not in List!");
        }

        //  TODO aherrera : Remove Minion from RoomModel
    }



    /////////////////////////////////////
    /// METHODS THAT COULD POSSIBLY NOT BELONG IN A MODEL?
    /////////////////////////////////////



    /// <summary>
    /// Challenge packet to their CurrentRoom.
    /// Returns : TRUE if party is still ALIVE
    /// </summary>
    /// <param name="packet_key"></param>
    /// <returns>Returns TRUE if the party is ALIVE</returns>
    public bool ChallengePacketInCurrentRoom(string packet_key)
    {
        AdventurerPacket party_packet = m_questingParties[packet_key];
        if(party_packet != null)
        {
            RoomModel party_current_room = GetRoom(party_packet.current_room_index);
            if (party_current_room != null)
            {
                bool challenge_successful = party_current_room.challengeParty(party_packet.adventurers);
                if (challenge_successful)
                {
                    party_packet.AwardPacket(party_current_room);
                }
                else
                {
                    party_packet.PunishPacket(party_current_room);
                }
            }
            else { Debug.LogError("DungeonModel::ChallengePacketInCurrentRoom -- room not in list from packet: " + party_packet.adventureTitle); }

            //  TODO aherrera, wspier : like, should this logic be in the Model?? Should it be in charge to figure it out?
            if(party_packet.PartyDead)
            {
                party_packet.SetPacketDungeonFailure();
            }

            return !party_packet.PartyDead;
        }
        else { Debug.LogError("DungeonModel::ChallengePacketInCurrentRoom -- packet not found in list with string: " + packet_key); }
        
    //  Hey! Pls don't come here okay thanks                     
        return false;
    }

    /// <summary>
    /// Advances Packet to the next room.
    /// Updates packet with new room data.
    /// Returns : TRUE if packet has completed dungeon
    /// </summary>
    /// <param name="packet"></param>
    public bool UpdatePacketToNextRoom(string packet_key)
    {
        AdventurerPacket party_packet = m_questingParties[packet_key];
        if (party_packet != null)
        {
            //  We move in the list from last->first
            bool completed_dungeon = (0 > party_packet.AdvanceIndex());

            if(completed_dungeon)
            {
                //  TODO aherrera : Party has defeated the boss? 
                party_packet.SetPacketDungeonCompleted();
            }
            else
            {
                RoomModel party_new_current_room = GetRoom(party_packet.current_room_index);
                if (party_new_current_room != null)
                {
                    InitializePacketToRoom(packet_key);
                }
            }

            return completed_dungeon;
        }else { Debug.LogError("DungeonModel::UpdatePacketToNextRoom -- packet not found in list with string: " + packet_key); }

        //  Hey pls don't be here
        //  TODO aherrera : handle these kinds of errors to search out and erase traces of this packet?
        return false;
    }

    /// <summary>
    /// Initialize Packet data to it's Current Room.
    /// Optional param sets to given room index
    /// </summary>
    /// <param name="packet_key"></param>
    /// <param name="new_room_index">optional; sets the packet to this room index</param>
    public void InitializePacketToRoom(string packet_key, int new_room_index = -1)
    {
        AdventurerPacket packet_reference = m_questingParties[packet_key];

        //  TODO aherrera, wspier : should 0 be allowed? That would mean fighting Boss Room right away
        if(new_room_index >= 0)
        {
            packet_reference.current_room_index = new_room_index;
        }

        //  TODO aherrera : replace this check with something more elegant :/
        if(new_room_index >= m_roomList.Count)
        {
            Debug.LogError("GIVEN INDEX LARGER THAN ARRAY SIZE");
        }
        
        RoomModel current_room_reference = m_roomList[packet_reference.current_room_index];

        if (current_room_reference != null)
        {
            packet_reference.SetTimer(current_room_reference.timer_frequency);
        }
        else { Debug.LogError("DungeonModel::InitializePacketToCurrentRoom -- currentroom not found for string: " + packet_key); }

    }
}
