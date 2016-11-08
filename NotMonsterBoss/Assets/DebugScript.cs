using UnityEngine;
using System.Collections;

public class DebugScript : MonoBehaviour
{
    public static DebugScript instance = null;

    public bool _enableDebugs = true;

    public GameObject _RoomPrefab;
    public GameObject _AdventurerPrefab;

    public AdventurerGenerator _AdventurerGen;
    public RoomGenerator _RoomGen;

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
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(_enableDebugs)
        {
            if (Input.GetKeyUp(KeyCode.N))
            {
                DungeonManager.instance.addRoom (_RoomGen.GenerateUnique ());
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                DungeonManager.instance.enterDungeon (_AdventurerGen.GenerateUnique ());
            }
        }
    }
}
