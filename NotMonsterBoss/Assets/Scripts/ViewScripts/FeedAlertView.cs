using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeedAlertView : MonoBehaviour
{
    Text mAlertText;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void InitializeAlert(RectTransform parent_transform, int numAlerts)
    {
        RectTransform newRect = GetComponent<RectTransform>();
        newRect.SetParent(parent_transform, false);
        newRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 1.0f, parent_transform.rect.width);
        newRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 1.0f, parent_transform.rect.height);
        //newRect.position = new Vector2(newRect.position.x, newRect.position.y - (parent_transform.rect.height / 8) * (numAlerts) * newRect.lossyScale.y);

        mAlertText = this.transform.FindChild("Text").GetComponent<Text>();
        mAlertText.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 10.0f, parent_transform.rect.width);
    }

    public void UpdateAlertText(string new_text)
    {
        mAlertText.text = new_text;
    }



}
