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
        mDungeonModel = mDungeonModel.GetComponent<DungeonModel>();
        mDungeonModel.init(mc);
        mDungeonModel.name = "DUNGEON_1";

        GameObject newRoomPrefab = Instantiate(BossRoomPrefab);
        RoomModel newBossRoom = RoomGenerator.instance.GenerateUniqueBoss();

        newRoomPrefab.name = newBossRoom.room_name;
        newRoomPrefab.AddComponent<RoomModel>(newBossRoom);
        mDungeonModel.AddRoomToDungeon(newRoomPrefab);
    }

    public void AddNewRoom(GameObject new_room_prefab)
    {
        mDungeonModel.AddRoomToDungeon(new_room_prefab);
    }

    public string AddNewParty(GameObject party, bool automatic_start = false)
    {
        PartyController pc = party.GetComponent<PartyController> ();
        PartyModel pm = party.GetComponent<PartyModel> ();
        DebugLogger.DebugSystemMessage("Inserting " + pm._AdventureTitle + " Party to Dungeon");

        pc.RegisterToEvent(HandlePacketTimerUp);
        string newPacketKey = mDungeonModel.AddPartyToDungeon(party);
        
        return newPacketKey;
    }

    //public string AddNewPacket(List<AdventurerModel> party_list, bool automatic_start = false)
    //{

        
    //}

    public void RemovePacket(string packet_key)
    {
        GameObject party_to_remove = mDungeonModel.RemovePartyFromDungeon(packet_key);
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
        AdventurerModel new_model = new_Adventurer.AddComponent<AdventurerModel>(AdventurerGenerator.instance.GenerateRandom(1, Enums.UnitRarity.e_rarity_COMMON));
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

        if (mDungeonModel.ChallengePacketInCurrentRoom(packet_key))
        {
            //  STATE :: Party should still be ALIVE
            //  Actually progress Packet
            UpdatePartyToNextRoom(packet_key);
        }
    }

    protected void UpdatePartyToNextRoom(string party_key)
    {
        DebugLogger.DebugSystemMessage("UPDATING PARTY TO NEXT ROOM");
        if (mDungeonModel.UpdatePacketToNextRoom(party_key))
        {
            //  STATE :: Packet has defeated dungeon!
            //  celebrate
        }
    }


}
