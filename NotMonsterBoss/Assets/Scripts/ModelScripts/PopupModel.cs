using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * The base model for Popups
 */

public class PopupModel : MonoBehaviour
{
    public delegate void dPopupCallback();

    public enum ePopupType
    {
        ePopupType_DEFAULT, //  popup with text

        ePopupType_COUNT
    }

    public struct sPopupInfos
    {
        public string title;
        public string content;

        public dPopupCallback callback1;
        public dPopupCallback callback2;

    }

    //  TODO aherrera : POPUP INFOS


	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void InitializePopup()
    {
        OnPopupInitialize();
    }

    protected void OnPopupInitialize()
    {

    }

    


}
