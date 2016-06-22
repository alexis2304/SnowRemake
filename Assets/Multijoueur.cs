using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using BestHTTP;
using BestHTTP.SocketIO;

using UnityEngine.UI;

public class Multijoueur : MonoBehaviour {

	// Notre socket
	public SocketManager manager = new SocketManager (new Uri ("http://192.168.1.22:8080/socket.io/"));

	public string name = "";

	public string sendingPlayerName = "";

	public GameObject playerPrefab;
	public Transform ActualPlayer;

	public int PlayerInThisSession = 1;

	public bool AsRecevedLevel = false;
	public string LevelProceduralData = "";

	public RectTransform DeadTrans;

	public bool IsDead = false;

	public Vector3 PositionToLerp = Vector3.zero;

	public Transform Second_Player;

	public Server_Action _action;

	// Use this for initialization
	void Start () {

		_action = new Server_Action ();

		manager.Socket.On ("Reception", Reception);
		manager.Socket.On ("Utility", Utility);

		ActualPlayer.GetComponent<PlayerSystem> ().DeadMenu = DeadTrans;
		//CreateServer ("Alexis");
	}
	
	// Update is called once per frame
	void Update () {
		if (Second_Player != null && PositionToLerp != null) {
			Second_Player.position = Vector3.Lerp (Second_Player.position, PositionToLerp, 10.0f * Time.deltaTime);
		}
	}

	void OnGUI(){
		/*
		name 				= GUILayout.TextField (name, GUILayout.Width (300), GUILayout.Height(100));
		sendingPlayerName 	= GUILayout.TextField (sendingPlayerName, GUILayout.Width (300), GUILayout.Height(100));

		if (GUILayout.Button ("Join", GUILayout.Height(100))) {
			manager.Socket.Emit ("Emmission", name, sendingPlayerName, JsonUtility.ToJson (ActualPlayer.position), "");
		}

		if (GUILayout.Button ("Create", GUILayout.Height(100))) {

		}
		*/

	}

	public void Join(){
		manager.Socket.Emit ("Emmission", name, sendingPlayerName, JsonUtility.ToJson (ActualPlayer.position), "");
	}

	public void Dead(){
		IsDead = true;

		manager.Socket.Emit ("Utilitaire", name, sendingPlayerName, "Dead");

	}

	public void CreateServer(string name){
		
	}



	public void Reception(Socket socket, Packet packet, params object[] args){
		string SenderPlayerName			= (string)args[0];
		string ReceptionPlayerName		= (string)args[1];
		Vector3 SenderPlayerPosition	= JsonUtility.FromJson<Vector3>((string)args[2]);
		string Action					= (string)args[3];

		if (ReceptionPlayerName != name)
			return;

		GameObject SecondPlayer = GameObject.Find (SenderPlayerName);

		if (SecondPlayer == null) {
			if(ActualPlayer != null)
				ActualPlayer.GetComponent<PlayerSystem> ().enabled = true;
			SecondPlayer = (GameObject)Instantiate (playerPrefab, SenderPlayerPosition, Quaternion.identity);
			Second_Player = SecondPlayer.transform;
			SecondPlayer.name = SenderPlayerName;
			PlayerInThisSession++;
			GetComponent<MenuMultiplayerButton> ().Hide ();
		}

		if(ActualPlayer != null)
			ActualPlayer.GetComponent<PlayerSystem> ().isMobile = true;

		sendingPlayerName = SenderPlayerName;

		PositionToLerp = SenderPlayerPosition;

		/*if (Action != null) {
			Server_Action _ServAction = JsonUtility.FromJson<Server_Action> (Action);
			if(_ServAction.isFume)
				Second_Player.GetComponent<PlayerSystem> ().PlayFume ();
			else
				Second_Player.GetComponent<PlayerSystem> ().StopFume();

			if (!_ServAction.isActivateTrail)
				Second_Player.GetComponent<TrailRenderer> ().Clear ();
		}*/

		//string action = JsonUtility.ToJson (_action);

		manager.Socket.Emit ("Emmission", name, SenderPlayerName, JsonUtility.ToJson (ActualPlayer.position),"");

		if(!ActualPlayer.gameObject.GetComponent<PlayerSystem> ().IsLocal)
			ActualPlayer.gameObject.GetComponent<PlayerSystem> ().IsLocal = true;
		
		if (GetComponent<CameraFollow> ().Target == null)
			GetComponent<CameraFollow> ().Target = ActualPlayer;

		if(AsRecevedLevel == false)
			manager.Socket.Emit ("Utilitaire", name, SenderPlayerName, ProceduralSystem.GenerateLevelString(1000));

	}

	public void Utility(Socket socket, Packet packet, params object[] args){
		string SenderPlayerName			= (string)args[0];
		string ReceptionPlayerName		= (string)args[1];
		string Message 					= (string)args [2];

		if (ReceptionPlayerName != name)
			return;

		if (!AsRecevedLevel) {
			manager.Socket.Emit ("Utilitaire", name, SenderPlayerName, Message);
			LevelProceduralData = Message;
			ActualPlayer.GetComponent<LodSystem> ().GenerateLevelViewString (LevelProceduralData);
			LevelProceduralData = null;
			AsRecevedLevel = true;
		}

		if (Message == "Dead") {
			manager.Close ();
			if (!IsDead) {
				DeadTrans.localPosition = new Vector3 (0, DeadTrans.localPosition.y, DeadTrans.localPosition.z);
				DeadTrans.GetChild (2).GetComponent<Text> ().text = "WIN";
				DeadTrans.GetChild (0).transform.GetChild (0).GetComponent<Text> ().text = PlayerPrefs.GetInt ("Point", 0).ToString();
				ActualPlayer.GetComponent<PlayerSystem> ().enabled = false;
			}
		}

	}
}

[Serializable]
public class Server_Action{
	public bool isFume = false;
	public bool isActivateTrail = true;
}