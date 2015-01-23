using UnityEngine;
using System.Collections;
[RequireComponent(typeof(PlayControl))]

public class NetworkManager : MonoBehaviour {

	public bool isViewing = true;
	public string roomName = null;

	private RoomOptions roomOptions;
	
	void Awake ()
	{
		// Connect to Photon
		PhotonNetwork.ConnectUsingSettings("1");

		// Set up room requirements. 
		roomOptions = new RoomOptions() { isVisible = true, isOpen = true, 	maxPlayers = 3 };

		// Enable Play control for players.
		if(isViewing == false)
		{
			this.gameObject.GetComponent<PlayControl>().enabled = true;
		}
	}
	
	void OnJoinedLobby ()
	{
		PhotonNetwork.JoinRandomRoom();
	}
	
	void OnPhotonRandomJoinFailed ()
	{
		PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default); 
	}
	
	void OnJoinedRoom ()
	{
		GameObject[] players = GameObject.FindGameObjectsWithTag("NetworkManager");
		int i = 0;

		foreach(GameObject player in players)
		{
			if(player.GetComponent<NetworkManager>().isViewing == false)
				i++;
		}

		if(i >= 2 && isViewing == false)
		{
			PhotonNetwork.LeaveRoom();
			PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
		}
	}	
}
