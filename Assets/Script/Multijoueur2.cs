using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using BestHTTP;
using BestHTTP.SocketIO;

public class Multijoueur2 : MonoBehaviour {

	// L'adresse du serveur
	static string ServerAdress = "http://91.134.243.214:8080/socket.io/";
	// Le manager
	SocketManager Manager;

	// Le nom du deuxieme joueur
	string NameOfFriend;
	// Le transform du deuxieme joueur
	Transform TransformOfFriend;
	// La position a lerper du deuxieme joueur
	Vector3 PositionToLerpOfFriend;
	// La position a lerper pour moi
	Vector3 PositionToLerpOfMe;
	// Le procedural code
	private string ProceduralCode;
	// Le transform du joueur actuel
	[HideInInspector]
	public Transform TransformOfMe;
	// La position ou on va commencer.
	Vector3 PositionOfStarted;

	void Start(){
		InnitialiseConnexion ();
	}

	// Pour innitialiser la connexion au serveur...
	public void InnitialiseConnexion(){
		// On innitialise notre manager avec l'url du serveur
		Manager = new SocketManager (new System.Uri (ServerAdress));

		// On écoutes les differents Callback venant du serveur
		Manager.Socket.On ("OnRegister", 				OnRegister);
		Manager.Socket.On ("OnPlayerFinded", 			OnPlayerFinded);
		Manager.Socket.On ("RecevedPositionFriends", 	OnRecevedPositionFriends);
		Manager.Socket.On ("OnPlayerNotFinded", 		OnPlayerNotFinded);
		Manager.Socket.On ("Deconnexion", 				OnFriendDisconnect);
		Manager.Socket.On ("OnFriendDead",	 			OnFriendDead);
		Manager.Socket.On ("OnFriendWin", 				OnFriendWin);
		Manager.Socket.On ("RecevedPositionOfMe", 		OnRecevedPositionOfMe);
		Manager.Socket.On ("RecevedEvent", 				OnRecevedEvent);

		// On s'enregistre avec un nom...
		Register ("Alexis");

	}

	// Permet de se deconnecter...
	public void Disconnect(){
		if (Manager != null) {
			Manager.Socket.Emit("Deconnexion", NameOfFriend);
			Debug.Log ("Send disconnect : " + NameOfFriend);
		}
	}

	// Permet de s'enregister sur le serveur..
	void Register(string PlayerName){
		ProceduralCode = ProceduralSystem.GenerateLevelString (1000);
		Manager.Socket.Emit ("Register", OnRegister, PlayerName, ProceduralCode);
	}

	// Quand on s'est enregistrer sur le serveur.
	void OnRegister(Socket socket, Packet packet, params object[] args){
		string response = (string)args [0];
		Debug.Log ("Response Register : " + response);
	}

	// Quand un autre joueur a était trouver.
	void OnPlayerFinded(Socket socket, Packet packet, params object[] args){
		// On récupère le nom de notre amis
		NameOfFriend = (string)args [0];
		// On récupere le ProceduralCode de notre amis
		if(args.Length > 1)
			ProceduralCode = (string)args[1];
		if (PositionOfStarted != new Vector3 (-1, 0, 0))
			PositionOfStarted = new Vector3 (1, 0, 0);
		// Si le transform de notre ami et null on l'instantie
		if (TransformOfFriend == null) {
			GameObject Friend = (GameObject)Instantiate (Resources.Load ("Prefabs/Player"));
			TransformOfFriend = Friend.transform;
		}
		// Si notre transform et null on l'instantie
		if (TransformOfMe == null) {
			GameObject Me = (GameObject)Instantiate (Resources.Load ("Prefabs/Player"));
			TransformOfMe = Me.transform;
			TransformOfMe.name = "Nous";
			// On nous met a la position destiner
			TransformOfMe.position = PositionOfStarted;
		}
		// On génère la map
		TransformOfMe.GetComponent<LodSystem>().GenerateLevelViewString(ProceduralCode);
		// On lance le counter...
		GetComponent<CameraCounter> ().LaunchCounter ();
		// On va demander au serveur d'envoyer notre position a notre amis par son nom
		Manager.Socket.Emit ("SendMyPosition", NameOfFriend, JsonUtility.ToJson (TransformOfMe.position));
	}

	// Quand on a pas trouver d'autre joueur
	void OnPlayerNotFinded(Socket socket, Packet packet, params object[] args){
		PositionOfStarted = new Vector3 (-1, 0, 0);
	}

	// Quand on recois la position du deuxieme joueur.
	void OnRecevedPositionFriends(Socket socket, Packet packet, params object[] args){
		// Quand on recois la position de notre amis on va la mettre dans la variable a lerper
		PositionToLerpOfFriend = JsonUtility.FromJson<Vector3> ((string)args[0]);
		//Debug.Log("Receved Position from my friends");

	}

	// Quand notre amis et déconnecter
	void OnFriendDisconnect(Socket socket, Packet packet, params object[] args){
		//On se déconnecte
		Disconnect ();
		// On detruie l'objet de notre amis
		Destroy (TransformOfFriend.gameObject);
		// On détruit notre obbjet
		Destroy (TransformOfMe.gameObject);

		Manager.Close ();
		Manager = null;
	}

	void OnFriendDead(Socket socket, Packet packet, params object[] args){
		TransformOfMe.GetComponent<PlayerSystem> ().DeadMenu.localPosition = new Vector3 (0, 0, 0);
		TransformOfMe.GetComponent<PlayerSystem> ().DeadMenu.GetChild (2).GetComponent<Text> ().text = "YOU WIN";
		//TransformOfMe.GetComponent<PlayerSystem> ().DeadMenu.GetChild (3).GetComponent<Text> ().text = "+25";
		//PlayerPrefs.SetInt ("Point", PlayerPrefs.GetInt ("Point") + 25);
		TransformOfMe.GetComponent<PlayerSystem> ().enabled = false;
		//TransformOfMe.GetComponent<PlayerSystem> ().DeadMenu.GetChild (0).transform.GetChild (0).GetComponent<Text> ().text = PlayerPrefs.GetInt ("Point", 0).ToString ();
	}

	void OnFriendWin (Socket socket, Packet packet, params object[] args){
		TransformOfMe.GetComponent<PlayerSystem> ().DeadMenu.localPosition = new Vector3 (0, 0, 0);
		TransformOfMe.GetComponent<PlayerSystem> ().DeadMenu.GetChild (2).GetComponent<Text> ().text = "YOU LOSE";
		TransformOfMe.GetComponent<PlayerSystem> ().DeadMenu.GetChild (3).GetComponent<Text> ().text = "";
		TransformOfMe.GetComponent<PlayerSystem> ().enabled = false;
	}

	void OnRecevedEvent(Socket socket, Packet packet, params object[] args){
		string EventName = (string)args [0];
		string Args = "";
		if (args.Length > 1)
			Args = (string)args [1];

		if (EventName == "Flaque") {
			Vector3 position = JsonUtility.FromJson<Vector3> (Args);
			GameObject flaque = (GameObject)Instantiate (Resources.Load ("Prefabs/Flaque"), position, Quaternion.identity);
		} else if (EventName == "Invincible") {
			TransformOfMe.GetComponent<PlayerSystem> ().StartInvincible ();	
		} else {
			
		}
	}

	public void Dead(){
		Manager.Socket.Emit ("OnFriendDead", NameOfFriend);
	}

	public void Win(){
		Manager.Socket.Emit ("OnFriendWin", NameOfFriend);
	}

	// Dans un fixedUpdate pour que sa soit un minimum syncro..
	void Update(){
		// Si le nom de notre amis n'est pas égale a null ou a rien
		if (NameOfFriend != null && NameOfFriend != "" && TransformOfFriend != null && TransformOfMe != null) {
			// On lerp la position de notre amis jusqu'a la position demander
			TransformOfFriend.position = Vector3.Lerp (TransformOfFriend.position, PositionToLerpOfFriend, Time.deltaTime * 15);
			// On lerp notre position
			//TransformOfMe.position = Vector3.Lerp (TransformOfMe.position, PositionToLerpOfMe, Time.deltaTime * 15);
			// On envoie notre position a notre amis via le serveur
			//Manager.Socket.Emit ("SendMyPosition", NameOfFriend, JsonUtility.ToJson (TransformOfMe.position));
		}

	}

	public void SendPosition (Vector3 position){
		Manager.Socket.Emit ("SendMyPosition", NameOfFriend, JsonUtility.ToJson (position));
	}


	void OnRecevedPositionOfMe(Socket socket, Packet packet, params object[] args){
		PositionToLerpOfMe = JsonUtility.FromJson<Vector3> ((string)args [0]);
		TransformOfMe.GetComponent<PlayerSystem> ().WaitServerPosition = false;
	}

	public void SendEvent(string Event, string Args){
		Manager.Socket.Emit ("SendEventTo", NameOfFriend, Event, Args);
	}

}	

