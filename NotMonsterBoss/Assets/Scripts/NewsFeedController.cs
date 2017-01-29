using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class NewsFeedController : MonoBehaviour
{ 
    public static NewsFeedController instance = null;

    public NewsFeedModel mModel;
    public NewsFeedView mView;

    //  TODO aherrera : replace this with a reference to Resources.?
    public GameObject _AlertFeedPrefab;

    //  TODO aherrera : does this reference to the container object need to be in Controller, or Model?
    protected GameObject mAlertsContainer;
    public Transform _ShowFeedPoint;
    public Transform _HideFeedPoint;

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

        mView = this.gameObject.GetComponent<NewsFeedView>();
        if (mView == null)
        {
            mView = this.gameObject.AddComponent<NewsFeedView>();
        }

        mModel = this.gameObject.GetComponent<NewsFeedModel>();
        if (mModel == null)
        {
            mModel = this.gameObject.AddComponent<NewsFeedModel>();
        }

        if (mAlertsContainer == null)
        {
            mAlertsContainer = new GameObject("Alerts Container", typeof(RectTransform));
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

    public void CreateNewAlert(string message)
    {
        //  TODO aherrera : Create "AlertModel", view, controller? For more control over what we can do with these alerts? Like buttons, callbacks, etc.?
        //                  It'll provide some headache relief to getting the Text in following way
        GameObject new_alert_go = Instantiate(_AlertFeedPrefab);
        Text alert_message = new_alert_go.transform.FindChild("Text").GetComponent<Text>();
        alert_message.text = message;

        mModel.AddNewAlert(new_alert_go);
        new_alert_go.transform.SetParent(mAlertsContainer.transform);
        mModel.PositionAlerts();
    }

    protected GameObject GetNewestAlert()
    {
        if(mModel.AlertsList.Count > 0)
        {
            return mModel.AlertsList[mModel.AlertsList.Count - 1];
        }
        else
        {
            return null;
        }
    }

    protected GameObject GetOldestAlert()
    {
        return mModel.AlertsList[0];
    }

    public void ToggleAlerts()
    {
        mModel.SetShowAlerts(!mModel.ShowAlerts);
    }

    public void ShowAlerts(bool show_alerts)
    {
        NewsFeedModel model = this.GetComponent<NewsFeedModel>();
        if(model != null)
        {
            model.SetShowAlerts(show_alerts);
        }
        else
        {
            Debug.LogError("NewsFeedController::ShowAlerts -- GameObject does not have \"Model\" reference!");
        }

        if (show_alerts)
        {
            //  Position List accordingly
                //  For now, i'm having the alerts just appear/disappear -- no "sliding" in fancy stuff yet
        }
    }

}
