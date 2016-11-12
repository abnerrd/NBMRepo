using UnityEngine;
using System.Collections;

/// <summary>
// Base Unit Script for entities
/// </summary>
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

    private
        Enums.UnitRarity m_rarity = Enums.UnitRarity.e_rarity_none;
    public Enums.UnitRarity rarity { get { return m_rarity; } set { m_rarity = value; } }

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
        if (m_health_current < 0) m_health_current = 0 ;   
    }

}
