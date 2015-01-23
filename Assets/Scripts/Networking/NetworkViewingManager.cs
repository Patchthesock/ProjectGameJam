using UnityEngine;
using System.Collections;

public class ViewingNetworkManager : MonoBehaviour {

	void Awake ()
	{
		if(!PhotonNetwork.connected)
		{
			// Connect using version number.
			PhotonNetwork.ConnectUsingSettings("1");
			PhotonNetwork.automaticallySyncScene = true;
		}
	}

	void OnJoinedLobby ()
	{
		PhotonNetwork.JoinRandomRoom();
	}

	void OnPhotonRandomJoinFailed ()
	{
		RoomOptions roomOptions = new RoomOptions() { isVisible = true, isOpen = true, 	maxPlayers = 3 };
		PhotonNetwork.JoinOrCreateRoom( SystemInfo.deviceUniqueIdentifier, roomOptions, TypedLobby.Default); 
	}

	void OnJoinedRoom ()
	{
		Debug.Log ("I'm Online.");
	}
}
