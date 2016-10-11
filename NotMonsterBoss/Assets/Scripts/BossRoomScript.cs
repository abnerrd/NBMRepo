using UnityEngine;
using System.Collections;

public class BossRoomScript : RoomScript
{
    [SerializeField]
    private
        BossScript bossReference;

    void Awake()
    {
        _isBossRoom = true;
    }

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public override bool challengeAdventurer(AdventurerScript adventurerChallenging)
    {
        bool retval = challengeUnit(adventurerChallenging);

        return retval;
    }

    /// Returns TRUE if Unit defeats Boss
    public override bool challengeUnit(UnitScript unitChallenging)
    {
        // TODO aherrera: set Unit to FIGHT BOSS!!!!


        return false;
    }

    public override void onFailRoom(ref AdventurerScript adventurer)
    {
        // Apply BOSS dmg here
    }
}
