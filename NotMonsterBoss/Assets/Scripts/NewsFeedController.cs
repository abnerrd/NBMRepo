using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class NewsFeedController : MonoBehaviour
{ 
    public static NewsFeedController instance = null;

    //  TODO aherrera : replace this with a reference to Resources.?
    public GameObject _AlertFeedPrefab;

    protected GameObject mAlertsContainer;
    public Transform _ShowFeedPoint;
    public Transform _HideFeedPoint;

    private List<GameObject> mAlertsList;

    private bool mShowAlerts = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        mAlertsList = new List<GameObject>();

        if(mAlertsContainer == null)
        {
            mAlertsContainer = new GameObject();
            mAlertsContainer.name = "Alerts Container";
            mAlertsContainer.transform.SetParent(this.transform);

            //  TODO aherrera : change this when it's not just appearing/disappearing
            mAlertsContainer.transform.position = _ShowFeedPoint.position;
        }
    }

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void AddNewAlert(string message)
    {
        //  TODO aherrera : Create "AlertModel", view, controller? For more control over what we can do with these alerts? Like buttons, callbacks, etc.?
        //                  It'll provide some headache relief to getting the Text in following way
        GameObject new_alert_go = Instantiate(_AlertFeedPrefab);
        Text alert_message = new_alert_go.transform.FindChild("Text").GetComponent<Text>();
        alert_message.text = message;

        mAlertsList.Add(new_alert_go);

        new_alert_go.transform.SetParent(mAlertsContainer.transform);
        new_alert_go.transform.localPosition = Vector3.zero;
        
        Vector3 new_position = new_alert_go.transform.position;
        foreach (GameObject go in mAlertsList)
        {
            new_position.y += 30;
        }
        new_alert_go.transform.position = new_position;

        new_alert_go.SetActive(mShowAlerts);

        if(mShowAlerts)
        {
            //  Position accordingly!
        }
    }

    protected GameObject GetNewestAlert()
    {
        if(mAlertsList.Count > 0)
        {
            return mAlertsList[mAlertsList.Count - 1];
        }
        else
        {
            return null;
        }
    }

    protected GameObject GetOldestAlert()
    {
        return mAlertsList[0];
    }

    public void ToggleAlerts()
    {
        ShowAlerts(!mShowAlerts);
    }

    public void ShowAlerts(bool show_alerts)
    {
        mShowAlerts = show_alerts;

        //  set alerts as Visible/Invisible
        foreach (GameObject go in mAlertsList)
        {
            //  TODO aherrera : access something else just to set as visible/invisible? or is SetActive() enough?
            go.SetActive(show_alerts);
        }

        if (show_alerts)
        {
            //  Position List accordingly
                //  For now, i'm having the alerts just appear/disappear -- no "sliding" in fancy stuff yet
        }
    }

}
