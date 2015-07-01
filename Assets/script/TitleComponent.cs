using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Soomla.Store;

public class TitleComponent : UIComponent {
	public	Text											labelStart;
	public	Text											labelReward;
	public	ThemeChangerComponent							themeChanger;
	public	RectTransform									themeContent;
	public	TitlePurchaseComponent							purchaser;					
	public	Button											buttonStart;
	public	Button											buttonAds;
	private	Observer										observer;
	private	GameObject										head;
	private	Dictionary<AnimalType, ThemeSelectorComponent>	dictionary;

	void Start () {
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		themeContent.sizeDelta = new Vector2(160*themeContent.childCount+20*(themeContent.childCount-1), 160);
	}

	public override void OnUIChangeLanguage (SmartLocalization.LanguageManager lm) { 
		base.OnUIChangeLanguage (lm);
		labelStart.text = lm.GetTextValue ("fnf.ui.start");
		labelReward.text = lm.GetTextValue ("fnf.ui.reward");
	}

	public	override void OnUIStart() {
		base.OnUIStart();
		observer = Observer.GetInstance ();
		dictionary = new Dictionary<AnimalType, ThemeSelectorComponent>();

		int index = 0;
		int counter = 0;
		ThemeSelectorComponent[] selectors = themeContent.GetComponentsInChildren<ThemeSelectorComponent>();
		PlayerInfo info = PlayerInfoKeeper.GetInstance().playerInfo;
		foreach (ThemeSelectorComponent tsc in selectors) {
			if (tsc.SetGetAnimalType(info)) {
				index = counter;
			}
			dictionary.Add(tsc.theme.type, tsc);
			counter++;
		}

		CheckUnlock();

		// center aglign themeContent
		themeContent.anchoredPosition = new Vector3(index * -180 + 300, 0);
		MakeHead (selectors[index].theme);

		observer.themeChange += OnThemeChange;
		observer.networkStatusChange += OnChangeNetwork;
		observer.inventoryChange += OnUpdateInventory;
	}

	public	override void OnUIStop() {
		base.OnUIStop ();
		observer.themeChange -= OnThemeChange;
		observer.networkStatusChange -= OnChangeNetwork;
		observer.inventoryChange -= OnUpdateInventory;
		ClearHead ();
	}

	public	override void OnUIPause() {
		base.OnUIPause ();
		if (head != null) {
			head.SetActive (false);
		}
	}

	public	override void OnUIResume() {
		base.OnUIResume ();
		if (head != null) {
			head.SetActive (true);
		} else {
			purchaser.Refresh ();
		}
	}
	
	public void ClearHead() {
		if (head != null) {
			GameObject.Destroy(head);
			head = null;
		}
	}

	public void MakeHead(ThemeInfo info) {
		ClearHead ();

		ThemeSelectorComponent tsc = dictionary[info.type];
		if (tsc.state == ThemeSelectorState.UNLOCKED) {
			head = GameObject.Instantiate (Resources.Load("head/"+info.code.ToLower())) as GameObject;
			purchaser.ClearThemeInfo();
			buttonStart.interactable = true;
		} else if (tsc.state == ThemeSelectorState.BLINDED) {
			purchaser.SetThemeInfo(info, false);
			buttonStart.interactable = false;
		} else {
			purchaser.SetThemeInfo(info);
			buttonStart.interactable = false;
		} 
	}

	public	void OnChangeNetwork(bool availability) {
		buttonAds.gameObject.SetActive (availability);
	}

	public	void OnThemeChange(ThemeInfo info) {
		ClearHead ();
		MakeHead (info);
	}

	public	void OnUpdateInventory(string id, int balance) {
		ThemeInfo theme = ThemeInfo.Find(id);
		if ((theme!=null) && (balance>0)) {
			ThemeSelectorComponent tsc = dictionary[theme.type];
			tsc.SetGetAnimalType(PlayerInfoKeeper.GetInstance().playerInfo);
			RefreshHead(tsc);
			CheckUnlock();
		}
	}

	public	void RefreshHead(ThemeSelectorComponent tsc) {
		ClearHead();
		purchaser.ClearThemeInfo();
		MakeHead (tsc.theme);
	}

	public void OnClickPlay() {
		AudioPlayerComponent.Play ("fx_click");
		SendMessageUpwards("ReserveNextUI", UIType.GAME);
		OnUIChange ();
	}

	public	void OnClickBuy(ThemeInfo info) {
		AudioPlayerComponent.Play ("fx_click");
		StoreInventory.BuyItem(info.id);
	}

	public	void CheckUnlock() {
		ThemeSelectorComponent min = null;
		foreach (ThemeSelectorComponent tsc in dictionary.Values) {
			if (tsc.state != ThemeSelectorState.BLINDED) {
				continue;
			}

			if (min==null) {
				min = tsc;
				continue;
			}

			if (tsc.theme.order < min.theme.order) {
				min = tsc;
				continue;
			}
		}

		if (min != null) {
			min.Locked();
		}
	}
}
