using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonController : MonoBehaviour
{
    private DungeonView mDungeonView;
    private DungeonModel mDungeonModel;

    //  TODO aherrera : to be removed probably into GameController; he calls the shots
    public GameObject RoomPrefab;
    public GameObject BossRoomPrefab;

    private GameObject MainCanvas;

    /// <summary>
    /// List of all groups of adventurers currently progressing through the Dungeon.
    /// </summary>
    protected Dictionary<string, GameObject> m_questingParties;

    //  TODO aherrera : should consider putting this in a BaseController class
    eControllerState mState;


    void Awake ()
    {
        mState = eControllerState.eControllerState_CREATED;

        mDungeonView = this.gameObject.GetComponent<DungeonView>();
        if (mDungeonView == null)
        {
            mDungeonView = this.gameObject.AddComponent<DungeonView>();
        }

        mDungeonModel = this.gameObject.GetComponent<DungeonModel>();
        if (mDungeonModel == null)
        {
            mDungeonModel = this.gameObject.AddComponent<DungeonModel>();
        }

        m_questingParties = new Dictionary<string, GameObject>();
    }

    private void Start()
    {
    }
	
	// Update is called once per frame
	void Update ()
    {
        //  TODO aherrera : create a method for this pls.. (something like "change state")
        if(mState == eControllerState.eControllerState_READY_TO_INITIALIZE)
        {
            CreateDungeonObject();
            mState = eControllerState.eControllerState_ACTIVE;
        }

        //  TODO aherrera : don't put this here 
                // 1/29     moved this block from DungeomModel.Update -- still not here?
        foreach (GameObject go in m_questingParties.Values)
        {
            PartyController pc = go.GetComponent<PartyController>();
            if (pc != null)
            {
                pc.InternalUpdate();
            }
        }
    }

    public void InitializeDungeon(GameObject main_canvas)
    {
        MainCanvas = main_canvas;
        mState = eControllerState.eControllerState_READY_TO_INITIALIZE;
    }

    protected void CreateDungeonObject()
    {
        //    GameObject playerDungeon = Instantiate(DungeonPrefab);
        RectTransform mc = MainCanvas.GetComponent<RectTransform>();
        mDungeonModel.name = "DUNGEON_1";
        mDungeonView.initialize(mc);

        GameObject newRoomPrefab = Instantiate(BossRoomPrefab);
        RoomModel newBossRoom = RoomGenerator.instance.GenerateUniqueBoss();

        newRoomPrefab.name = newBossRoom.room_name;
        newRoomPrefab.AddComponent<RoomModel>(newBossRoom);
        AddNewRoom(newRoomPrefab);
    }

    public void AddNewRoom(GameObject new_room_prefab)
    {
        mDungeonModel.AddRoomToDungeon(new_room_prefab);
        mDungeonView.AddRoomToDungeon(new_room_prefab);
    }

    public string AddNewParty(GameObject party, bool automatic_start = false)
    {
        PartyController pc = party.GetComponent<PartyController> ();
        PartyModel pm = party.GetComponent<PartyModel> ();
        DebugLogger.DebugSystemMessage("Inserting " + pm._AdventureTitle + " Party to Dungeon");

        pc.RegisterToEvent(HandlePacketTimerUp);
        string newPacketKey = AddPartyToDungeon(party);
        
        return newPacketKey;
    }

    //public string AddNewPacket(List<AdventurerModel> party_list, bool automatic_start = false)
    //{

        
    //}

    public void RemovePacket(string packet_key)
    {
        GameObject party_to_remove = RemovePartyFromDungeon(packet_key);
        PartyController p = party_to_remove.GetComponent<PartyController> ();
        //  RISK aherrera : is this gonna work? Like, since we're not really returning a reference
        //                      to packet_to_remove, will it unregister from the Packet correctly?
        p.UnRegisterToEvent(HandlePacketTimerUp);
    }

    //  TODO aherrera : DEBUG SCRIPT
    public void AddRandomRoom()
    {
        GameObject newRoomPrefab = Instantiate(RoomPrefab);
        RoomModel newRoom = RoomGenerator.instance.GenerateUnique();
        newRoomPrefab.name = newRoom.room_name;
        newRoomPrefab.AddComponent<RoomModel>(newRoom);

        AddNewRoom(newRoomPrefab);
    }

    //  TODO aherrera : DEBUG SCRIPT; kill this before it's too late
    public void AddRandomParty()
    {
        GameObject new_Adventurer = new GameObject ();
        AdventurerModel new_model = AdventurerGenerator.instance.GenerateRandom(ref new_Adventurer, 1, Enums.UnitRarity.e_rarity_COMMON);
        GameObject new_party = new GameObject();
        PartyModel go_pm = new_party.AddComponent<PartyModel> ();
        PartyController go_pc = new_party.AddComponent<PartyController> ();

        //AdventurerPacket newpackofcigs = go_adventurerpacket.AddComponent<AdventurerPacket>();
        new_party.name = "AD_PACK: " + go_pm._AdventureTitle;
        go_pc.InitializeParty();
        go_pm._Adventurers.Add(new_Adventurer);

        RoomModel room_model = mDungeonModel.GetRoom(go_pc.GetCurrentRoomIndex());

        AddNewParty(new_party, true);
        go_pc.BeginRoom(room_model.timer_frequency);
    }

    protected void HandlePacketTimerUp(string packet_key)
    {
        //  TODO aherrera : Challenge room, update accordingly

        if (ChallengePacketInCurrentRoom(packet_key))
        {
            //  STATE :: Party should still be ALIVE
            //  Actually progress Packet
            UpdatePartyToNextRoom(packet_key);
        }
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// Party Methods
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////


    protected void UpdatePartyToNextRoom(string party_key)
    {
        NewsFeedController.instance.CreateNewAlert(party_key + " Party has entered the next room!");
        DebugLogger.DebugSystemMessage("UPDATING PARTY TO NEXT ROOM");
        if (UpdatePacketToNextRoom(party_key))
        {
            //  STATE :: Packet has defeated dungeon!
            //  celebrate
        }
    }

    public string AddPartyToDungeon(GameObject new_party)
    {
        PartyController pc = new_party.GetComponent<PartyController>();
        //  TODO aherrera : probably make sure i'm unique
        string packet_key = pc.GetAdventureTitle();
        m_questingParties.Add(packet_key, new_party);

        InitializePacketToRoom(packet_key, mDungeonModel.GetEntrance());

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

    public List<string> GetPartyKeysList()
    {
        return new List<string>(m_questingParties.Keys);
    }

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
        PartyController party = party_object.GetComponent<PartyController>();
        if (party != null)
        {
            RoomModel party_current_room = mDungeonModel.GetRoom(party.GetCurrentRoomIndex());
            if (party_current_room != null)
            {
                bool challenge_successful = party_current_room.challengeParty(party.GetAdventurers());
                if (challenge_successful)
                {
                    //party_packet.AwardPacket(party_current_room);
                }
                else
                {
                    ResolveChallengeFailure(party, party_current_room);

                }
            }
            else { Debug.LogError("DungeonModel::ChallengePacketInCurrentRoom -- room not in list from packet: " + party.GetAdventureTitle()); }

            //  TODO aherrera, wspier : like, should this logic be in the Model?? Should it be in charge to figure it out?
            if (party.isPartyDead())
            {
                party.SetState(PartyState.PARTY_FAILED);
                NewsFeedController.instance.CreateNewAlert("Party just died! lol");
            }

            return !party.isPartyDead();
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
        PartyController party = m_questingParties[packet_key].GetComponent<PartyController>();
        if (party != null)
        {
            //  We move in the list from last->first
            bool completed_dungeon = (0 > party.AdvanceRoom());

            if (completed_dungeon)
            {
                //  TODO aherrera : Party has defeated the boss? 
                party.SetState(PartyState.PARTY_SUCCESS);
            }
            else
            {
                RoomModel party_new_current_room = mDungeonModel.GetRoom(party.GetCurrentRoomIndex());
                if (party_new_current_room != null)
                {
                    InitializePacketToRoom(packet_key);
                }
            }

            return completed_dungeon;
        }
        else { Debug.LogError("DungeonModel::UpdatePacketToNextRoom -- packet not found in list with string: " + packet_key); }

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
        if (new_room_index >= 0)
        {
            party.SetCurrentRoomIndex(new_room_index);
        }

        //  TODO aherrera : replace this check with something more elegant :/
        if (new_room_index >= mDungeonModel.RoomCount)
        {
            Debug.LogError("GIVEN INDEX LARGER THAN ARRAY SIZE");
        }

        RoomModel current_room_reference = mDungeonModel.GetRoom(party.GetCurrentRoomIndex());

        if (current_room_reference != null)
        {
            party.SetRoomTimer(Helper.Epoch.GetEpochTimestamp(current_room_reference.timer_frequency));
        }
        else { Debug.LogError("DungeonModel::InitializePacketToCurrentRoom -- currentroom not found for string: " + packet_key); }

    }

    public void ResolveChallengeFailure(PartyController party, RoomModel room)
    {
        foreach (GameObject adventurer in party.GetAdventurers())
        {
            AdventurerModel a = adventurer.GetComponent<AdventurerModel>();
            a.applyDamage(room.room_attack);
        }
    }


}
