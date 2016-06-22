using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuMultiplayerButton : MonoBehaviour {

	public int Button_Join_Count 	= 0;
	public int Button_Create_Count 	= 0;

	public Button[] buttons;

	public Text Player_Name;
	public Text Join_Name;

	public Transform panel;
	public PointSystem PointPanel;

	public GameObject PrefabPlayer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Party_Create(){
		if (Button_Create_Count == 0)
			Button_Create_Count++;
		if (Player_Name.text != "" && Button_Create_Count == 2)
			panel.GetComponent<Animator> ().SetBool ("Hide", true);
		if (Player_Name.text != "" && Button_Create_Count == 1) {
			
			foreach (Button a in buttons) {
				//a.transform.parent.GetComponent<Animator> ().enabled = false;
				a.interactable = false;
			}
			this.GetComponent<Multijoueur> ().name = Player_Name.text;
			GameObject player = (GameObject)Instantiate (PrefabPlayer, new Vector3 (-1, 0, 0), Quaternion.identity);
			GetComponent < Multijoueur> ().ActualPlayer = player.transform;
			GetComponent<Multijoueur> ().ActualPlayer.GetComponent<PlayerSystem> ().IsMultiplayer = true;
			player.transform.GetChild (3).GetComponent<Checker> ().pointSystem = PointPanel;
			GetComponent < Multijoueur> ().enabled = true;
		}
	}

	public void Party_Join(){
		if (Button_Join_Count == 0)
			Button_Join_Count++;
		if (Player_Name.text != "" && Button_Join_Count == 2)
			this.GetComponent<Animator> ().SetBool ("Hide", true);
		if (Player_Name.text != "" && Join_Name.text != "" && Button_Join_Count == 1) {

			foreach (Button a in buttons) {
				//a.transform.parent.GetComponent<Animator> ().enabled = false;
				a.interactable = false;
			}
			this.GetComponent<Multijoueur> ().name = Player_Name.text;
			this.GetComponent<Multijoueur> ().sendingPlayerName = Join_Name.text;
			GameObject player = (GameObject)Instantiate (PrefabPlayer, new Vector3 (1, 0, 0), Quaternion.identity);
			GetComponent < Multijoueur> ().ActualPlayer = player.transform;
			GetComponent<Multijoueur> ().ActualPlayer.GetComponent<PlayerSystem> ().IsMultiplayer = true;
			player.transform.GetChild (3).GetComponent<Checker> ().pointSystem = PointPanel;
			GetComponent < Multijoueur> ().enabled = true;
			GetComponent<Multijoueur> ().Join ();
		}
	}

	public void Hide(){
		foreach (Button a in buttons)
			a.interactable = false;
		panel.GetComponent<Animator> ().SetBool ("Hide", true);
	}
}
