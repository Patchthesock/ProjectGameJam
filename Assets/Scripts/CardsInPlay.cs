using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardsInPlay : MonoBehaviour 
{
	public bool turnEnded;
	public List<string> cardsInPlay;
	public List<string> cardsOnTable;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	[RPC]
	void CardsInPlayEmpty ()
	{
		cardsInPlay.Clear();
		turnEnded = false;
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext(turnEnded);
			string[] cards = cardsInPlay.ToArray();
			stream.SendNext(cards);

			cards = cardsOnTable.ToArray();
			stream.SendNext(cards);
		}
		else 
		{	
			turnEnded = (bool)stream.ReceiveNext();
			string[] cards = (string[])stream.ReceiveNext();
			cardsInPlay = new List<string>(cards);

			cards = (string[])stream.ReceiveNext();
			cardsOnTable = new List<string>(cards);
		}
	}
}
