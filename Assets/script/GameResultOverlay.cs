using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Soomla.Store;

public class GameResultOverlay : PopupOverlay {
	public	Text		textCount;
	public	Text		textMessage;
	public	RawImage[]	stars;
	public	Image		title;
	public	Button		button;
	public	Sprite		spriteVictory;
	public	Sprite		spriteGameOver;
	public	Texture2D	textureSuccess;
	public	Texture2D	textureEnable;
	public	Texture2D	textureDisable;
	public	GameObject	objectCoin;
	public	Text		textCoin;
	public	float[]		coinMux;

	private	int	 starSuccess;
	private	int	 starEnable;
	private	int	 starIndex;
	private	bool rewardable;

	void OnEnable() {
		PlayerInfo pinfo = PlayerInfoManager.instance;
		ThemeInfo tinfo = pinfo.GetThemeInfo();

		int starOld = pinfo.stars[tinfo.order];
		int starMin = tinfo.star;
		int starMax = stars.Length;

		starSuccess = pinfo.highLevel;
		starIndex = 0;

		int starTarget = 0;
		if (starOld == starMax) {
			starTarget = starMax;
		} else if (starOld < starMin) {
			starTarget = starMin;
		} else if (starOld == starMin) {
			starTarget = Mathf.Min(starMin + 1, starMax);
		} else if (starOld > starMin) {
			starTarget = Mathf.Min(starOld + 1, starMax);
		}

		if (starSuccess == starMax) {
			starEnable = starSuccess;
			textCount.text = starSuccess.ToString() + " / " + starSuccess.ToString();
			textMessage.text = SmartLocalization.LanguageManager.Instance.GetTextValue("fnf.ui.clear");
			rewardable = (starOld < starMax);
		} else if (starSuccess >= starTarget) {
			starEnable = starSuccess+1;
			textCount.text = starSuccess.ToString() + " / " + starSuccess.ToString();
			textMessage.text = SmartLocalization.LanguageManager.Instance.GetTextValue("fnf.ui.level.up");
			rewardable = true;
		} else {
			starEnable = Mathf.Min(starTarget+1, starMax);
			textCount.text = starSuccess.ToString() + " / " + starTarget.ToString();
			textMessage.text = SmartLocalization.LanguageManager.Instance.GetTextValue("fnf.ui.level.practice");
			rewardable = false;
		}
		objectCoin.SetActive(rewardable);
	}

	void OnDisable() {
		CancelInvoke();
	}

	public	void Victory(OverlayEventHandler handler) {
		onClose += handler;
		title.sprite = spriteVictory;
		OnShow();
	}

	public	void GameOver(OverlayEventHandler handler) {
		onClose += handler;
		title.sprite = spriteGameOver;
		OnShow();
	}
	
	public override void OnShowComplete () {
		base.OnShowComplete ();
		GetReward();
		Invoke("ShowEffect", 0.2f);
	}

	private	void ShowEffect() {
		RawImage raw = stars[starIndex];
		if (starIndex < starSuccess) {
			raw.texture = textureSuccess;
			EffectComponent.Show(EffectType.STAR_SUCCESS, raw.transform.position).SetParent(objectPanel.transform);
		} else if (starIndex < starEnable) {
			raw.texture = textureEnable;
			EffectComponent.Show(EffectType.STAR_ABLE, raw.transform.position).SetParent(objectPanel.transform);
		}

		starIndex++;
		if (starIndex < stars.Length) {
			Invoke("ShowEffect", 0.2f);
		} else {
			button.interactable = true;
		}
	}

	public	void OnClickButton() {
		AudioPlayerComponent.Play ("fx_click");
		OnClose ();
	}

	private	void GetReward() {
		if (rewardable) {
			PlayerInfo pinfo = PlayerInfoManager.instance;
			int num = pinfo.highLevel;
			int coin = (int)(pinfo.buffInfoCoin.Calculate(110f) * coinMux[num]);

			Hashtable hash = new Hashtable();
			hash.Add("from", 0);
			hash.Add("to", coin);
			hash.Add("time", (float)num*0.2f);
			hash.Add("onupdate", "OnCoinUpdate");
			hash.Add("onupdatetarget", gameObject);
			hash.Add("oncomplete", "OnCoinComplete");
			hash.Add("oncompletetarget", gameObject);
			hash.Add("oncompleteparams", coin);
			iTween.ValueTo(objectCoin, hash);
		}
	}

	public	void OnCoinUpdate(int value) {
		textCoin.text = value.ToString();
	}

	public	void OnCoinComplete(int coin) {
		Utility.GiveCoin(coin);
	}
}
