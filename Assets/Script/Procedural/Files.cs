using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class Files : MonoBehaviour {

	public static LevelData ReadFile(string path){
		StreamReader reader = new StreamReader (path);
		string FileContent = reader.ReadToEnd ();
		reader.Close ();

		string[] Spliting0 = FileContent.Split('|');
		LevelData levelData = new LevelData ();

		foreach (string a in Spliting0) {
			if (a == "")
				break;
			string[] b = a.Split ('!');

			Level level = new Level ();

			level.x 	= float.Parse(b[0]);
			level.y 	= float.Parse(b[1]);
			level.size 	= float.Parse(b[2]);

			levelData.datas.Add (level);

		}
		return levelData;
	}

	public static LevelData ReadText(string text){
		string FileContent = text;
		string[] Spliting0 = FileContent.Split('|');
		LevelData levelData = new LevelData ();

		foreach (string a in Spliting0) {
			if (a == "")
				break;
			string[] b = a.Split ('!');

			Level level = new Level ();

			level.x 	= float.Parse(b[0]);
			level.y 	= float.Parse(b[1]);
			level.size 	= float.Parse(b[2]);

			levelData.datas.Add (level);

		}
		return levelData;
	}

	public static void WriteFile(string content, string path){
		StreamWriter writer = new StreamWriter (path);
		writer.Write (content);
		writer.Close ();

	}
}

public class Level{
	public float x;
	public float y;
	public float size;
	public bool AsGenerated = false;
}

public class LevelData{
	public List<Level> datas = new List<Level>();
}
