using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Datatest : MessageBase{
	public string a = "";
}

public class DrawingPath : NetworkBehaviour {

	List<Vector3> Points = new List<Vector3> ();
	LineRenderer lr;
	public int Lenght = 10;
	public Color StartColor;
	public Color EndColor;

	ParticleSystem particle;

	public float SpeedDescente = 5.0f;
	public float SpeedMoving = 0.0f;
	public Vector2 direction;
	public bool isNetwork = false;
	private Transform skin;

	[SyncVar]
	public bool isFume = false;

	// Use this for initialization
	void Start () {
		//Debug.Log(ProceduralGeneration.GetPublicIp ());
		skin = transform.GetChild (1).transform;
		particle = transform.GetChild (0).GetComponent<ParticleSystem> ();
		// On prend le LineRenderer
		lr = GetComponent<LineRenderer> ();

		// On l'innitialise (couleur etc...)
		lr.SetColors(StartColor, EndColor);

		// On Démarre !
		InvokeRepeating ("AddLine", 0, 0.05f);


	}
	
	// Update is called once per frame
	void Update () {
		
		if (!isNetwork) {
			if(transform.position.y <= -1000)
				//transform.GetChild(1).GetComponent<DeadPlayer>().deadfunc();
			//Debug.Log ("Width : " + Screen.width + " Position : " + Camera.main.WorldToViewportPoint (transform.position));
			//transform.GetChild (2).transform.rotation = skin.rotation;
			transform.Translate (-Vector2.up * SpeedDescente * Time.deltaTime);
			if (Input.GetKey (KeyCode.Q) || Input.GetKey(KeyCode.RightArrow)) {
				SpeedMoving = Mathf.Lerp (SpeedMoving, -10, 2 * Time.deltaTime);
				SpeedDescente = Mathf.Lerp (SpeedDescente, 1, 1 * Time.deltaTime);
				direction = Vector2.Lerp (direction, new Vector2 (1, 0), 2 * Time.deltaTime);
				transform.Translate (direction * SpeedMoving * Time.deltaTime);
				skin.rotation = Quaternion.Euler (Vector3.Lerp (skin.rotation.eulerAngles, new Vector3 (0, 0, 0), 4 * Time.deltaTime));
				if (!particle.emission.enabled && SpeedMoving < -6) {
					particle.enableEmission = true;
				} else {
					particle.enableEmission = false;
				}
			} else if (Input.GetKey (KeyCode.D) || Input.GetKey(KeyCode.LeftArrow)) {
				SpeedMoving = Mathf.Lerp (SpeedMoving, 10, 2 * Time.deltaTime);
				SpeedDescente = Mathf.Lerp (SpeedDescente, 1, 1 * Time.deltaTime);
				direction = Vector2.Lerp (direction, new Vector2 (1, 0), 2 * Time.deltaTime);
				transform.Translate (direction * SpeedMoving * Time.deltaTime);
				skin.rotation = Quaternion.Euler (Vector3.Lerp (skin.rotation.eulerAngles, new Vector3 (0, 0, 90), 4 * Time.deltaTime));
				if (!particle.emission.enabled && SpeedMoving > 6) {
					particle.enableEmission = true;
				} else {
					particle.enableEmission = false;
				}
			} else {
				SpeedDescente = Mathf.Lerp (SpeedDescente, 10, 1 * Time.deltaTime);
				SpeedMoving = Mathf.Lerp (SpeedMoving, 0, 1 * Time.deltaTime);
				direction = Vector2.Lerp (direction, new Vector2 (0, 0), 2 * Time.deltaTime);
				transform.Translate (direction * SpeedMoving * Time.deltaTime);
				skin.rotation = Quaternion.Euler (Vector3.Lerp (skin.rotation.eulerAngles, new Vector3 (0, 0, 45), 4 * Time.deltaTime));
				if (particle.emission.enabled) {
					particle.enableEmission = false;
				}
			}
		}
	}

	void AddLine(){
		if (Points.Count > Lenght) {
			while (Points.Count > Lenght) {
				Points.RemoveAt (0);
			}
		}
		Points.Add (transform.position - new Vector3(0, 0, 1));
		lr.SetVertexCount (Points.Count);
		for (int i = 0; i < Points.Count; i++) {
			lr.SetPosition (i, Points [i]);
		}
	}
}
