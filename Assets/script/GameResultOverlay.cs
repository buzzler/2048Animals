using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameResultOverlay : OverlayComponent {
	public	Text		textCount;
	public	Text		textMessage;
	public	RawImage	shadow;
	public	Image		panel;
	public	RawImage[]	stars;
	public	Image		title;
	public	Button		button;
	public	Sprite		spriteVictory;
	public	Sprite		spriteGameOver;
	public	Texture2D	textureSuccess;
	public	Texture2D	textureEnable;
	public	Texture2D	textureDisable;

	private	int starSuccess;
	private	int starEnable;
	private	int starIndex;

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

		if (starSuccess == stars.Length) {
			starEnable = starSuccess;
			textCount.text = starSuccess.ToString() + " / " + starSuccess.ToString();
			textMessage.text = SmartLocalization.LanguageManager.Instance.GetTextValue("fnf.ui.clear");
		} else if (starSuccess >= starTarget) {
			starEnable = starSuccess+1;
			textCount.text = starSuccess.ToString() + " / " + starSuccess.ToString();
			textMessage.text = SmartLocalization.LanguageManager.Instance.GetTextValue("fnf.ui.level.up");
		} else {
			starEnable = Mathf.Min(starTarget+1, stars.Length);
			textCount.text = starSuccess.ToString() + " / " + starTarget.ToString();
			textMessage.text = SmartLocalization.LanguageManager.Instance.GetTextValue("fnf.ui.level.practice");
		}
	}

	void OnDisable() {
		CancelInvoke();
	}

	public	void Victory(OverlayEventHandler handler) {
		onOK += handler;
		title.sprite = spriteVictory;
		OnShow();
	}

	public	void GameOver(OverlayEventHandler handler) {
		onOK += handler;
		title.sprite = spriteGameOver;
		OnShow();
	}

	public	void OnShow() {
		Hashtable hash = new Hashtable();
		hash.Add("from", 1f);
		hash.Add("to", 0f);
		hash.Add("time", 0.5f);
		hash.Add("delay", 0.5f);
		hash.Add("easetype", iTween.EaseType.easeOutCubic);
		hash.Add("onupdate", "OnShowUpdate");
		hash.Add("onupdatetarget", gameObject);
		hash.Add("oncomplete", "OnShowComplete");
		hash.Add("oncompletetarget", gameObject);
		iTween.ValueTo(gameObject, hash);
	}

	public	void OnShowUpdate(float value) {
		Vector3 pos = transform.position;
		pos.y = value * 1400f;
		panel.transform.localPosition = pos;

		Color c = shadow.color;
		c.a = (1f-value) * 0.8f;
		shadow.color = c;
	}

	public	void OnShowComplete() {
		Invoke("ShowEffect", 0.2f);
	}

	private	void ShowEffect() {
		RawImage raw = stars[starIndex];
		if (starIndex < starSuccess) {
			raw.texture = textureSuccess;
			EffectComponent.Show(EffectType.STAR_SUCCESS, raw.transform.position).SetParent(panel.transform);
		} else if (starIndex < starEnable) {
			raw.texture = textureEnable;
			EffectComponent.Show(EffectType.STAR_ABLE, raw.transform.position).SetParent(panel.transform);
		}

		starIndex++;
		if (starIndex < stars.Length) {
			Invoke("ShowEffect", 0.2f);
		} else {
			button.interactable = true;
		}
	}

	public	void OnHide() {
		AudioPlayerComponent.Play("fx_click");
		Hashtable hash = new Hashtable();
		hash.Add("from", 0f);
		hash.Add("to", 1f);
		hash.Add("delay", 0.5f);
		hash.Add("time", 0.3f);
		hash.Add("easetype", iTween.EaseType.easeInCubic);
		hash.Add("onupdate", "OnHideUpdate");
		hash.Add("onupdatetarget", gameObject);
		hash.Add("oncomplete", "OnHideComplete");
		hash.Add("oncompletetarget", gameObject);
		iTween.ValueTo(gameObject, hash);
	}

	public	void OnHideUpdate(float value) {
		Vector3 pos = transform.position;
		pos.y = value * -1400f;
		panel.transform.localPosition = pos;
		
		Color c = shadow.color;
		c.a = (1f-value) * 0.8f;
		shadow.color = c;
	}

	public	void OnHideComplete() {
		OnOK();
	}
}
