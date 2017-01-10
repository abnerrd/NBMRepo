using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// The purpose of this script is to contain data for the DungeonManager of ongoing
/// parties of Adventurers going through the Dungeon.
///  
/// Doesn't have to be a MonoBehaviour, but leaving for design functionality. 


public class AdventurerPacket : MonoBehaviour
{
                //  packetReference should be a key or something else.. I don't think it gets passed-by-ref
    public delegate void PacketTimerCompleteCallback(ref AdventurerPacket packetReference);
    public event PacketTimerCompleteCallback Event_TimerComplete;


    //  TODO aherrera : reorder all these variables to not ALL be public.. the problem switching from a struct to a class :-P

    public string adventureTitle;

    public List<AdventurerModel> adventurers;
    public int PartyCount { get { return adventurers.Count; } }
    public bool PartyDead
    {
        get
        {
            foreach (AdventurerModel ad in adventurers)
            {
                if (!ad.isDead)
                {
                    return false;
                }
            }

            return true;
        }
    }

    public enum PacketState
    {
        PARTY_NOT_STARTED,
        PARTY_IN_PROGRESS,
        PARTY_FAILED,
        PARTY_SUCCESS
    }
    protected PacketState m_state;
    public PacketState State { get { return m_state; } }

    // TODO aherrera: exchange this for an id? or dictionary enum? for better comparison
    //    public RoomModel currentRoom;
    //  TODO aherrera : what happens when/if List count gets modified? 
    public int current_room_index;

    //  The timestamp this packet is racing towards to call Event_TimerComplete
    protected int epoch_timer_timestamp;
    protected bool epoch_flag = false;      //  should consider replacing this with something else.. (only used for multiple event calls

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //  TODO aherrera : Check timestamp & Call event
        if(!epoch_flag && m_state == PacketState.PARTY_IN_PROGRESS && Helper.Epoch.IsPastTimestamp(epoch_timer_timestamp))
        {
            DebugLogger.DebugSystemMessage("HANDLING : TIMESTAMP @ " + epoch_timer_timestamp);
            //  TODO aherrera, wspier : does this require a 'ref' keyword?
            epoch_flag = true;
            if(Event_TimerComplete != null)
            {
                //  forgive me
                AdventurerPacket packet_ref = this;
                Event_TimerComplete(ref packet_ref);
            }
        }
    }

    public void initializePacket(string expeditionTitle = "default")
    {
        epoch_timer_timestamp = 0;
        epoch_flag = false;
        m_state = PacketState.PARTY_NOT_STARTED;
        current_room_index = 0;
        adventurers = new List<AdventurerModel>();
        adventureTitle = expeditionTitle;
    }

    /// <summary>
    /// Changes PACKET_STATE to IN_PROGRESS so that the Event_TimerComplete will start calling.
    /// Probably should call this AFTER you SetTimer()
    /// </summary>
    public void StartPacket()
    {
        m_state = PacketState.PARTY_IN_PROGRESS;
    }

    public void addAdventurerToParty(AdventurerModel adventurer)
    {
        adventurers.Add(adventurer);
    }
    
    /// <summary>
    /// Update the epoch timestamp from the (Current time + parameter).
    /// </summary>
    /// <param name="room_time">in seconds</param>
    public void SetTimer(int room_time)
    {
        epoch_timer_timestamp = Helper.Epoch.GetEpochTimestamp(room_time);
        epoch_flag = false;
    }

    public void SetPacketSuccess()
    {
        m_state = PacketState.PARTY_SUCCESS;

        //  TODO aherrera : reward all Adventurers who survived -- maybe put some in a "history"? Legends, memorial, etc.
    }

    public void SetPacketFailure()
    {
        m_state = PacketState.PARTY_FAILED;

        //  TODO aherrera : is there any more punishment after death?
    }

    
}
