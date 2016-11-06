using UnityEngine;
using System.Collections;

using Utilities;

public class RoomData : MonoBehaviour
{
    #region Public Vars
    [CSVColumn ("NAME")]
    public string name;
    [CSVColumn ("DESCRIPTION")]
    public string description;
    [CSVColumn ("ATTACK_DAMAGE")]
    public int attackDamage;
    [CSVColumn ("CR_DEX")]
    public int cr_dex;
    [CSVColumn ("CR_STR")]
    public int cr_str;
    [CSVColumn ("CR_WIS")]
    public int cr_wis;
    [CSVColumn ("IS_BOSS")]
    public bool is_boss;
    [CSVColumn ("RARITY")]
    public string rarity;
    [CSVColumn ("TYPE")]
    public string type;
    [CSVColumn ("SUCCESS")]
    public string success;
    [CSVColumn ("FAILURE")]
    public string failure;
    #endregion


    public RoomData ()
    {
    }
}
