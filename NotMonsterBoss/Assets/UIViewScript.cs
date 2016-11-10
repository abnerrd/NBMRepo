using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
 *  The purpose of this script is to handle the output variables of the UI.
 *  
 *  It will contain references to almost EVERY UI element on the screen.
 *  
 *  The only class that should access this script should be UIModelScript.
 *  
 */


public class UIViewScript : MonoBehaviour
{
    public static UIViewScript instance = null;


    public Button _AddRoomButton;
    public void enable_AddRoomButton(bool enabled)
    {
        if(_AddRoomButton != null)
        {
            _AddRoomButton.gameObject.SetActive(enabled);
        }
    }

    public Button _AddAdventurerButton;
    public void enable_AddAdventurerButton(bool enabled)
    {
        if (_AddAdventurerButton != null)
        {
            _AddAdventurerButton.gameObject.SetActive(enabled);
        }
    }

    public Button _RemoveAllAdventurersButton;
    public void enable_RemoveAllAdventurersButton(bool enabled)
    {
        if(_RemoveAllAdventurersButton != null)
        {
            _RemoveAllAdventurersButton.gameObject.SetActive(enabled);
        }
    }


    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            DestroyObject(this.gameObject);
        }
    }

	// Use this for initialization
	void Start ()
    {
        InitializeUI();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    void InitializeUI()
    {
        enable_AddAdventurerButton(false);
        enable_AddRoomButton(true);
    }


}
