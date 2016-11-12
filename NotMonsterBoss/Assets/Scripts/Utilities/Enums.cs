using UnityEngine;
using System.Collections;

/// <summary>
/// This class holds each enum used in 
/// each respective class separated by regions
/// </summary>
public static class Enums {

    #region Unit
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
    #endregion

    #region Room
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

    public enum ChallengeType
    {
        e_challenge_DEX,
        e_challenge_STR,
        e_challenge_WIS,

        e_challenge_count
    }
    #endregion
}
