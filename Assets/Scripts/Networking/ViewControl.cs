using UnityEngine;
using System.Collections;

public class ViewControl : MonoBehaviour {

	private GameObject[] players;

	void Start () {
		InvokeRepeating( "LookForPlayers", 0, 5 );
	}

	void Update ()
	{
		foreach(GameObject player in players)
		{
			// do something
		}
	}

	void LookForPlayers ()
	{
		players = GameObject.FindGameObjectsWithTag("Player");
	}
}
