using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FootingUIControl : MonoBehaviour {

	public GameObject player; 
	
	private Slider mySlider;

	void Start () {
		mySlider = this.GetComponent<Slider>();
	}
	
	// Update is called once per frame
	void Update () {
		if(player)
			mySlider.value = player.GetComponent<Player>().GetPosition();
	}
}
