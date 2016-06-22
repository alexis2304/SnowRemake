using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

public class CameraCounter : MonoBehaviour {
	
	public void LaunchCounter(){
		StartCoroutine (Counter (5));
	}

	IEnumerator Counter(int waitingTime){
		int timeWaiting = waitingTime;
		while (timeWaiting >= 0) {
			ShowTimeInUI (timeWaiting);
			yield return new WaitForSeconds (1);
			timeWaiting--;
		}
		GetComponent<MainMultiplayerSystem> ().HideUI ();
	}

	void ShowTimeInUI(int time){
		GetComponent<MainMultiplayerSystem>().CounterUI.transform.GetChild (0).GetComponent<Text> ().text = "Start in " + time.ToString () + "...";
	}
}
