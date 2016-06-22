using UnityEngine;
using System.Collections;

public class Checker : MonoBehaviour {

	public PointSystem pointSystem;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.name == "Tree(Clone)") {
			pointSystem.AddPoint (1);
			Debug.Log ("Win point");
		}
	}

}
