using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using BestHTTP;
using BestHTTP.SocketIO;

public class MultijoueurSys : MonoBehaviour {

	// Le socket manager
	public SocketManager manager = new SocketManager (new Uri ("http://192.168.0.8:8080/socket.io/"));

	// Le nom de la session
	public string sessionName = "";

	// L'identifiant unique du houeur
	public string playerId = "";

	public Transform player1;
	public Transform player2;

	// Quand la scene demarre...
	void Start(){

		// On innitialise l'id
		playerId = SystemInfo.deviceUniqueIdentifier;

		// On innitialise le nom de session...
		sessionName = SystemInfo.deviceUniqueIdentifier + "_Session";

		player1.name = playerId;

		// On ecoute le serveur
		manager.Socket.On("position", position);

	}

	void Update(){
		Debug.Log ("emit...");
		manager.Socket.Emit ("SendPosition", playerId, sessionName, JsonUtility.ToJson(player1.transform.position));
	}

	void position(Socket socket, Packet packet, params object[] args){
		string player_id 	= (string)args[0];
		string session_name = (string)args[1];
		Vector3 position 	= JsonUtility.FromJson<Vector3>((string)args[2]);

		if (session_name != sessionName)
			return;

		if (player_id != playerId)
			return;
		
		player1.transform.position = position;
	}

}