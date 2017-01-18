using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class resembles a sort of "Popup Manager".
 * But I wouldn't think of it as a singleton -- more of a Regional Manager.
 */

public class PopupController : MonoBehaviour
{
    //  TODO aherrera : prefabs vs just getting popups from /Assets/Resources/? (Resources.)
    public GameObject PopupDefaultPrefab;

    [SerializeField]
    protected List<PopupModel> mPopupList;

    protected GameObject mPopupGO;

	// Use this for initialization
	void Start ()
    {
        mPopupList = new List<PopupModel>();

        if(mPopupGO == null)
        {
            mPopupGO = new GameObject();
            mPopupGO.transform.SetParent(this.gameObject.transform);
        }
        mPopupGO.name = "Popup Root";
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    //  TODO aherrera : second param: ePopupType?
    public void AddPopup(PopupModel.sPopupInfos popup_infos)
    {
        //  TODO aherrera : create a PopupModel?

        
    }

    public void AddPopup(PopupModel new_popup)
    {
        new_popup.InitializePopup();
        mPopupList.Add(new_popup);

        //  TODO aherrera, wspier : Add Popup Views/GO(?) onto mPopupGO
        

        UpdatePopupInput();
    }

    public void CloseTopPopup()
    {
        PopupModel popup_to_close = GetTopPopup();

        //  TODO aherrera : close Popup

        //  TODO aherrera : remove from list (destroy gameobject too?)

    }

    protected PopupModel GetTopPopup()
    {
        if(mPopupList.Count > 0)
        {
            return mPopupList[mPopupList.Count - 1];
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Purpose of this function is to turn off/on the Input for the Popups depending on whose on top; prevents clicking between popups
    /// </summary>
    protected void UpdatePopupInput()
    {
        //  TODO aherrera : iterate through Popups and turn off/on their Input
    }

}
