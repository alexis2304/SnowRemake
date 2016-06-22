using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class StoreSystem : MonoBehaviour {
	
	public static int position = 0;
	int Min = 0;
	int Max = 0;
	public static List<Skin> skins = new List<Skin>();
	string WebsiteUrl = "http://www.codeandeat.fr/SkinManager.php";
	public Image image;
	public Text Text_price;
	public Button buy_button;
	public PointSystem pointsys;
	public static Texture2D text;

	// Use this for initialization
	void Start () {
		DownloadAndUpateSkins ();
	}

	void Innitialise(){
		image.sprite = Sprite.Create(skins [position].texture, new Rect(0, 0, skins [position].texture.width, skins [position].texture.height), Vector2.zero);
		string playerSkins = PlayerPrefs.GetString ("PlayerSkins", "");

		if (!string.IsNullOrEmpty (playerSkins)) {
			PlayerSkins playersk = JsonUtility.FromJson<PlayerSkins> (playerSkins);
			foreach (string a in playersk.urls) {
				if (a == skins [position].url) {
					Text_price.text = "Change";
					if (text = skins [position].texture)
						Text_price.transform.parent.GetComponent<Button> ().interactable = false;
					return;
				}
			}
			Text_price.text = skins [position].price;	
		}else{
			Text_price.text = skins [position].price;	
		}
	}

	public void Next(){
		if (position < Max) {
			position++;
		} else {
			position = Min;
		}
		Innitialise ();
	}

	public void Prev(){
		if (position > Min) {
			position--;
		} else {
			position = Max;
		}
		Innitialise ();
	}

	public void DownloadAndUpateSkins(){
		StartCoroutine (DownloadAndUpdate());
	}

	IEnumerator DownloadAndUpdate(){
		WWWForm form = new WWWForm ();
		form.AddField ("GetSkins", "3984403b932304=0502");

		WWW www = new WWW (WebsiteUrl, form);
		yield return www;
		string message = www.text;
		string[] skin = message.Split ('|');
		foreach (string a in skin) {
			if (!string.IsNullOrEmpty (a)) {
				string _price = a.Split (',') [0];
				string _url = a.Split (',') [1];
				skins.Add (new Skin (_price, _url));
			}
		}

		foreach (Skin a in skins) {
			a.texture = new Texture2D (512, 512, TextureFormat.DXT5, false);
			WWW w= new WWW(a.url);
			yield return w;
			w.LoadImageIntoTexture (a.texture);
		}

		Min = 0;
		Max = skins.Count -1;
		Innitialise ();

	}

	public void Buy(){
		string playerSkins = PlayerPrefs.GetString ("PlayerSkins", "");
		PlayerSkins playersk;
		if (!string.IsNullOrEmpty (playerSkins)) {
			playersk = JsonUtility.FromJson<PlayerSkins> (playerSkins);
			if (buy_button.transform.GetChild (0).GetComponent<Text> ().text != "Change") {
				int prices = int.Parse (skins [position].price);
				int money = PlayerPrefs.GetInt ("Point", 0);
				if (money >= prices) {
					money -= prices;
					PlayerPrefs.SetInt ("Point", money);
					playersk.urls.Add (skins [position].url);
					PlayerPrefs.SetString ("PlayerSkins", JsonUtility.ToJson (playersk));
					pointsys.Refresh ();
					Innitialise ();
				}
			} else {
				text = skins [position].texture;
				Innitialise ();
			}
		} else {
			playersk = new PlayerSkins ();
			if (buy_button.transform.GetChild (0).GetComponent<Text> ().text != "Change") {
				int prices = int.Parse (skins [position].price);
				int money = PlayerPrefs.GetInt ("Point", 0);
				if (money >= prices) {
					money -= prices;
					PlayerPrefs.SetInt ("Point", money);
					playersk.urls.Add (skins [position].url);
					PlayerPrefs.SetString ("PlayerSkins", JsonUtility.ToJson (playersk));
					pointsys.Refresh ();
					Innitialise ();
				}
			} else {
				text = skins [position].texture;
				Innitialise ();
			}
		}
	}

}

public class Skin{

	public string price { get; private set;}
	public string url { get; private set; }
	public Texture2D texture { get; set; }

	public Skin(string Price, string Url){
		price = Price;
		url = Url;
	}
}


[System.Serializable]
public class PlayerSkins{
	public List<string> urls 	= new List<string>();
	public List<string> prices = new List<string>();
}
