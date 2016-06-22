using UnityEngine;
using System.Collections;

public class ProceduralMenu : MonoBehaviour {

	public bool Up = true;
	public bool ReadOnlyProcedural = false;

	// Use this for initialization
	void Start () {
		if(!ReadOnlyProcedural)
		ProceduralSystem.GenerateLevel (1000);
		
		Camera.main.gameObject.AddComponent<LodSystem> ().GenerateLevelView ();
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y > -900 && !Up)
			transform.Translate (-Vector3.up * 1 * Time.deltaTime);
		else
			Up = true;

		if(transform.position.y < -100 && Up)
			transform.Translate (Vector3.up * 1 * Time.deltaTime);
		else
			Up = false;
	}
}
