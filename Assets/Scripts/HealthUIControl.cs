using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthUIControl : MonoBehaviour {

	public GameObject player; 
	
	private Slider mySlider;

	void Start () {
		mySlider = this.GetComponent<Slider>();
	}
	
	// Update is called once per frame
	void Update () {
		if(player)
			mySlider.value = player.GetComponent<Player>().GetHealth();
	}
}
