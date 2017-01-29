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

    public void InitializeAlert(RectTransform parent_transform)
    {
        RectTransform newRect = GetComponent<RectTransform>();
        newRect.SetParent(parent_transform, false);
        newRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 1.0f, parent_transform.rect.width);
        newRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 1.0f, (parent_transform.rect.height / 8));
        //newRect.position = new Vector2(newRect.position.x, newRect.position.y - (mTransform.rect.height / 8) * (roomCount - 1) * newRect.lossyScale.y);

        mAlertText = this.transform.FindChild("Text").GetComponent<Text>();
        mAlertText.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 1.0f, parent_transform.rect.width);
        newRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 1.0f, (parent_transform.rect.height / 8));
    }

    public void UpdateAlertText(string new_text)
    {
        mAlertText.text = new_text;
    }



}
