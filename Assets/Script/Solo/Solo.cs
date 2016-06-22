using UnityEngine;
using System.Collections;

public class Solo : MonoBehaviour {

	public Transform player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartGame(){
		player.GetComponent<PlayerSystem> ().isMobile = false;
		#if UNITY_IOS || UNITY_ANDROID
		player.GetComponent<PlayerSystem> ().isMobile = true;
		#endif
		player.GetComponent<LodSystem> ().GenerateLevelViewString (ProceduralSystem.GenerateLevelString (1000));
		player.GetComponent<PlayerSystem> ().enabled = true;
	}
}
