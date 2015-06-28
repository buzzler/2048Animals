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
		bool locked = false;
		ThemeSelectorComponent[] selectors = themeContent.GetComponentsInChildren<ThemeSelectorComponent>();
		PlayerInfo info = PlayerInfoKeeper.GetInstance().playerInfo;
		foreach (ThemeSelectorComponent tsc in selectors) {
			if (tsc.SetGetAnimalType()==info.type) {
				index = counter;
			}
			if ((locked!=true) && (tsc.state==ThemeSelectorState.BLINDED)) {
				tsc.Locked();
				locked = true;
			}

			dictionary.Add(tsc.theme.type, tsc);

			counter++;
		}

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
			tsc.Unlocked();
			RefreshHead(tsc);
			CheckNextUnlock();

			PlayerInfoKeeper keeper = PlayerInfoKeeper.GetInstance();
			switch (theme.buffInfo.type) {
			case BuffType.COIN:
				keeper.playerInfo.buffInfoCoin = theme.buffInfo;
				break;
			case BuffType.SCORE:
				keeper.playerInfo.buffInfoScore = theme.buffInfo;
				break;
			case BuffType.REWARD:
				keeper.playerInfo.buffInfoReward = theme.buffInfo;
				break;
			}
			keeper.Save();
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

	public	void OnClickSelector(ThemeSelectorComponent tsc) {
		AudioPlayerComponent.Play ("fx_click");
		SendMessageUpwards("ReserveTheme", tsc.theme);
	}

	public	void OnClickBuy(ThemeInfo info) {
		AudioPlayerComponent.Play ("fx_click");
		StoreInventory.BuyItem(info.id);
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
					tsc.Locked();
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
			SendMessageUpwards("ReserveTheme", tsc.theme);
			break;
		}
		return selectors;
	}
}
