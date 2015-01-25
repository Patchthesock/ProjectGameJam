using UnityEngine;
using System.Collections;
using PhotonHashTable = ExitGames.Client.Photon.Hashtable;

[RequireComponent(typeof(PlayControl))]
[RequireComponent(typeof(ViewControl))]


public class NetworkManager : MonoBehaviour {

	public bool isViewing;

	public bool startGame = false;
	private string connectionSettings = "1.1";
	private RoomInfo[] roomsList;
	private RoomOptions roomOptions;
	private string roomName;
	private bool displayRooms = true;
	
	void Awake ()
	{
		// Connect to Photon Network
		PhotonNetwork.ConnectUsingSettings(connectionSettings);

		// Set up room requirements.
		roomName = SystemInfo.deviceUniqueIdentifier;
		roomOptions = new RoomOptions() { isVisible = true, isOpen = true, 	maxPlayers = 3 };

		PhotonHashTable playerHash;

		// Enable Play control for players.
		// Or View control for non - players.
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

	void Start ()
	{
		
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

		CheckStartGame();
	}

	void OnPhotonPlayerConnected(PhotonPlayer other)
	{
		if(PhotonNetwork.isMasterClient)
		{	
			// If more than 2 players leave current room.
			if(PlayerCount() > 2 && this.isViewing == false)
			{
				PhotonNetwork.CloseConnection(other);
			}
		}

		CheckStartGame();
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
		PhotonNetwork.ConnectUsingSettings(connectionSettings);
	}

	void OnPhotonPlayerDisconnected ()
	{
		Application.LoadLevel("Winning");
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

	private void CheckStartGame()
	{
		if(PlayerCount() == 2)
			startGame = true;
	}

	private int PlayerCount()
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

		return i;
	}
}
