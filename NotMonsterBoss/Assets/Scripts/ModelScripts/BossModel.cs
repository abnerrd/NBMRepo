using UnityEngine;
using System.Collections;

public class BossModel : UnitScript {

    [SerializeField]
    private 
        string m_name;
    public 
        string boss_name { get { return m_name; } set { m_name = value; } }


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
