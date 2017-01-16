using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonController : MonoBehaviour
{
    private DungeonView mDungeonView;
    private DungeonModel mDungeonModel;

    public GameObject RoomPrefab;
    public GameObject BossRoomPrefab;
    public GameObject MainCanvas;


    //  TODO aherrera : should consider putting this in a BaseController class
    eControllerState mState;


    void Awake ()
    {
        mState = eControllerState.eControllerState_CREATED;

        mDungeonView = this.gameObject.GetComponent<DungeonView>();
        mDungeonModel = this.gameObject.GetComponent<DungeonModel>();
        if (mDungeonView == null)
        {
            mDungeonView = this.gameObject.AddComponent<DungeonView>();
        }
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
        //  TODO aherrera : create a method for this pls..
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

    public string AddNewPacket(AdventurerPacket party_packet, bool automatic_start = false)
    {
        DebugLogger.DebugSystemMessage("Inserting " + party_packet.adventureTitle + " Party to Dungeon");

        party_packet.RegisterToEvent_TimerComplete(HandlePacketTimerUp);
        string newPacketKey = mDungeonModel.AddPartyToDungeon(party_packet);

        if (automatic_start)
        {
            StartPacket(newPacketKey);
        }

        return newPacketKey;
    }

    //public string AddNewPacket(List<AdventurerModel> party_list, bool automatic_start = false)
    //{

        
    //}

    public void RemovePacket(string packet_key)
    {
        AdventurerPacket packet_to_remove = mDungeonModel.RemovePartyFromDungeon(packet_key);

        //  RISK aherrera : is this gonna work? Like, since we're not really returning a reference
        //                      to packet_to_remove, will it unregister from the Packet correctly?
        packet_to_remove.UnregisterToEvent_TimerComplete(HandlePacketTimerUp);
    }

    protected void StartPacket(string packet_key)
    {
        mDungeonModel.StartPartyPacket(packet_key);
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

    //  TODO aherrera : DEBUG SCRIPT
    public void AddRandomPacket()
    {
        AdventurerModel new_model = AdventurerGenerator.instance.GenerateRandom(1, Enums.UnitRarity.e_rarity_COMMON);
        GameObject go_adventurerpacket = new GameObject();
        AdventurerPacket newpackofcigs = go_adventurerpacket.AddComponent<AdventurerPacket>();
        go_adventurerpacket.name = "AD_PACK: " + newpackofcigs.adventureTitle;
        newpackofcigs.initializePacket("cig crew");
        newpackofcigs.adventurers.Add(new_model);

        AddNewPacket(newpackofcigs, true);
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
