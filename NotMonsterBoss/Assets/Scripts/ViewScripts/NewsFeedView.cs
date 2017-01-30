using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewsFeedView : MonoBehaviour
{
    private RectTransform mTransform;
    private bool mShowAlerts = false;
    public GameObject _AlertButton;
    public bool ShowAlerts { get { return mShowAlerts; } }


    void Awake ()
    {
        mTransform = gameObject.GetComponent<RectTransform> ();
    }

    // Use this for initialization
    public void initialize (GameObject mainCanvas)
    {
        //Scale transform the the main canvas
        RectTransform mainRect = mainCanvas.GetComponent<RectTransform> ();
        mTransform.SetInsetAndSizeFromParentEdge (RectTransform.Edge.Left, 1.0f, mainRect.rect.width);
        mTransform.SetInsetAndSizeFromParentEdge (RectTransform.Edge.Bottom, 1.0f, (mainRect.rect.height / 8));

        //Position Alert Button
        RectTransform alertRect = _AlertButton.GetComponent<RectTransform> ();
        alertRect.SetInsetAndSizeFromParentEdge (RectTransform.Edge.Right, alertRect.rect.width / 2, alertRect.rect.width);
        alertRect.SetInsetAndSizeFromParentEdge (RectTransform.Edge.Bottom, alertRect.rect.height / 2, alertRect.rect.height);
    }

    public void initializeAlertContainer (GameObject alertCotnainer)
    {
        RectTransform containerRect = alertCotnainer.GetComponent<RectTransform> ();
        RectTransform feedRect = gameObject.GetComponent<RectTransform> ();
        containerRect.SetInsetAndSizeFromParentEdge (RectTransform.Edge.Top, -feedRect.rect.height, (feedRect.rect.height * 0.75f));
        containerRect.SetInsetAndSizeFromParentEdge (RectTransform.Edge.Left, 0.0f, (feedRect.rect.width));
    }
}
