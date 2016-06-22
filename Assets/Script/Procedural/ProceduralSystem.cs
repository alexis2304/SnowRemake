using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class ProceduralSystem : MonoBehaviour {

	public static void GenerateLevel(int Distance, float Started = 10){
		float RandomizeX = UnityEngine.Random.Range (-100000.0f, 100000.0f);
		float RandomizeY = UnityEngine.Random.Range (-100000.0f, 100000.0f);

		float xMin, xMax;

		if ((UnityEngine.iOS.Device.generation.ToString ()).IndexOf ("iPad") > -1) {
			xMin = -15;
			xMax = 15;
			Debug.Log("This device is an iPad");

		} else if ((UnityEngine.iOS.Device.generation.ToString ()).IndexOf ("iPhone") > -1) {
			xMin = -10;
			xMax = 10;
			Debug.Log("This device is an iPhone");

		} else {
			xMin = -10;
			xMax = 10;
			// Ici si c'est Mac, Windows Linux...

		}

		List<string> datas = new List<string>(); 
		Level level = new Level ();
		for (float y = Started; y < Distance; y+=0.5f) {
			for (float x = xMin; x < xMax; x+=0.5f) {
				float result = Mathf.PerlinNoise (x + 0.01f + RandomizeX, y + 0.01f + RandomizeY);
				if (Math.Round (result, 2) == 0.45) {
					string data = x.ToString () + "!" + y.ToString () + "!" + (UnityEngine.Random.Range (3f, 5f)).ToString() + "|";
					datas.Add (data);
				}

			}
		}

		string results = "";
		foreach (string a in datas) {
			results += a;
		}

		Files.WriteFile (results, Application.persistentDataPath + "/Test.json");

	}

	public static string GenerateLevelString(int Distance, float Started = 10){
		float xMin, xMax;

		if ((UnityEngine.iOS.Device.generation.ToString ()).IndexOf ("iPad") > -1) {
			xMin = -15;
			xMax = 15;
			Debug.Log("This device is an iPad");

		} else if ((UnityEngine.iOS.Device.generation.ToString ()).IndexOf ("iPhone") > -1) {
			xMin = -10;
			xMax = 10;
			Debug.Log("This device is an iPhone");

		} else {
			xMin = -10;
			xMax = 10;
			// Ici si c'est Mac, Windows Linux...

		}

		float RandomizeX = UnityEngine.Random.Range (-100000.0f, 100000.0f);
		float RandomizeY = UnityEngine.Random.Range (-100000.0f, 100000.0f);

		List<string> datas = new List<string>(); 
		Level level = new Level ();
		for (float y = Started; y < Distance; y+=0.5f) {
			for (float x = xMin; x < xMax; x+=0.5f) {
				float result = Mathf.PerlinNoise (x + 0.01f + RandomizeX, y + 0.01f + RandomizeY);
				if (Math.Round (result, 2) == 0.45) {
					string data = x.ToString () + "!" + y.ToString () + "!" + (UnityEngine.Random.Range (3f, 5f)).ToString() + "|";
					datas.Add (data);
				}

			}
		}

		string results = "";
		foreach (string a in datas) {
			results += a;
		}

		return results;
	}
}
