using UnityEngine;
using System.Collections;

public class SecondPlayerWait : MonoBehaviour {

	private NetworkManager NM;

	void Start () {
		NM = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if(NM.startGame)
			this.gameObject.SetActive(false);
	}
}
