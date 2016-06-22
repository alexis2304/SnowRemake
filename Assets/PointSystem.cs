using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PointSystem : MonoBehaviour {

	public Text text;
	public int Point;

	public bool ShowTotalPoint = false;

	// Use this for initialization
	void Start () {
		if (ShowTotalPoint)
			text.text = PlayerPrefs.GetInt ("Point", 0).ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AddPoint(int point){
		Point++;
		text.text = Point.ToString();
		PlayerPrefs.SetInt ("Point", PlayerPrefs.GetInt ("Point", 0) + point);
	}

	public void Refresh(){
		text.text = PlayerPrefs.GetInt ("Point", 0).ToString ();
	}
}
