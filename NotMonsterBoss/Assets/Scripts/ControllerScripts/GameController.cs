using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Utilities;

/*
 *  Purpose of this class it to handle logic and push along the flow of the game.
 *  It'll recieve inputs & actions from the user and perform the appropriate commands and methods.
 */


//  TODO aherrera : Should I make a BaseController class?
public enum eControllerState
{
    eControllerState_CREATED,
    eControllerState_READY_TO_INITIALIZE,
    eControllerState_ACTIVE,
    eControllerState_PAUSED,    //  To pause.. the controller? Like block input, etc.?

    eControllerState_COUNT
}

public class GameController : MonoBehaviour
{
    ////////////////////////////////////////////////////////////
    ///     Prefabs
    ////////////////////////////////////////////////////////////
    public GameObject DungeonPrefab;


    //  TODO aherrera, wspier : eventually replace with List of Dungeons for multiple Dungeons?
    protected DungeonController m_playerDungeon;

    public GameObject MainCanvas;

    private void Awake()
    {
        if(m_playerDungeon == null)
        {
            GameObject new_player_dungeon = Instantiate(DungeonPrefab);
            m_playerDungeon = new_player_dungeon.GetComponent<DungeonController>();
            if(m_playerDungeon == null)
            {
                m_playerDungeon = new_player_dungeon.AddComponent<DungeonController>();
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        m_playerDungeon.InitializeDungeon(MainCanvas);
    }

    // Update is called once per frame
    void Update()
    {

    }

}
