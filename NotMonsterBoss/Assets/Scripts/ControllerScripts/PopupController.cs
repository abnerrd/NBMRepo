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

    protected List<PopupModel> mPopupList;


	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    //  TODO aherrera : popup information?
    public PopupModel ShowPopup(/*string name? GameObject? enum?*/)
    {
        return null;
    }

    //  TODO aherrera : setup "dictionary" list, like DungeonModel?
    //public PopupModel ClosePopup(PopupModel popup_to_close)
    //{

    //}

}
