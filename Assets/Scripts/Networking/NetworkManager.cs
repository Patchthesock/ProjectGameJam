using UnityEngine;
using System.Collections;
[RequireComponent(typeof(PlayControl))]
[RequireComponent(typeof(ViewControl))]

public class NetworkManager : MonoBehaviour {

	public bool isViewing;
	private RoomInfo[] roomsList;
	private RoomOptions roomOptions;
	private string roomName;
	private bool displayRooms = true;
	
	void Awake ()
	{
		// Connect to Photon
		PhotonNetwork.ConnectUsingSettings("1");

		// Set up room requirements.
		roomName = SystemInfo.deviceUniqueIdentifier;
		roomOptions = new RoomOptions() { isVisible = true, isOpen = true, 	maxPlayers = 3 };

		// Enable Play control for players.
		if(!isViewing)
		{
			this.gameObject.GetComponent<PlayControl>().enabled = true;
			this.gameObject.GetComponent<ViewControl>().enabled = false;
		}
		else
		{
			this.gameObject.GetComponent<PlayControl>().enabled = false;
			this.gameObject.GetComponent<ViewControl>().enabled = true;
		}
	}
	
	void OnJoinedLobby ()
	{
		if(!isViewing)
			PhotonNetwork.JoinRandomRoom();
	}
	
	void OnPhotonRandomJoinFailed ()
	{
		PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default); 
	}
	
	void OnJoinedRoom ()
	{	
		displayRooms = false;
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		int i = 0;
		int e = 0;

		foreach(GameObject player in players)
		{

			if(player.GetComponent<NetworkManager>().isViewing == false)
				i++;

		}

		if((i >= 2 && isViewing == false))
		{
			PhotonNetwork.LeaveRoom();
			PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
		}

		Debug.Log("On Scene");
	}

	void SpawnPlayer()
	{
		// Only for actually spawning
		PhotonNetwork.Instantiate("Player",
		                          GameObject.FindGameObjectWithTag("PlayerSpawnPoint").transform.position,
		                          GameObject.FindGameObjectWithTag("PlayerSpawnPoint").transform.rotation,
		                          0
		                          );
	}

	void OnReceivedRoomListUpdate()
	{
		roomsList = PhotonNetwork.GetRoomList();
	}

	void OnGUI()
	{
		if (!PhotonNetwork.connected)
		{
			GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
		}
			
		// Join Room
		if (roomsList != null && isViewing && displayRooms)
		{
			for (int i = 0; i < roomsList.Length; i++)
			{
				if (GUI.Button(new Rect(100, 250 + (110 * i), 350, 100), "Join " + roomsList[i].name))
					PhotonNetwork.JoinRoom(roomsList[i].name);
			}
		}
	}
}
