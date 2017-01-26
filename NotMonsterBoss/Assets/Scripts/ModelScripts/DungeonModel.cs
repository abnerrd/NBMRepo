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
    protected Dictionary<string, GameObject> m_questingParties;

    //  TODO aherrera, wspier : DungeonModel should not have a reference to DungeonView. It should be handled by the DungeonController to adjust both.
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
        m_questingParties = new Dictionary<string, GameObject>();
    }

    // Use this for initialization
	void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
		//  TODO aherrera : don't put this here
        foreach(GameObject go in m_questingParties.Values)
        {
            PartyController pc = go.GetComponent<PartyController>();
            if(pc != null)
            {
                pc.InternalUpdate();
            }
        }
	}

    public void init (RectTransform MainCanvas)
    {
        RectTransform pd_transform = GetComponent<RectTransform> ();
        pd_transform.SetParent (MainCanvas, false);
        pd_transform.SetAsFirstSibling ();
        pd_transform.rect.size.Set (MainCanvas.rect.size.x, MainCanvas.rect.size.y);
       // pd_transform.
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

    public string AddPartyToDungeon(GameObject new_party)
    {
        PartyController pc = new_party.GetComponent<PartyController>();
        //  TODO aherrera : probably make sure i'm unique
        string packet_key = pc.GetAdventureTitle();
        m_questingParties.Add(packet_key, new_party);
        
        InitializePacketToRoom(packet_key, GetEntrance());

        return packet_key;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="packet_key"></param>
    /// <returns>Returns reference to removed packet</returns>
    public GameObject RemovePartyFromDungeon(string packet_key)
    {
        GameObject return_value = null;
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



    //TODO Should be in DungeonController.cs
    /// <summary>
    /// Challenge packet to their CurrentRoom.
    /// Returns : TRUE if party is still ALIVE
    /// </summary>
    /// <param name="packet_key"></param>
    /// <returns>Returns TRUE if the party is ALIVE</returns>
    public bool ChallengePacketInCurrentRoom(string packet_key)
    {
        GameObject party_object = m_questingParties[packet_key];
        PartyController party = party_object.GetComponent<PartyController> ();
        if(party != null)
        {
            RoomModel party_current_room = GetRoom(party.GetCurrentRoomIndex());
            if (party_current_room != null)
            {
                bool challenge_successful = party_current_room.challengeParty(party.GetAdventurers());
                if (challenge_successful)
                {
                    //party_packet.AwardPacket(party_current_room);
                }
                else
                {
                    ResolveChallengeFailure (party, party_current_room);

                }
            }
            else { Debug.LogError ("DungeonModel::ChallengePacketInCurrentRoom -- room not in list from packet: " + party.GetAdventureTitle ()); }

            //  TODO aherrera, wspier : like, should this logic be in the Model?? Should it be in charge to figure it out?
            if(party.isPartyDead())
            {
                party.SetState(PartyState.PARTY_FAILED);
            }

            return !party.isPartyDead();
        }
        else { Debug.LogError("DungeonModel::ChallengePacketInCurrentRoom -- packet not found in list with string: " + packet_key); }
        
    //  Hey! Pls don't come here okay thanks                     
        return false;
    }

    public void ResolveChallengeFailure (PartyController party, RoomModel room)
    {
        foreach (GameObject adventurer in party.GetAdventurers ())
        {
            AdventurerModel a = adventurer.GetComponent<AdventurerModel> ();
            a.applyDamage (room.room_attack);
        }
    }

    /// <summary>
    /// Advances Packet to the next room.
    /// Updates packet with new room data.
    /// Returns : TRUE if packet has completed dungeon
    /// </summary>
    /// <param name="packet"></param>
    public bool UpdatePacketToNextRoom(string packet_key)
    {
        PartyController party = m_questingParties [packet_key].GetComponent<PartyController> ();
        if (party != null)
        {
            //  We move in the list from last->first
            bool completed_dungeon = (0 > party.AdvanceRoom());

            if(completed_dungeon)
            {
                //  TODO aherrera : Party has defeated the boss? 
                party.SetState (PartyState.PARTY_SUCCESS);
            }
            else
            {
                RoomModel party_new_current_room = GetRoom(party.GetCurrentRoomIndex());
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
        PartyController party = m_questingParties[packet_key].GetComponent<PartyController>();

        //  TODO aherrera, wspier : should 0 be allowed? That would mean fighting Boss Room right away
        if(new_room_index >= 0)
        {
            party.SetCurrentRoomIndex( new_room_index) ;
        }

        //  TODO aherrera : replace this check with something more elegant :/
        if(new_room_index >= m_roomList.Count)
        {
            Debug.LogError("GIVEN INDEX LARGER THAN ARRAY SIZE");
        }
        
        RoomModel current_room_reference = m_roomList[party.GetCurrentRoomIndex()];

        if (current_room_reference != null)
        {
            party.SetRoomTimer(current_room_reference.timer_frequency);
        }
        else { Debug.LogError("DungeonModel::InitializePacketToCurrentRoom -- currentroom not found for string: " + packet_key); }

    }
}
