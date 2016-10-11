using UnityEngine;
using System.Collections;

// Base Unit Script for entities

// TODO aherrera: Move these definitions into their own class

/// <summary>
/// Defines the race of the Unit; Room synergy
/// </summary>
public enum UnitTypes
{
    e_type_none = -1,
    e_type_BEAST,
    e_type_HUMANOID,
    e_type_UNDEAD,
    e_type_DEMON,

    e_type_count
}

/// <summary>
/// General power level of Unit
/// </summary>
public enum UnitRarity
{
    e_rarity_none = -1,
    e_rarity_COMMON,
    e_rarity_RARE,
    e_rarity_EPIC,
    e_rarity_LEGEND,

    e_rarity_count
}

public class UnitScript : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField]
    private
        int m_health_current = 0;
    public int currentHealth { get { return m_health_current; } set { m_health_current = value; } }
    public bool isDead { get { return (m_health_current <= 0); } }

    private 
        int m_health_total = 1;
    public int totalHealth { get { return m_health_total; } set { m_health_total = value; } }

    [SerializeField]
    [Tooltip("Flat damage to other Units")]
    private
        int m_attack_damage = 0;
    public int attack_damage { get { return m_attack_damage; } set { m_attack_damage = value; } }

    [SerializeField]
    private
        int m_stat_dexterity = 0;
    public int dexterity { get { return m_stat_dexterity; } set { m_stat_dexterity = value; } }

    [SerializeField]
    private
        int m_stat_strength;
    public int strength { get { return m_stat_strength; } set { m_stat_strength = value; } }

    [SerializeField]
    private
        int m_stat_wisdom = 0;
    public int wisdom { get { return m_stat_wisdom; } set { m_stat_wisdom = value; } }

    [Header("Flavor")]
    public string _unitName = "Jeb";
    public string _unitDescription = "Lowly minion.";

    void Awake()
    {
        m_health_current = m_health_total;
    }

   // Use this for initialization
   void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void applyDamage(int dmg)
    {
        m_health_current -= dmg;
    }

}
