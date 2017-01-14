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

    public Button CloseButton;
    public Button OkButton;
    public Text Content;
    public Text Title;

    public void SetEnabled(bool enabled)
    {
        mCurrentSprite.enabled = enabled;
        CloseButton.enabled = enabled;
        OkButton.enabled = enabled;
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

    }

    public void HidePopup()
    {

    }

    public void OnCloseButton()
    {
        //  TODO aherrera : callback to Controller method to tell Model method that SOMETHING just happened.
        //                  and then the MODEL has ANOTHER callback to that is already assigned.
    }

    public void OnOkButton()
    {
        //  TODO aherrera : callback to Controller method to tell Model method that SOMETHING just happened.
        //                  and then the MODEL has ANOTHER callback to that is already assigned.
    }


    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
