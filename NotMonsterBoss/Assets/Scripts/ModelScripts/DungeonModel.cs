﻿using System.Collections;
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
    //  TODO aherrera : replace with GameObjects!
    public List<RoomModel> m_roomList { get; private set;}
    public int RoomCount { get { return m_roomList.Count; } }

    //  TODO aherrera, wspier : should this be handled by RoomModels only instead?
    /// <summary>
    /// List of all Minions within the Dungeon.
    /// </summary>
    protected List<MinionModel> m_allMinions;

    public int GetRoomCount ()
    {
        return m_roomList.Count;
    }

    private void Awake()
    {
        //  TODO aherrera : move these into an initialize script
        m_roomList = new List<RoomModel>();
        m_allMinions = new List<MinionModel>();
    }

    // Use this for initialization
	void Start ()
    {
        
	}

    // Update is called once per frame
    void Update()
    {
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// Room Methods
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void AddRoomToDungeon(GameObject new_room)
    {
        m_roomList.Add(new_room.GetComponent<RoomModel>());
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
        
}
