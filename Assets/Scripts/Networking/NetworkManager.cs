using UnityEngine;
using System.Collections;
using PhotonHashTable = ExitGames.Client.Photon.Hashtable;

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
		// Connect to Photon Network
		PhotonNetwork.ConnectUsingSettings("1.1");

		// Set up room requirements.
		roomName = SystemInfo.deviceUniqueIdentifier;
		roomOptions = new RoomOptions() { isVisible = true, isOpen = true, 	maxPlayers = 3 };

		PhotonHashTable playerHash;

		// Enable Play control for players.
		if(!isViewing)
		{
			this.gameObject.GetComponent<PlayControl>().enabled = true;
			this.gameObject.GetComponent<ViewControl>().enabled = false;
			playerHash = new PhotonHashTable { { "Viewer", false } };
		}
		else
		{
			this.gameObject.GetComponent<PlayControl>().enabled = false;
			this.gameObject.GetComponent<ViewControl>().enabled = true;
			playerHash = new PhotonHashTable { { "Viewer", true } };
		}
		
		// Set Player Prefs.
		PhotonNetwork.SetPlayerCustomProperties(playerHash);
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
		SpawnPlayer ();
	}

	void OnPhotonPlayerConnected(PhotonPlayer other)
	{
		if(PhotonNetwork.isMasterClient)
		{	
			PhotonPlayer[] players = PhotonNetwork.playerList;
			int i = 0;

			foreach(PhotonPlayer player in players)
			{
				PhotonHashTable playerHashTable = player.customProperties;
				bool isWatching = (bool)playerHashTable["Viewer"];

				if(!isWatching)
				{
					i++;
				}
			}
			
			// If more than 2 players or more than one view leave current room.
			if((i > 2 && this.isViewing == false))
			{
				PhotonNetwork.CloseConnection(other);
			}
			
			Debug.Log("Players Playing: " + i);
		}
	}
	
	void SpawnPlayer ()
	{
		// Only for actually spawning
		PhotonNetwork.Instantiate("Player", new Vector3(0,0,0), Quaternion.identity, 0);
	}

	void OnReceivedRoomListUpdate()
	{
		roomsList = PhotonNetwork.GetRoomList();
	}

	void OnPhotonDissconnected ()
	{
		PhotonNetwork.ConnectUsingSettings("1");
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
