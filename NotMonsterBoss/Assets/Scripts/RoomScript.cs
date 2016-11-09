using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Utilities;

// Rooms make up a Dungeon
// Each room has a timer, and when completed it'll set itself as done
//
//  Dungeons are in charge of resetting a room, as well as triggering on room timer effects

public class RoomScript : MonoBehaviour
{
    [SerializeField]
    [Tooltip("seconds")]
    private
        int m_timer_frequency;
    public int timer_frequency { get { return m_timer_frequency; } set { m_timer_frequency = value; } }   //  in seconds

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

    [SerializeField]
    private
        int m_passes_required = 0;
    public int pass_req { get { return  m_passes_required; } set { m_passes_required = value; } }

    [SerializeField]
    private
        string m_success_description = "";

    public string success { get { return m_success_description; } set { m_success_description = value; } }
    [SerializeField]
        string m_failure_description = "";
    public string failure { get { return m_failure_description; } set { m_failure_description = value; } }


    [SerializeField]
    private
        string m_description = "";
    public
        string description { get { return m_description; } set { m_description = value; } }

    [SerializeField]
    private
        string m_name = "";
    public
        string room_name { get { return m_name; } set { m_name = value; } }

    [SerializeField]
    private
        Enums.RoomType m_type;
    public 
        Enums.RoomType type { get { return m_type; } set { m_type = value; } }
    [SerializeField]
    private
        Enums.UnitRarity m_rarity;
    public
        Enums.UnitRarity rarity { get { return m_rarity; } set { m_rarity = value; } }

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
        int cr = (int)challengeRoll (adventurerDexterity);

        Debug.Log ("Dexterity Challenge roll: " + cr + " / " + m_challenge_dexterity);

        if ( cr >= m_challenge_dexterity)
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

        int cr = (int)challengeRoll (adventurerStrength);

        Debug.Log ("Strength Challenge roll: " + cr + " / " + m_challenge_strength);

        if (cr >= m_challenge_strength)
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

        int cr = (int)challengeRoll (adventurerWisdom);

        Debug.Log ("Wisdom Challenge roll: " + cr + " / " + m_challenge_wisdom);

        if (cr >= m_challenge_wisdom)
            retVal = true;

        return retVal;
    }
    
    public virtual bool challengeUnit(UnitScript unitChallenging)
    {
        int challengesPassed = 0;

        if (challengeDexterity (unitChallenging.dexterity)) challengesPassed++;
        if (challengeStrength (unitChallenging.strength)) challengesPassed++;
        if (challengeWisdom (unitChallenging.wisdom)) challengesPassed++;

        Debug.Log (challengesPassed + " out of 3 challenges passed!");

        return (challengesPassed >= m_passes_required);
    }

    public virtual bool challengeAdventurer(AdventurerScript adventurerChallenging)
    {
        bool retval = challengeUnit(adventurerChallenging);

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
                    //Adventurer has lost
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
        Debug.Log (adventurer._unitName + " has take " + m_attack_damage + " damage!");

        adventurer.applyDamage(m_attack_damage);

        Debug.Log (adventurer._unitName + " now has " + adventurer.currentHealth + " health");
    }

    private float challengeRoll (float mod)
    {
        return Random.Range((mod),(mod+10));
    }

}
