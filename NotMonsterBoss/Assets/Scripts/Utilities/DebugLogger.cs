

using UnityEngine;
using System.Collections;
using System;

public class DebugLogger
{

    public static void DebugChallenge (int roll, int max, Enums.ChallengeType challange)
    {
        Colors color = Colors.black;
        string challengeString = "";
        switch (challange) 
        {
            case Enums.ChallengeType.e_challenge_DEX: 
                color = Colors.olive;
                challengeString = "DEX";
                break;
            case Enums.ChallengeType.e_challenge_STR:  
                color = Colors.red;
                challengeString = "STR";    
                break;
            case Enums.ChallengeType.e_challenge_WIS: 
                color = Colors.blue;
                challengeString = "WIS";
                break;
        }

        string result = (roll >= max ? "SUCCESS" : "FAILURE");
        Colors resultColor = (roll >= max ? Colors.lime : Colors.grey);

        string message = " check result: " + roll + "/" + max + " - ";

        Debug.Log (challengeString.Bold ().Colored (color) + message + result.Colored (resultColor));
    }

    public static void DebugRoomResult (int pass, int max, RoomScript room, UnitScript adventurer)
    {
        string passed = pass.ToString () + "/" + max.ToString () + " challenges passed";
        string result = (pass >= max ? room.success : room.failure);
        string name = adventurer._unitName + " ";
        Debug.Log (passed + " | " + name + result);
    }

    public static void DebugRoomTransition (RoomScript room, UnitScript unit)
    {
        string unitName = unit._unitName;
        string roomName = room.room_name;

        Debug.Log (unitName.Bold() + " has now entered " + roomName.Italics());
    }

    public static void DebugUnitDamage (int damage, UnitScript unit)
    {
        string unitName = unit._unitName;
        string message = " has taken " + damage.ToString () + " damage!";
        int remainingHP = unit.currentHealth - damage;
        string remainingHealth = (remainingHP > 0 ? unitName + " has " + remainingHP.ToString() + " remaining" :
                                  unitName + " died!");
        Colors remainingHPColor = (remainingHP > 0 ? Colors.green : Colors.red);

        Debug.Log ((unitName + message).Bold());
        Debug.Log (remainingHealth.Colored(remainingHPColor));
    }

    public static void DebugSystemMessage (string message)
    {
        Debug.Log (message.Colored (Colors.purple));
    }

    public static void DebugGameObver ()
    {
        Debug.Log (("Everyone died. The End").Bold().Colored(Colors.red));
    }






    //Unused

    public static void DebugTimeStep (float num, string step)
    {

        string actionLabel = "TIME_STEP: ";

        string message = num + " " + step + " have passed.";
        Debug.Log (actionLabel.Bold ().Sized (12).Colored (Colors.purple) + " " + message.Sized (10));

    }
    public static void DebugWait (float num)
    {
        string actionLabel = "WAIT: ";

        string message = num + " seconds.";
        Debug.Log (actionLabel.Bold ().Sized (12).Colored (Colors.white) + " " + message.Sized (10));
    }


}

public static class StringLoggingExtensions
{
    public static string Colored (this string message, Colors color)
    {
        return string.Format ("<color={0}>{1}</color>", color.ToString (), message);
    }
    public static string Colored (this string message, string colorCode)
    {
        return string.Format ("<color={0}>{1}</color>", colorCode, message);
    }
    public static string Sized (this string message, int size)
    {
        return string.Format ("<size={0}>{1}</size>", size, message);
    }
    public static string Bold (this string message)
    {
        return string.Format ("<b>{0}</b>", message);
    }
    public static string Italics (this string message)
    {
        return string.Format ("<i>{0}</i>", message);
    }
}

public enum Colors
{
    aqua,
    black,
    blue,
    brown,
    cyan,
    darkblue,
    fuchsia,
    green,
    grey,
    lightblue,
    lime,
    magenta,
    maroon,
    navy,
    olive,
    purple,
    red,
    silver,
    teal,
    white,
    yellow
}
