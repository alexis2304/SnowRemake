using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LodSystem : MonoBehaviour {

	List<Level> levels = new List<Level>();
	List<GameObject> GameObjects = new List<GameObject> ();
	float yMin = 30;
	float yMax = -30;
	bool isReady = false;

	public void GenerateLevelView(){

		LevelData levelData = Files.ReadFile (Application.persistentDataPath + "/Test.json");
		foreach (Level a in levelData.datas) {
			Level copy = a;
			copy.y = -a.y;
			levels.Add (copy);
		}

		isReady = true;

	}

	public void GenerateLevelViewString(string proceduralLevelData){
		LevelData levelData = Files.ReadText (proceduralLevelData);
		foreach (Level a in levelData.datas) {
			Level copy = a;
			copy.y = -a.y;
			levels.Add (copy);
		}

		isReady = true;
	}

	void Update(){

		yMin = transform.position.y + 20;
		yMax = transform.position.y - 30;

		if (isReady) {
			for (int i = 0; i < levels.Count; i++) {
				if (levels [i].y > yMax) {
					if (!levels [i].AsGenerated) {
						GameObject a = (GameObject)Instantiate (Resources.Load ("Prefabs/Tree"));
						a.transform.position = new Vector3 (levels [i].x, levels [i].y, 0);
						a.transform.localScale = new Vector3 (levels [i].size, levels [i].size, levels [i].size);
						GameObjects.Add (a);
						levels [i].AsGenerated = true;
					} else {
						for (int j = 0; j < GameObjects.Count; j++) {
							if (GameObjects [j].transform.position.y > yMin) {
								Destroy (GameObjects [j]);
								GameObjects.RemoveAt (j);
							}
						}
					}
				}
			}
		}
	}
}
