using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class resembles a Popup Manager.
 */

public class PopupController : MonoBehaviour
{
    public static PopupController instance = null;

    //  TODO aherrera : replace this with a system to read in Resources.
    public GameObject PopupDefaultPrefab;
    
    //  TODO aherrera : refactor this list not in the Controller
    [SerializeField]
    protected List<GameObject> mPopupList;

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
        mPopupList = new List<GameObject>();

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

        PopupView view = new_popup_go.GetComponent<PopupView>();
        if (view == null)
        {
            view = new_popup_go.gameObject.AddComponent<PopupView>();
        }

        //  TODO aherrera : IF you're gonna do enums and default popups, use popup_infos to setup the popup HERE
        //                      probably to the popup_model

        AddPopup(new_popup_go);

        popup_model.InitializePopup();
        //  view.Initialize?

        UpdatePopupInput();
    }

    ///// <summary>
    ///// Add a popup to the Screen
    ///// </summary>
    ///// <param name="new_popup"></param>
    //public void AddPopup(GameObject new_popup)
    //{
    //    PopupModel popup_model = new_popup.GetComponent<PopupModel>();
    //    if (popup_model == null)
    //    {
    //        popup_model = new_popup.AddComponent<PopupModel>();
    //    }

    //    AddPopup(popup_model);
    //}

    /// <summary>
    /// Add a popup to the screen. Aligns to Parent GameObject
    /// </summary>
    /// <param name="new_popup"></param>
    protected void AddPopup(GameObject new_popup)
    {
        mPopupList.Add(new_popup);
        new_popup.transform.SetParent(mPopupGO.transform);
        new_popup.transform.localPosition = Vector3.zero;
    }

    public void CloseTopPopup()
    {
        GameObject popup_to_close = GetTopPopup();
        ClosePopup(popup_to_close);
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

    public void ClosePopup(GameObject popup_gameobject)
    {
        if (mPopupList.Contains(popup_gameobject))
        {
            mPopupList.Remove(popup_gameobject);

            //  RISK :: Does destroying the gameobject given as a PARAMETER work? Or should we get the reference from mPopupList?
            DestroyObject(popup_gameobject);

            UpdatePopupInput();
        }
    }

}
