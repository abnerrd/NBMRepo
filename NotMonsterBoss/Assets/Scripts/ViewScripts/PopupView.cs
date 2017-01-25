using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupView : MonoBehaviour
{
    //  TODO aherrera : create base "View" class for similar variables?
    private bool mEnabled;
    public Image mDefaultSprite;
    private Image mCurrentSprite;
    private RectTransform mTransform;

    public Button _CloseButton;
    public Button _OkButton;
    public Text _Content;
    public Text _Title;

    public void SetEnabled(bool enabled)
    {
 //       mCurrentSprite.enabled = enabled;
        _CloseButton.enabled = enabled;
        _OkButton.enabled = enabled;
    }

    public void SetRectPosition(Vector2 pos)
    {
        mTransform.position = pos;
    }
    public Vector2 GetRectPosition()
    {
        return new Vector2(mTransform.position.x, mTransform.position.y);
    }

    public void ShowPopup()
    {
        SetEnabled(true);
    }

    /// <summary>
    /// Do any "closing" items you want to take care of here.
    /// Does NOT destroy Gameobject. PopupController takes care of that.
    /// </summary>
    public void ClosePopup()
    {
        SetEnabled(false);
    }

    public void OnCloseButton()
    {
        //  TODO aherrera : callback to Controller method to tell Model method that SOMETHING just happened.
        //                  and then the MODEL has ANOTHER callback to that is already assigned.
        ClosePopup();
    }

    public void OnOkButton()
    {
        //  TODO aherrera : callback to Controller method to tell Model method that SOMETHING just happened.
        //                  and then the MODEL has ANOTHER callback to that is already assigned.
                    //      callback or Event?
        ClosePopup();
    }

    /// <summary>
    /// Turn on/off ANY item in the Popup that can send an input
    /// </summary>
    /// <param name="input_on"></param>
    public void ToggleInput(bool input_on)
    {
        _OkButton.interactable = input_on;
        _CloseButton.interactable = input_on;
    }


    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
