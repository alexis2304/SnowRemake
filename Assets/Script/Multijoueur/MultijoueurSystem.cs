using UnityEngine;
using System.Collections;

using BestHTTP;
using BestHTTP.SocketIO;

using System;

using System.Collections.Generic;

public class MultijoueurSystem : MonoBehaviour {

	public SocketManager manager = new SocketManager (new Uri ("http://192.168.0.8:8080/socket.io/"));
	string sessionName = "";

	public GameObject Player;

	GameObject player;
	List<GameObject> Other = new List<GameObject>();

	void OnGUI(){

		GUILayout.Label ("Session name : ");
		sessionName = GUILayout.TextField (sessionName);

		if (GUILayout.Button ("Crée une session")) {
			sessionName = SystemInfo.deviceUniqueIdentifier + "_Session";
			manager.Socket.Emit ("CreateSession", sessionName, 4, SystemInfo.deviceUniqueIdentifier, "0,0,0");
		}

		if (GUILayout.Button ("Rejoindre une session")) {
			sessionName = SystemInfo.deviceUniqueIdentifier + "_Session";
			manager.Socket.Emit ("JoinSession", SystemInfo.deviceUniqueIdentifier);
		}

		if (GUILayout.Button ("Quitter une session")) {
			sessionName = SystemInfo.deviceUniqueIdentifier + "_Session";
			manager.Socket.Emit ("QuitSession", sessionName, SystemInfo.deviceUniqueIdentifier);
		}

		if (GUILayout.Button ("Suprimmer la session")) {
			manager.Socket.Emit ("DeleteSession", sessionName);
		}

		if (GUILayout.Button ("Voir toutes les sessions")) {
			manager.Socket.Emit ("ShowSession");
		}

		if (GUILayout.Button ("Voir le nombre de joueur dans la session")) {
			manager.Socket.Emit ("GetSessionPlayerCount", sessionName);
		}

		if (GUILayout.Button ("Get Data")) {
			manager.Socket.Emit ("GetData", sessionName);
		}

		if (GUILayout.Button ("Set Data")) {
			manager.Socket.Emit ("SetData", sessionName, SystemInfo.deviceUniqueIdentifier, "0,1,0");
		}

		if (GUILayout.Button ("Disconnect")) {
			manager.Close ();
		}
	}

	void Start(){
		sessionName = SystemInfo.deviceUniqueIdentifier + "_Session";
		manager.Socket.On ("SessionPlayersPosition", SessionState);
		manager.Socket.On("SessionMessage", SessionMessage);

		manager.Socket.Emit ("JoinSession", SystemInfo.deviceUniqueIdentifier);

	}

	void SessionState(Socket socket, Packet packet, params object[] args){
		Debug.Log ("Session State Message : " + (string)args[0]);
	}

	void SessionMessage(Socket socket, Packet packet, params object[] args){
		string message = (string)args [0];

		if (message == "createSession") {
			// On crée une session
			manager.Socket.Emit ("CreateSession", sessionName, 4, SystemInfo.deviceUniqueIdentifier, ServerUtilities.Vector3ToString(new Vector3(1, 0, 0)));
		}

		if(player == null)
			player = (GameObject)Instantiate (Player, new Vector3(-2, 0, 0), Quaternion.identity);


	}

	void Update(){
		if (player != null) {
			manager.Socket.Emit ("SetData", sessionName, SystemInfo.deviceUniqueIdentifier, ServerUtilities.Vector3ToString (player.transform.position));
			manager.Socket.Emit ("GetData", sessionName);
		}
	}

}

public class ServerUtilities{
	public static string Vector3ToString(Vector3 vector){
		string retour = vector.x + "," + vector.y + "," + vector.z;
		return retour;
	}

	public static Vector3 StringToVector(string vector){
		string[] spliting = vector.Split (',');
		Vector3 vecteur = new Vector3(float.Parse(spliting[0]), float.Parse(spliting[1]), float.Parse(spliting[2]));
		return vecteur;
	}
}

[Serializable]
public class Code
{
	// Position x
	public float x;

	// Position y
	public float y;

	// Position z
	public float z;

	// Identification
	public string id;

	// Pour l'effet de neige
	public bool IsFume = false;
}