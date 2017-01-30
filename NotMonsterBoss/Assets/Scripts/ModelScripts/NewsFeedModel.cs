using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewsFeedModel : MonoBehaviour
{
    private List<GameObject> mAlertsList;
    public List<GameObject> AlertsList { get { return mAlertsList; } }

    private bool mShowAlerts = false;
    public bool ShowAlerts { get { return mShowAlerts; } }

	// Use this for initialization
	void Awake ()
    {
        mAlertsList = new List<GameObject>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    /// <summary>
    /// Function assumes all components have already been added.
    /// </summary>
    /// <param name="new_alert_go"></param>
    public void AddNewAlert(GameObject new_alert_go)
    {
        mAlertsList.Add(new_alert_go);
        new_alert_go.SetActive(mShowAlerts);
    }

    public void SetShowAlerts(bool value)
    {
        mShowAlerts = value;

        //  set alerts as Visible/Invisible
        foreach (GameObject go in mAlertsList)
        {
            //  TODO aherrera : access AlertView to set as visible/invisible? or is SetActive() enough?
            go.SetActive(value);
        }
    }

    /// <summary>
    /// Position the alerts stacked on top of each other.
    /// </summary>
    /// <param name="as_shown"></param>
    public void PositionAlerts()
    {
        for(int i = 0; i < mAlertsList.Count; ++i)
        {
            GameObject go = mAlertsList[i];
            Vector3 new_position = Vector3.zero;
            new_position.y += i * 30;
            go.transform.localPosition = new_position;
        }
    }
}
