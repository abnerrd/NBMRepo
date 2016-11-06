using UnityEngine;

public static class EnumUtility 
{
    public static T ParseEnum<T>( string value )
    {
        return (T) System.Enum.Parse( typeof( T ), value, true );
    }


    public static Enums.UnitRarity StringToRarity (string rarity) { return (Enums.UnitRarity)System.Enum.Parse (typeof (Enums.UnitRarity), rarity); }
    public static Enums.RoomType StringToRoomType (string type) { return (Enums.RoomType)System.Enum.Parse (typeof (Enums.RoomType), type); }
}
