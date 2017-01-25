using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class resembles a Popup Manager.
 */

public class PopupController : MonoBehaviour
{
    public static PopupController instance = null;

    //  TODO aherrera : prefabs vs just getting popups from /Assets/Resources/? (Resources.)
    public GameObject PopupDefaultPrefab;

    [SerializeField]
    protected List<PopupModel> mPopupList;

    protected GameObject mPopupGO;

    public GameObject MainCanvas;

    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            DestroyObject(this.gameObject);
        }
    }

    // Use this for initialization
    void Start ()
    {
        mPopupList = new List<PopupModel>();

        if(mPopupGO == null)
        {
            mPopupGO = new GameObject();
            mPopupGO.transform.SetParent(MainCanvas.transform);
        }
        mPopupGO.transform.localPosition = Vector3.zero;
        mPopupGO.name = "Popup Root";
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    //  TODO aherrera : second param: ePopupType?
    public void AddPopup(PopupModel.sPopupInfos popup_infos)
    {
        GameObject new_popup_go = Instantiate(PopupDefaultPrefab);
        PopupModel popup_model = new_popup_go.GetComponent<PopupModel>();
        if (popup_model == null)
        {
            popup_model = new_popup_go.AddComponent<PopupModel>();
        }

        //  TODO aherrera : IF you're gonna do enums and default popups, use popup_infos to setup the popup HERE
        //                      probably to the popup_model

        AddPopup(popup_model);
    }

    /// <summary>
    /// Add a popup to the Screen
    /// </summary>
    /// <param name="new_popup"></param>
    public void AddPopup(GameObject new_popup)
    {
        PopupModel popup_model = new_popup.GetComponent<PopupModel>();
        if (popup_model == null)
        {
            popup_model = new_popup.AddComponent<PopupModel>();
        }

        AddPopup(popup_model);
    }

    /// <summary>
    /// Add a popup to the screen
    /// </summary>
    /// <param name="new_popup"></param>
    protected void AddPopup(PopupModel new_popup)
    {
        PopupView view = new_popup.GetComponent<PopupView>();
        if(view == null)
        {
            view = new_popup.gameObject.AddComponent<PopupView>();
        }

        new_popup.InitializePopup();
        mPopupList.Add(new_popup);
        new_popup.transform.SetParent(mPopupGO.transform);
        new_popup.transform.localPosition = Vector3.zero;

        //  TODO aherrera : view.initialize()?
        

        UpdatePopupInput();
    }

    public void CloseTopPopup()
    {
        GameObject popup_to_close = GetTopPopup();
        //  TODO aherrera : at this point they should still have this components, buuuuut error check for if they don't?
        PopupModel model = popup_to_close.GetComponent<PopupModel>();
        PopupView view = popup_to_close.GetComponent<PopupView>();
        
        view.ClosePopup();

        mPopupList.Remove(model);
        DestroyObject(popup_to_close);

        UpdatePopupInput();
    }

    protected GameObject GetTopPopup()
    {
        if(mPopupList.Count > 0)
        {
            return mPopupList[mPopupList.Count - 1].gameObject;
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
