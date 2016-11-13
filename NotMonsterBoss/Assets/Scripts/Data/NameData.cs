using UnityEngine;
using System.Collections;

using Utilities;

/// <summary>
// This class stores all the data imported from NameData.csv
// It is stored within the NameList in the database
/// </summary>
public class NameData : MonoBehaviour
{
    #region Public Vars
    [CSVColumn ("TITLE")]
    public string title;
    [CSVColumn ("DELIMITER")]
    public string delimiter;
    [CSVColumn ("SUBTITLE")]
    public string subtitle;
    #endregion


    public NameData ()
    {
    }
}
