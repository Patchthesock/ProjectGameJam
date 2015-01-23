using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	public bool isViewing = true;

	private RoomOptions roomOptions;
	
	void Awake ()
	{
		if(!PhotonNetwork.connected)
		{
			// Connect using version number.
			PhotonNetwork.ConnectUsingSettings("1");
		}

		// Set up room requirements. 
		roomOptions = new RoomOptions() { isVisible = true, isOpen = true, 	maxPlayers = 3 };
	}
	
	void OnJoinedLobby ()
	{
		PhotonNetwork.JoinRandomRoom();
	}
	
	void OnPhotonRandomJoinFailed ()
	{

		PhotonNetwork.JoinOrCreateRoom( SystemInfo.deviceUniqueIdentifier, roomOptions, TypedLobby.Default); 
	}
	
	void OnJoinedRoom ()
	{
		GameObject[] players = GameObject.FindGameObjectsWithTag("NetworkManager");
		if(players.Length > 2 && isViewing == false)
		{
			PhotonNetwork.LeaveRoom();
			PhotonNetwork.CreateRoom( SystemInfo.deviceUniqueIdentifier, roomOptions, TypedLobby.Default);
		}
	}
}
