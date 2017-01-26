using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PartyState
{
    PARTY_NOT_STARTED,
    PARTY_IN_PROGRESS,
    PARTY_FAILED,
    PARTY_SUCCESS
}

public struct Timer
{
    public enum State
    {
        ACTIVE,
        INACTIVE,
        TOTAL
    }

    public State _State { get; private set; }
    // When this timestamp is reached call TimerComplete
    public int   _TargetTimestamp { get; set; }

    public void Pause () { _State = State.INACTIVE;}
    public void Start () { _State = State.ACTIVE;}

    public delegate void TimerCompleteCallback ();
    private event TimerCompleteCallback TimerComplete;

    public void RegisterToEvent (TimerCompleteCallback callback)
    {
        DebugLogger.DebugSystemMessage ("REGISTERED EVENT :" + callback);
        TimerComplete += callback;
    }
    public void UnRegisterToEvent (TimerCompleteCallback callback)
    {
        DebugLogger.DebugSystemMessage ("UNREGISTERED EVENT :" + callback);
        TimerComplete -= callback;
    }

    public void InternalUpdate ()
    {
        if (_State == State.ACTIVE && Helper.Epoch.IsPastTimestamp (_TargetTimestamp)) {
            DebugLogger.DebugSystemMessage ("HANDLING : TIMESTAMP @ " + _TargetTimestamp);
            _State = State.INACTIVE;
            if (TimerComplete != null) {
                TimerComplete ();
            }
        }
    }
}

public class PartyModel : MonoBehaviour {


    public string           _AdventureTitle { get; set; }
    public List<GameObject> _Adventurers { get; set;}
    public PartyState       _State { get; set; }
    public int              _CurrentRoomIndex { get; set; }
    public Timer            _Timer;

    public int GetPartyCount { get { return _Adventurers.Count; } }

    void Start ()
    {
        _Timer.Pause ();
        _Adventurers = new List<GameObject> ();
    }

}
