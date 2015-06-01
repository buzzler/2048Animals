using UnityEngine;
using UnityEngine.UI;
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

	public void ResetData() {
		PlayerInfoKeeper.GetInstance().Create();
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

	public	void ForceBuyNext() {
		ThemeSelectorComponent[] selectors = GameObject.FindObjectOfType<TitleComponent> ().CheckNextUnlock ();
		foreach (ThemeSelectorComponent tsc in selectors) {
			if (tsc.state==ThemeSelectorState.LOCKED) {
				StoreInventory.BuyItem(tsc.theme.id);
				break;
			}
		}
//		StoreInventory.BuyItem(ThemeInfo.Find(AnimalType.SOAN).id);
	}

	public	void ForceUnlockNext() {
		ThemeSelectorComponent[] selectors = GameObject.FindObjectOfType<TitleComponent> ().CheckNextUnlock ();
		foreach (ThemeSelectorComponent tsc in selectors) {
			if (tsc.state==ThemeSelectorState.LOCKED) {
				StoreInventory.GiveItem(tsc.theme.id, 1);
				break;
			}
		}
//		StoreInventory.GiveItem(ThemeInfo.Find(AnimalType.BOO).id, 1);
	}

	public	void ForceLockLast() {
		ThemeSelectorComponent[] selectors = GameObject.FindObjectOfType<TitleComponent> ().CheckNextUnlock ();
		for (int i = selectors.Length-1 ; i >= 0 ; i--) {
			ThemeSelectorComponent tsc = selectors[i];
			if (tsc.state==ThemeSelectorState.UNLOCKED) {
				StoreInventory.TakeItem(tsc.theme.id, 1);
				break;
			}
		}
//		StoreInventory.TakeItem(ThemeInfo.Find(AnimalType.BOO).id, 1);
	}

	public	void ForceEquipLast() {
		ThemeSelectorComponent[] selectors = GameObject.FindObjectOfType<TitleComponent> ().CheckNextUnlock ();
		for (int i = selectors.Length-1 ; i >= 0 ; i--) {
			ThemeSelectorComponent tsc = selectors[i];
			if (tsc.state==ThemeSelectorState.UNLOCKED) {
				StoreInventory.EquipVirtualGood(tsc.theme.id);
				break;
			}
		}
//		StoreInventory.EquipVirtualGood(ThemeInfo.Find(AnimalType.BOO).id);
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
