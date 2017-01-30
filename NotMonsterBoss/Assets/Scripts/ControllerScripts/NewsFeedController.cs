using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NewsFeedController : MonoBehaviour
{ 
    public static NewsFeedController instance = null;

    private NewsFeedModel mModel;
    private NewsFeedView mView;

    //  TODO aherrera : replace this with a reference to Resources.?
    public GameObject _AlertFeedPrefab;

    //  TODO aherrera : does this reference to the container object need to be in Controller, or Model?
    protected GameObject mAlertsContainer;
    public Transform _ShowFeedPoint;
    public Transform _HideFeedPoint;

    public GameObject _MainCanvas;

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


    }

	// Use this for initialization
	void Start ()
    {
        mView.initialize (_MainCanvas);


        mAlertsContainer = new GameObject ("Alerts Container", typeof (RectTransform));
        mAlertsContainer.transform.SetParent (this.transform);

        mView.initializeAlertContainer (mAlertsContainer);
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

        FeedAlertModel alert_model = new_alert_go.AddComponent<FeedAlertModel>();
        FeedAlertView alert_view = new_alert_go.AddComponent<FeedAlertView>();

        alert_view.InitializeAlert(mAlertsContainer.GetComponent<RectTransform>(), mModel.AlertsList.Count);
        alert_view.UpdateAlertText(message);

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
