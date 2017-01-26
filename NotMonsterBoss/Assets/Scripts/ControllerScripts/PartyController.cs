using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyController : MonoBehaviour {

    private PartyModel mModel;

    //Getters
    public int GetCurrentRoomIndex ()         { return mModel._CurrentRoomIndex; }
    public List<GameObject> GetAdventurers () { return mModel._Adventurers; }
    public string GetAdventureTitle ()        { return mModel._AdventureTitle; }

    //Setters
    public void SetState (PartyState state)     { mModel._State = state; }
    public void SetCurrentRoomIndex (int index) { mModel._CurrentRoomIndex = index; }
    public void SetRoomTimer (int time) {
        mModel._Timer._TargetTimestamp = time;
        mModel._Timer.Start ();
    }


    //This function is called when the timer is finished
    public void TimerComplete () {
        EventComplete (mModel._AdventureTitle);
    }

    public delegate void EventCompleteCallback (string key);
    private event EventCompleteCallback EventComplete;

    public void RegisterToEvent (EventCompleteCallback callback)
    {
        DebugLogger.DebugSystemMessage ("REGISTERED EVENT :" + callback);
        EventComplete += callback;
    }
    public void UnRegisterToEvent (EventCompleteCallback callback)
    {
        DebugLogger.DebugSystemMessage ("UNREGISTERED EVENT :" + callback);
        EventComplete -= callback;
    }


    //Returns true if each adventurer in the party isDead
    public bool isPartyDead ()
    {
        foreach (GameObject adventurer in mModel._Adventurers)
        {
            AdventurerModel ad = adventurer.GetComponent<AdventurerModel> ();
            if (!ad.isDead)
            {
                return false;
            }
        }
        return true;
    }
    
	void Awake () {
        
        //  TODO aherrera : fix this title initialize
        //Store reference of THIS model
        mModel = gameObject.GetComponent<PartyModel> ();
        mModel._AdventureTitle = "dingus";
	}

    public void InternalUpdate ()
    {
        mModel._Timer.InternalUpdate ();
    }

    public void InitializeParty ()
    {
        //Register TimerComplete Event
        mModel._Timer.RegisterToEvent (TimerComplete);
        mModel._State = PartyState.PARTY_NOT_STARTED;
        mModel._CurrentRoomIndex = 0;
    }

    public void BeginRoom (int room_time)
    {
        mModel._Timer._TargetTimestamp = Helper.Epoch.GetEpochTimestamp (room_time);
        mModel._Timer.Start ();
        mModel._State = PartyState.PARTY_IN_PROGRESS;
    }

    public int AdvanceRoom ()
    {
        return --mModel._CurrentRoomIndex;
    }

}
