using UnityEngine;
using System.Collections;

public class DebugScript : MonoBehaviour
{
    public static DebugScript instance = null;

    public bool _enableDebugs = true;

    public GameObject _RoomPrefab;
    public GameObject _AdventurerPrefab;

    public AdventurerGenerator _AdventurerGen;

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
                DungeonManager.instance.addRoom(GameObject.Instantiate(_RoomPrefab).GetComponent<RoomScript>());
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                //DungeonManager.instance.enterDungeon(GameObject.Instantiate(_AdventurerPrefab).GetComponent<AdventurerScript>());
                //DungeonManager.instance.enterDungeon (_AdventurerGen.GenerateByName ("Billy").GetComponent<AdventurerScript> ());
                DungeonManager.instance.enterDungeon (_AdventurerGen.GenerateUnique ());
            }
        }
    }
}
