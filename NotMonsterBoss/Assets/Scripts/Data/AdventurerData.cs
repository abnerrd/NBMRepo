using UnityEngine;
using System.Collections;

using Utilities;

/// <summary>
// This class stores all the data imported from AdventurerData.csv
// It is stored within the AdventurerList in the database
// Rarity is an Enum and must be converted using the EnumUtility
/// </summary>
public class AdventurerData
{
    #region Public Vars
    [CSVColumn ("NAME")]
    public string name;
    [CSVColumn ("DESCRIPTION")]
    public string description;
    [CSVColumn ("ATTACK_DAMAGE")]
    public int attackDamage;
    [CSVColumn ("DEX_MOD")]
    public int dex_mod;
    [CSVColumn ("STR_MOD")]
    public int str_mod;
    [CSVColumn ("WIS_MOD")]
    public int wis_mod;
    [CSVColumn ("MAX_HEALTH")]
    public int max_health;
    [CSVColumn ("RARITY")]
    public string rarity;
    #endregion


    public AdventurerData ()
    {
    }
}
