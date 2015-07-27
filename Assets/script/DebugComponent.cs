using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System.Collections;
using System.Collections.Generic;
using Soomla;
using Soomla.Store;

public class DebugComponent : MonoBehaviour {
	public static List<string> messages = new List<string>();

	public	Text console;
	public	bool isDebugMode;
	private	bool last;

	// Use this for initialization
	void Start () {
		isDebugMode = false;
		last = false;
		TurnOff();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount >= 3) {
			for (int i = 0 ; i < 3 ; i++) {
				if (Input.GetTouch(i).phase == TouchPhase.Began) {
					isDebugMode = !isDebugMode;
					break;
				}
			}
		} else if (Input.GetKeyDown("space")){
			isDebugMode = !isDebugMode;
		}

		if (isDebugMode!=last) {
			if (isDebugMode) {
				TurnOn();
			} else {
				TurnOff();
			}
			last = isDebugMode;
		}

		if (isDebugMode) {
			string sum = "";
			foreach (string msg in messages) {
				sum = msg +"\n"+ sum;
			}
			console.text = sum;
		}
	}

	public void FBToggle() {
		Application.LoadLevel((Application.loadedLevel+1)%Application.levelCount);
	}

	public	void ForceBG2A() {
		BackgroundComponent bgc = GameObject.FindObjectOfType<BackgroundComponent>();
		bgc.A();
	}

	public	void ForceBG2B() {
		BackgroundComponent bgc = GameObject.FindObjectOfType<BackgroundComponent>();
		bgc.B();
	}

	public	void ForceBG2C() {
		BackgroundComponent bgc = GameObject.FindObjectOfType<BackgroundComponent>();
		bgc.C();
	}

	public	void ForceBG2D() {
		BackgroundComponent bgc = GameObject.FindObjectOfType<BackgroundComponent>();
		bgc.D();
	}

	public	void ShowAds() {
#if UNITY_EDITOR || UNITY_ANDROID || UNITY_IOS
		if (Advertisement.isReady()) {
			Advertisement.Show();
		}
#endif
	}

	public	void GetCoin() {
		StoreInventory.GiveItem(StoreAssetInfo.COIN,100000);
	}

	public	void TakeAllCoin() {
		StoreInventory.TakeItem(StoreAssetInfo.COIN, StoreInventory.GetItemBalance(StoreAssetInfo.COIN));
        StoreInventory.TakeItem(StoreAssetInfo.FOOT, StoreInventory.GetItemBalance(StoreAssetInfo.FOOT));
		foreach (ThemeInfo theme in ThemeInfo.dictionaryId.Values) {
			if (theme.costAmount>0) {
				StoreInventory.TakeItem(theme.id, 1);
			} else {
				StoreInventory.GiveItem(theme.id, 1);
			}
		}
		Application.Quit ();
	}

	public void TurnOn () {
		for (int i = 0 ; i < transform.childCount ; i++) {
			transform.GetChild(i).gameObject.SetActive(true);
		}
	}

	public void TurnOff () {
		for (int i = 0 ; i < transform.childCount ; i++) {
			transform.GetChild(i).gameObject.SetActive(false);
		}
	}

	public static void Error(string error) {
		Log(error);
	}

	public static void Log(string message) {
		messages.Add(message);
		Debug.Log(message);
	}
}
