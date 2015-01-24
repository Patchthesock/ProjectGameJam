using UnityEngine;
using System.Collections;

public class PlayControl : MonoBehaviour {

	public bool isPlaying = false;
	public bool isReady = false;

	public GameObject player;

	void Awake ()
	{
		InvokeRepeating("FindPlayer", 0, 0.5f);
	}

	void RoundControl()
	{

	}

	void FindPlayer()
	{
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		foreach(GameObject myPlayer in players)
		{
			if(myPlayer.GetComponent<PhotonView>().isMine)
				this.player = myPlayer;
		}
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{

		}
		else 
		{	

		}
	}
}
