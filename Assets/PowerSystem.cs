using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PowerSystem : MonoBehaviour {

	public Image img;
	public Sprite[] sprites;

	private string nameOfPower;

	public void StartPower(){
		if (nameOfPower == "Flaque") {
			Vector3 pos = Camera.main.GetComponent<Multijoueur2> ().TransformOfMe.position;
			Camera.main.GetComponent<Multijoueur2> ().SendEvent (nameOfPower, JsonUtility.ToJson (pos));
		}
	}

	public void AddPowerValidity(string powerName){
		if (powerName == "Flaque") {
			nameOfPower = powerName;
			img.sprite = sprites [0];
		}
	}
}
