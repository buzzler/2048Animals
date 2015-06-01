using UnityEngine;
using System.Collections;

public class TitleComponent : UIComponent {

	public	ThemeChangerComponent	themeChanger;
	public	RectTransform			themeContent;
	public	ThemeSelectorComponent	themeSelector;
	private	Observer				observer;
	private	GameObject				head;

	void Start () {
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		themeContent.sizeDelta = new Vector2(160*themeContent.childCount+20*(themeContent.childCount-1), 160);
	}

	public	override void OnUIStart() {
		base.OnUIStart();
		observer = Observer.GetInstance ();

		int index = 0;
		int counter = 0;
		bool locked = false;
		ThemeSelectorComponent[] selectors = themeContent.GetComponentsInChildren<ThemeSelectorComponent>();
		PlayerInfo info = PlayerInfoKeeper.GetInstance().playerInfo;
		foreach (ThemeSelectorComponent tsc in selectors) {
			if (tsc.SetAnimalType(info.type)) {
				index = counter;
			}
			if ((locked!=true) && (tsc.state==ThemeSelectorState.BLINDED)) {
				tsc.Lock();
				locked = true;
			}
			counter++;
		}

		themeContent.anchoredPosition = new Vector3(index * -180 + 300, 0);
		MakeHead (selectors[index].theme);
		observer.themeChange += OnThemeChange;
	}

	public	override void OnUIStop() {
		base.OnUIStop ();
		observer.themeChange -= OnThemeChange;
		ClearHead ();
	}

	public	override void OnUIPause() {
		base.OnUIPause ();
		if (head!=null) {
			head.SetActive(false);
		}
	}

	public	override void OnUIResume() {
		base.OnUIResume ();
		if (head!=null) {
			head.SetActive(true);
		}
	}

	public void ClearHead() {
		if (head != null) {
			GameObject.Destroy(head);
			head = null;
		}
	}

	public void MakeHead(ThemeInfo info) {
		head = GameObject.Instantiate (Resources.Load("head/"+info.name.ToLower())) as GameObject;
	}

	public void OnThemeChange(ThemeInfo info) {
		ClearHead ();
		MakeHead (info);
	}

	public void OnClickPlay() {
		SendMessageUpwards("ReserveNextUI", UIType.GAME);
		OnUIChange ();
	}

	public void SelectAnimal(AnimalType type) {
		SendMessageUpwards("ReserveTheme", type);
	}

	public	void BuyAnimal(ThemeInfo theme) {
		DebugComponent.Log("TODO: open buy interface....");
	}

	public	ThemeSelectorComponent[] CheckNextUnlock() {
		bool locked = false;
		ThemeSelectorComponent[] selectors = themeContent.GetComponentsInChildren<ThemeSelectorComponent>();
		foreach (ThemeSelectorComponent tsc in selectors) {
			switch (tsc.state) {
			case ThemeSelectorState.UNLOCKED:
				break;
			case ThemeSelectorState.LOCKED:
				locked = true;
				break;
			case ThemeSelectorState.BLINDED:
				if (!locked) {
					tsc.Lock();
					locked = true;
				}
				break;
			}
		}
		return selectors;
	}

	public	ThemeSelectorComponent[] LostCurrentTheme() {
		ThemeSelectorComponent[] selectors = themeContent.GetComponentsInChildren<ThemeSelectorComponent>();
		foreach (ThemeSelectorComponent tsc in selectors) {
			SelectAnimal(tsc.theme.type);
			break;
		}
		return selectors;
	}
}
