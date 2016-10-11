using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Rooms make up a Dungeon
// Each room has a timer, and when completed it'll set itself as done
//
//  Dungeons are in charge of resetting a room, as well as triggering on room timer effects

// TODO aherrera : move these definitions into their own class

/// <summary>
/// 
/// </summary>
public enum RoomType
{
    e_room_none = -1,
    e_room_MINION,
    e_room_TRAP,

    e_room_count
}

public class RoomScript : MonoBehaviour
{
    [SerializeField]
    [Tooltip("seconds")]
    private
        float m_timer_frequency;
    public float timer_frequency { get { return m_timer_frequency; } set { m_timer_frequency = value; } }   //  in seconds

    [SerializeField]
    private
       int m_challenge_dexterity = 0;
    public int dexterity_challenge { get { return m_challenge_dexterity; } set { m_challenge_dexterity = value; } }

    [SerializeField]
    private
        int m_challenge_strength;
    public int strength_challenge { get { return m_challenge_strength; } set { m_challenge_strength = value; } }

    [SerializeField]
    private
        int m_challenge_wisdom = 0;
    public int wisdom_challenge { get { return m_challenge_wisdom; } set { m_challenge_wisdom = value; } }

    [SerializeField]
    private
       int m_attack_damage = 0;
    public int room_attack { get { return m_attack_damage; } set { m_attack_damage = value; } }


    // TODO aherrera: add in cost for room?

    // TODO aherrera: room types?

    // TODO aherrera: extend to be an inherited class? BossRoom?
    public bool _isBossRoom = false;


    // Use this for initialization
    void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {

       
	}

    /// <summary>
    /// Compare a given stat to see if it passes this room's challenge
    /// </summary>
    /// <param name="adventurerDexterity"></param>
    /// <returns>TRUE if stat beat challenge rating for room</returns>
    public bool challengeDexterity(float adventurerDexterity)
    {
        bool retVal = false;

        // TODO aherrera: Implement BETTER stat adding and D&D stuff
        if (adventurerDexterity >= m_challenge_dexterity)
            retVal = true;

        return retVal;
    }

    /// <summary>
    /// Compare a given stat to see if it passes this room's challenge
    /// </summary>
    /// <param name="adventurerStrength"></param>
    /// <returns>TRUE if stat beat challenge rating for room</returns>
    public bool challengeStrength(float adventurerStrength)
    {
        bool retVal = false;

        // TODO aherrera: Implement more stat adding and D&D stuff
        if (adventurerStrength >= m_challenge_strength)
            retVal = true;

        return retVal;
    }

    /// <summary>
    /// Compare a given stat to see if it passes this room's challenge
    /// </summary>
    /// <param name="adventurerWisdom"></param>
    /// <returns>TRUE if stat beat challenge rating for room</returns>
    public bool challengeWisdom(float adventurerWisdom)
    {
        bool retVal = false;

        // TODO aherrera: Implement more stat adding and D&D stuff
        if (adventurerWisdom >= m_challenge_wisdom)
            retVal = true;

        return retVal;
    }
    
    public virtual bool challengeUnit(UnitScript unitChallenging)
    {
        Debug.Log("TODO aherrera: change these up to be more D&D exciting and stuff");
        return (challengeStrength(unitChallenging.strength) && challengeDexterity(unitChallenging.dexterity) && challengeWisdom(unitChallenging.wisdom));
    }

    public virtual bool challengeAdventurer(AdventurerScript adventurerChallenging)
    {
        bool retval = challengeUnit(adventurerChallenging);

        // TODO aherrera: extra adventurer stuff to do? I might even take this out if it's just easier to do unit challenging..

        return retval;
    }

    public virtual bool challengeParty(List<AdventurerScript> adventureParty)
    {
        bool retval = true;

        foreach(AdventurerScript adventurer in adventureParty)
        {
            if(!adventurer.isDead)
            {
                if(!challengeAdventurer(adventurer))
                {
                    Debug.Log("TODO aherrera: make determing the outcome for the Party to be more interseting");
                    retval = false;
                    break;
                }
            }
        }

        return retval;
    }

    // Set all variables as BLANK; a free pass for Adventurers
    public virtual void blankRoom()
    {
        m_challenge_dexterity = m_challenge_strength = m_challenge_wisdom = m_attack_damage = 0;
    }

    public virtual void onFailRoom(ref AdventurerScript adventurer)
    {
        adventurer.applyDamage(m_attack_damage);
    }

}
