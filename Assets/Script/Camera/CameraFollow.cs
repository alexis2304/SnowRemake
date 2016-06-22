using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public float SpeedFollow = 1.0f;
	public Transform Target;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Target != null) {
			Vector3 position = new Vector3 (transform.position.x, Target.position.y - 3.0f, this.transform.position.z);
			transform.position = Vector3.Lerp (transform.position, position, SpeedFollow * Time.deltaTime);
		}
	}
}
