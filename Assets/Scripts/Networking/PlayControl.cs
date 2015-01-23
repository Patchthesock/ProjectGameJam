using UnityEngine;
using System.Collections;

public class PlayControl : MonoBehaviour {

	public bool isPlaying = false;
	public bool isReady = false;

	void Awake ()
	{

	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext(isPlaying);
		}
		else 
		{	
			isPlaying = (bool)stream.ReceiveNext();
		}
	}
}
