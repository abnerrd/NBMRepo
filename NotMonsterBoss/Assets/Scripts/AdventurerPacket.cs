using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// The purpose of this script is to contain data for the DungeonManager of ongoing
/// parties of Adventurers going through the Dungeon.
///  
/// Doesn't have to be a MonoBehaviour, but leaving for design functionality. 


public class AdventurerPacket : MonoBehaviour
{
    //  TODO aherrera : reorder all these variables to not ALL be public.. the problem switching from a struct to a class :-P

    public string adventureTitle;

    public List<AdventurerScript> adventurers;
    public int PartyCount { get { return adventurers.Count; } }
    public bool PartyDead
    {
        get
        {
            foreach (AdventurerScript ad in adventurers)
            {
                if (!ad.isDead)
                {
                    return false;
                }
            }

            return true;
        }
    }

    // TODO aherrera: exchange this for an id? or dictionary enum? for better comparison
    public RoomScript currentRoom;

    public bool failedExpedition;

    public void triggerAsFailed() { failedExpedition = true; }

    public float timer;
    //  TODO aherrera : Temp variable for now.. well, maybe not tbh.
    protected float timer_counter;

    protected bool m_timerComplete;
    public bool TimerComplete { get { return m_timerComplete; } }
    
    public bool PacketCompleteAcknowledged = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdatePacket()
    {
        timer_counter -= Time.fixedDeltaTime;
        m_timerComplete = timer_counter <= 0;
    }

    public void initializePacket(string expeditionTitle = "default")
    {
        timer = 0;
        failedExpedition = false;
        currentRoom = null;
        adventurers = new List<AdventurerScript>();
        adventureTitle = expeditionTitle;
    }

    public void addAdventurerToParty(AdventurerScript adventurer)
    {
        adventurers.Add(adventurer);
    }
    
    protected bool setTimerToCurrentRoom()
    {
        if (currentRoom != null)
        {
            timer = currentRoom.timer_frequency;
            return true;
        }

        return false;
    }

    public bool StartPacket()
    {
        bool retVal = false;
        if(setTimerToCurrentRoom())
        {
            timer_counter = timer;
            PacketCompleteAcknowledged = false;
            retVal = true;
        }
        else
        {
            Debug.LogWarning("Adventurer Packet (" + adventureTitle + ") -- null Current Room to Start?");
        }
        
        return retVal;
    }


    
}
