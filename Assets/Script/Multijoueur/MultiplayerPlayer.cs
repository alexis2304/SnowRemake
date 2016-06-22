using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using BestHTTP;
using BestHTTP.SocketIO;


public class MultiplayerPlayer : MonoBehaviour {

	// Le Socket Manager (on se connecte au server)
	[HideInInspector]
	public SocketManager manager = new SocketManager (new Uri ("http://91.134.243.214:8080/socket.io/"));

	// La position a lerper
	Vector3 PositionToLerp = Vector3.zero;

	// L'identifiant sue le réseaux (on peut se servir de facebook)
	public string Identifier;

	// La vitesse de fluiditer (lerp)
	public float Speed = 1;

	public string state = "Not Connected";

	private bool particle = false;

	// La gui
	void OnGUI(){

		// On affiche un label contennant le statut de la connexion au server
		GUILayout.Label ("Sate : " + state);

	}

	// Quand on demarre
	void Start(){

		// On innitialise la position du joueur
		PositionToLerp = transform.position;

		// On ecoute la function 'message'
		manager.Socket.On ("message", OnMessageReceved);

		// On ecoute la function 'receved code'
		manager.Socket.On ("receved code", OnCodeReceved);

	}

	// Quand on recoit un message du server
	void OnMessageReceved(Socket socket, Packet packet, object[] args){

		// On met le statut reçus par le serveur
		state = (string)args[0];

	}

	// Quand on recoit des codes du server
	void OnCodeReceved(Socket socket, Packet packet, object[] args){

		// On récup le code
		Code code = JsonUtility.FromJson<Code> ((string)args [0]);

		// Si l'id ne correspont pas a notre id on retourne
		if (code.id != Identifier)
			return;

		// On met la position reçus, dans le lerp...
		PositionToLerp = new Vector3(code.x, code.y, code.z);

		particle = code.IsFume;

	}

	// Appeler constament
	void Update(){

		if(GetComponent<PlayerSystem>().IsLocal)
			SendCode (transform.position, GetComponent<PlayerSystem>().Particule.enableEmission ,Identifier);
		
		// On lerp la position sur la position qu'on récup du server...
		transform.position = Vector3.Lerp (transform.position, PositionToLerp, Speed * Time.deltaTime);
		//transform.position = PositionToLerp;

		GetComponent<PlayerSystem> ().Particule.enableEmission = particle;

	}

	// Fonction qui envoit les données néssaicaire aux personnage
	void SendCode(Vector3 Position, bool Fume, string Id){

		Code code 	= new Code ();
		code.x 		= Position.x;
		code.y 		= Position.y;
		code.z 		= Position.z;
		code.IsFume = Fume;
		code.id 	= Id;

		string json = JsonUtility.ToJson (code);
		manager.Socket.Emit ("send code", json);

	}

}
