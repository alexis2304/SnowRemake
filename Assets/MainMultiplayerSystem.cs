using UnityEngine;
using System.Collections;

public class MainMultiplayerSystem : MonoBehaviour {
	// La UI ou et afficher le counter etc...
	[HideInInspector]
	public GameObject CounterUI;
	public RectTransform DeadUI;
	// Use this for initialization
	void Start () {
		CounterUI = GameObject.Find ("StartedUI");
	}

	public void HideUI(){
		CounterUI.GetComponent<RectTransform> ().localPosition = new Vector3 (800, 0, 0);
		StartGame ();
	}

	public void StartGame(){
		PlayerSystem playerSystem = GetComponent<Multijoueur2> ().TransformOfMe.GetComponent<PlayerSystem> ();
		playerSystem.transform.GetChild (3).GetComponent<Checker> ().pointSystem = Camera.main.GetComponent<PointSystem> ();
		playerSystem.DeadMenu = DeadUI;
		#if UNITY_EDITOR
			playerSystem.isMobile = false;
		#else
			playerSystem.isMobile = true;
		#endif
		playerSystem.IsLocal = true;
		playerSystem.EmitParticle = true;
		Camera.main.GetComponent<CameraFollow> ().Target = GetComponent<Multijoueur2> ().TransformOfMe;
		playerSystem.IsMultiplayer = true;
		playerSystem.enabled = true;
	}

	public void DisconnectGame(){
		GetComponent<Multijoueur2> ().Disconnect ();
	}
}
