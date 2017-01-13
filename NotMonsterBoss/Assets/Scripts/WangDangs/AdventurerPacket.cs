using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// The purpose of this script is to contain data for the DungeonManager of ongoing
/// parties of Adventurers going through the Dungeon.
///  
/// Doesn't have to be a MonoBehaviour, but leaving for design functionality. 


public class AdventurerPacket : MonoBehaviour
{
    public delegate void PacketTimerCompleteCallback(string packet_key);
    private event PacketTimerCompleteCallback Event_TimerComplete;

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
    //  TODO aherrera : what happens when/if List count gets modified? 
    public int current_room_index;

    //  The timestamp this packet is racing towards to call Event_TimerComplete
    protected int epoch_timer_timestamp;
    protected bool epoch_flag = false;      //  should consider replacing this with something else.. (only used for multiple event calls)

    protected string dictionaryKey;
    public string DictionaryKey { get { return dictionaryKey; } }

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
            epoch_flag = true;
            if(Event_TimerComplete != null)
            {
                Event_TimerComplete(dictionaryKey);
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
    /// <param name="dictionary_key">Needs a key to start..?</param>
    public void StartPacket(string dictionary_key)
    {
        dictionaryKey = dictionary_key;
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

    /// <summary>
    /// Returns:  Current Room Index -= 1
    /// </summary>
    /// <returns></returns>
    public int AdvanceIndex()
    {
        return --current_room_index;
    }

    //  TODO aherrera, wspier: please rename this + parameter
    public void AwardPacket(RoomModel parental_room)
    {

    }

    //  TODO aherrera, wspier: please rename this + parameter
    /// <summary>
    /// With RoomModel param, deal consequences of room to Party
    /// </summary>
    public void PunishPacket(RoomModel parental_room)
    {
        //  HEY THIS MIGHT NOT WORK BUUUUUUUUUT PUTTING IT IN BEFORE I COMMIT [1/10]
        foreach(AdventurerModel ad_mod in adventurers)
        {
            ad_mod.applyDamage(parental_room.room_attack);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void SetPacketDungeonCompleted()
    {
        DebugLogger.DebugSystemMessage("PACKET HAS DEFEATED DUNGEON. GOOD JOB " + adventureTitle);

        m_state = PacketState.PARTY_SUCCESS;

        //  TODO aherrera : reward all Adventurers who survived -- maybe put some in a "history"? Legends, memorial, etc.
    }

    public void SetPacketDungeonFailure()
    {
        m_state = PacketState.PARTY_FAILED;

        //  TODO aherrera : is there any more punishment after death?
    }


    //  TODO aherrera : for these two functions, should there be a list holding all these "listeners"? What if the wrong callback is sent...?
    public void RegisterToEvent_TimerComplete(PacketTimerCompleteCallback callback)
    {
        DebugLogger.DebugSystemMessage("REGISTERING TO EVENT FOR :" + this.adventureTitle);
        Event_TimerComplete += callback;
    }

    public void UnregisterToEvent_TimerComplete(PacketTimerCompleteCallback callback)
    {
        DebugLogger.DebugSystemMessage("UNREGISTERING TO EVENT FOR :" + this.adventureTitle);
        Event_TimerComplete -= callback;
    }
    
}
