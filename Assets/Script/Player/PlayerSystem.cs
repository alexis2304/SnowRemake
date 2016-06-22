using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class PlayerSystem : MonoBehaviour {
	
	// Le transform de l'ombre
	private Transform Player_Shadow;

	// Le transform du skin
	private Transform Player_Skin;

	// Le systeme de particule (fumer, neige)
	[HideInInspector]
	public ParticleSystem Particule;

	// Si le joueur peut bouger ou pas...
	private bool CanLowering = true;

	// Vitesse de descente Minimum et maximum
	public float LoweringSpeedMax = 10;
	public float LoweringSpeedMin = 1;

	public float TurnSpeed = 10.0f;

	// Les axes de direction
	private float axisX = 0.0f;
	private float axisY = 0.0f;

	// L'axis de rotation
	private float axisZ = 0;

	// La fluidité du mouvement
	public float Smoothing = 1.0f;
	public float SmoothingTurn = 1.0f;
	public float SmoothingRot = 1.0f;

	// Le diviseur qui fait en sorte que, quand le joueur va a droite ou a gauche, la fummer se déclenche.
	public float ParticuleEmitionDiviseur = 4;

	public RectTransform DeadMenu;

	// Definie si le joueur et controller ou pas
	public bool IsLocal = true;

	// La largeur de l'ecran
	private float ScreenWidth;

	// Si les commande son sur telephone ou pas
	public bool isMobile = true;

	// Si le joueur emmet des particule
	public bool EmitParticle = false;

	private bool TouchDown = false;

	public bool IsMultiplayer = false;

	public bool WaitServerPosition = false;

	public bool IsInvincible = false;

	// Use this for initialization
	void Start () {

		//ProceduralSystem.GenerateLevel (1000);
		//GetComponent<LodSystem> ().GenerateLevelView ();

		// On assign les variables
		Player_Shadow 	= transform.GetChild (0);
		Player_Skin 	= transform.GetChild (1);
		Particule 		= transform.GetChild (2).GetComponent<ParticleSystem> ();

		if (EmitParticle) {
			// On démarre le particle system
			Particule.Play ();
		}

		// on met la largeur de l'écran
		ScreenWidth = Screen.width;

		// On innitialise le skin du perso avec celui acheter dans la boutique
		if (IsMultiplayer)
			Camera.main.GetComponent<Multijoueur2> ().SendPosition (transform.position);

	}
	
	// Update is called once per frame
	void Update () {

		if(!IsLocal)
			return;
		// Si on et en multijoueur on envoie constamment notre position
		if (IsMultiplayer)
			Camera.main.GetComponent<Multijoueur2> ().SendPosition (transform.position);
		
		Vector3 PositionPlayerOnScreen = Camera.main.WorldToScreenPoint (transform.position);

		if (PositionPlayerOnScreen.x <= 0 || PositionPlayerOnScreen.x >= Screen.width) {
			GetComponent<TrailRenderer> ().Clear ();
			if (IsMultiplayer) {
				//Camera.main.GetComponent<Multijoueur> ()._action.isActivateTrail = false;
			}
		} else {
			GetComponent<TrailRenderer> ().enabled = true;
			if (IsMultiplayer) {
				//Camera.main.GetComponent<Multijoueur> ()._action.isActivateTrail = true;
			}
		}

		if (PositionPlayerOnScreen.x > Screen.width) {
			transform.position = Camera.main.ScreenToWorldPoint (new Vector3 (0, PositionPlayerOnScreen.y, PositionPlayerOnScreen.z));
		}

		if (PositionPlayerOnScreen.x < 0) {
			transform.position = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width, PositionPlayerOnScreen.y, PositionPlayerOnScreen.z));
		}

		#region Particle

		if(axisX > TurnSpeed / ParticuleEmitionDiviseur | axisX < -TurnSpeed / ParticuleEmitionDiviseur)
		{
			Particule.enableEmission = true;
			//if(IsMultiplayer)
				//Camera.main.GetComponent<Multijoueur> ()._action.isFume = true;
		}else{
			Particule.enableEmission = false;
			//if(IsMultiplayer)
				//Camera.main.GetComponent<Multijoueur> ()._action.isFume = false;
		}

		#endregion

		#region Touch System
		TouchDown = false;
		foreach(Touch a in Input.touches){
			//Debug.Log(string.Format("Touch position : {0} , deltaPosition : {1} , rawPosition : {2} , Screen Width : {3}", a.position, a.deltaPosition, a.rawPosition, Screen.width));
			TouchDown = true;

			// Si la position du touch et inférieur a la largeur de l'écran diviser par deux
			if(a.position.x < ScreenWidth / 2){
				axisX = Mathf.Lerp (axisX, -TurnSpeed, SmoothingTurn * Time.deltaTime);
				axisY = Mathf.Lerp (axisY, LoweringSpeedMin, Smoothing * Time.deltaTime);
				axisZ = Mathf.Lerp(axisZ, -70, SmoothingRot * Time.deltaTime);
			}else if(a.position.x > ScreenWidth / 2){
				axisX = Mathf.Lerp (axisX, TurnSpeed, SmoothingTurn * Time.deltaTime);
				axisY = Mathf.Lerp (axisY, LoweringSpeedMin, Smoothing * Time.deltaTime);
				axisZ = Mathf.Lerp(axisZ, 70, SmoothingRot * Time.deltaTime);
			}else{
				axisX = Mathf.Lerp (axisX, 0, 1 * Time.deltaTime);
				axisY = Mathf.Lerp (axisY, LoweringSpeedMax, Smoothing * Time.deltaTime);
				axisZ = Mathf.Lerp(axisZ, 0, SmoothingRot * Time.deltaTime);
			}
		}

		if (CanLowering) {
			transform.position = Vector3.Lerp (transform.position, new Vector3 (transform.position.x + axisX, transform.position.y + axisY, 0), Smoothing * Time.deltaTime);
			Player_Skin.rotation = Quaternion.Euler(0, 0, axisZ);
		}

		if(!TouchDown && isMobile){
			axisX = Mathf.Lerp (axisX, 0, 1 * Time.deltaTime);
			axisY = Mathf.Lerp (axisY, LoweringSpeedMax, Smoothing * Time.deltaTime);
			axisZ = Mathf.Lerp(axisZ, 0, SmoothingRot * Time.deltaTime);
		}
		#endregion

		if (isMobile)
			return;

		#region Translation & Rotation
		if (Input.GetKey (KeyCode.D)) {
			axisX = Mathf.Lerp (axisX, TurnSpeed, SmoothingTurn * Time.deltaTime);
			axisY = Mathf.Lerp (axisY, LoweringSpeedMin, Smoothing * Time.deltaTime);
			axisZ = Mathf.Lerp(axisZ, 70, SmoothingRot * Time.deltaTime);

		}else if (Input.GetKey (KeyCode.Q)) {
			axisX = Mathf.Lerp (axisX, -TurnSpeed, SmoothingTurn * Time.deltaTime);
			axisY = Mathf.Lerp (axisY, LoweringSpeedMin, Smoothing * Time.deltaTime);
			axisZ = Mathf.Lerp(axisZ, -70, SmoothingRot * Time.deltaTime);

		}else{
			axisX = Mathf.Lerp (axisX, 0, 1 * Time.deltaTime);
			axisY = Mathf.Lerp (axisY, LoweringSpeedMax, Smoothing * Time.deltaTime);
			axisZ = Mathf.Lerp(axisZ, 0, SmoothingRot * Time.deltaTime);
		}
		#endregion

	}

	public void PlayFume(){
		Particule.enableEmission = true;
	}

	public void StopFume(){
		Particule.enableEmission = false;
	}

	public void StartInvincible (){
		StartCoroutine (invincible());
	}

	IEnumerator invincible(){
		IsInvincible = true;
		yield return new WaitForSeconds (10);
		IsInvincible = false;
	}

	void OnTriggerEnter2D(Collider2D other) {
		
		if (other.name == "Tree(Clone)" || other.tag == "Player" || other.tag == "Flaque" || other.tag == "Dead" && !IsInvincible) {
			DeadMenu.localPosition = new Vector3 (0, DeadMenu.localPosition.y, DeadMenu.localPosition.z);
			DeadMenu.GetChild (2).GetComponent<Text> ().text = "YOU LOSE";

			if (IsMultiplayer) {
				//Debug.Log ("Dead Multiplayer");
				//DeadMenu.GetChild (0).transform.GetChild (0).GetComponent<Text> ().text = PlayerPrefs.GetInt ("Point", 0).ToString ();
				Camera.main.GetComponent<Multijoueur2> ().Dead ();
			}
			GetComponent<PlayerSystem> ().enabled = false;
		}


		if (other.name == "End(Clone)") {
			DeadMenu.localPosition = new Vector3 (0, DeadMenu.localPosition.y, DeadMenu.localPosition.z);
			DeadMenu.GetChild (2).GetComponent<Text> ().text = "YOU WIN";
			GetComponent<PlayerSystem> ().enabled = false;
			if (IsMultiplayer) {
				Camera.main.GetComponent<Multijoueur2> ().Win ();
			}
			GetComponent<PlayerSystem> ().enabled = false;
		}
	}
}
