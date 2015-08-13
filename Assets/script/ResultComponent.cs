using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using Soomla;
using Soomla.Store;

public class ResultComponent : UIComponent {

	public	Text			labelTitle;
	public	Text			labelBest;
	public	Text			labelHome;
	public	Text			labelRank;
	public	Text			labelShare;
	public	Text			labelRetry;
	public	Text			textCoin;
	public	Text			textCoinDelta;
	public	ScoreComponent	bestGroup;
	public	ScoreComponent	currentGroup;
	public	Transform		boxHolder;
	public	Transform[]		boxes;
	public	Color			colorUpdate;
	public	Color			colorNormal;
	public	Button			buttonHome;
	public	Button			buttonShare;
	public	Button			buttonRetry;
	public	RateOverlay		overlayRate;
	private	PlayerInfo		info;
	private	int				flash;
	private	bool			uploading;
	private	bool			interactable;

	void Update() {
		SystemCheckComponent scc = GetComponentInParent<SystemCheckComponent> ();
		buttonHome.interactable = interactable;
		buttonRetry.interactable = interactable;
		buttonShare.interactable = interactable && SystemCheckComponent.network && FB.IsInitialized && FB.IsLoggedIn && scc.HasScreenshot ();
		if (uploading) {
			if (!scc.HasScreenshot ()) {
				uploading = false;
			}
			OnUIChangeLanguage (SmartLocalization.LanguageManager.Instance);
		}

		if (Input.GetKeyDown (KeyCode.Escape)&&escapable) {
			OnClickTitle();
		}
	}

	public override void OnUIChangeLanguage (SmartLocalization.LanguageManager lm) {
		base.OnUIChangeLanguage (lm);
		labelTitle.text = lm.GetTextValue ("fnf.ui.result");
		labelBest.text = lm.GetTextValue ("fnf.ui.best");
		labelHome.text = lm.GetTextValue ("fnf.ui.home");
		labelRank.text = lm.GetTextValue ("fnf.ui.rank");
		labelShare.text = uploading ? lm.GetTextValue("fnf.ui.connect.wait"):lm.GetTextValue ("fnf.ui.share");
		labelRetry.text = lm.GetTextValue ("fnf.ui.retry");
	}

	public	override void OnUIStart () {
		base.OnUIStart();

		Observer.GetInstance ().maskOpen += OnMaskOpen;
		Observer.GetInstance ().currencyChange += OnChangeCurrency;

		info = PlayerInfoManager.instance;
		flash = 0;

		SetDeltaCoin ();
		SetBestScore();
		OnScoreUpdate (0);
		SetBox();

        AnalyticsComponent.LogGameEvent(AnalyticsComponent.ACTION_SCORE, (long)currentGroup.GetScore());
        AnalyticsComponent.LogGameEvent(AnalyticsComponent.ACTION_COIN, (long)info.gameCoin);
	}

	public	void OnMaskOpen() {
		SetCurrentScore ();
		PlayerInfoManager.Save();
	}

	private	void OnChangeCurrency(int balance, int delta) {
		textCoin.text = balance.ToString ();
	}

	public override void OnUIStop () {
		base.OnUIStop ();
		for (int i = boxHolder.childCount-1 ; i >= 0 ; i--) {
			DestroyImmediate(boxHolder.GetChild(i).gameObject);
		}
		CancelInvoke ();
		Observer.GetInstance ().currencyChange -= OnChangeCurrency;
		Observer.GetInstance ().maskOpen -= OnMaskOpen;
		interactable = false;
	}

	public	void OnClickRetry() {
		AudioPlayerComponent.Play ("fx_click");
		OnUIReserve (UIType.GAME);
		OnUIChange ();
	}

	public	void OnClickTitle() {
		AudioPlayerComponent.Play ("fx_click");
		OnUIReserve (UIType.TITLE);
		OnUIChange ();
	}

	public	void OnClickShare() {
		if (!uploading) {
			AudioPlayerComponent.Play ("fx_click");
			GetComponentInParent<SystemCheckComponent> ().UploadFacebook ();
			uploading = true;
		}
	}

	public	void OnClickRank() {
		AudioPlayerComponent.Play ("fx_click");
	}

	public	void SetBox() {
		Transform box = Instantiate(boxes[info.highLevel-1], boxHolder.position, Quaternion.identity) as Transform;
		box.SetParent (boxHolder);
	}

	public	void SetDeltaCoin() {
		textCoin.text = StoreInventory.GetItemBalance(StoreAssetInfo.COIN).ToString();
		textCoinDelta.text = info.gameCoin.ToString();
	}

	public	void SetBestScore() {
		bestGroup.SetScore(info.bestScore);
		bestGroup.SetColor(colorNormal);
		currentGroup.SetColor(colorNormal);
	}

	public	void SetCurrentScore() {
		Hashtable hash = new Hashtable();
		hash.Add("from", 0f);
		hash.Add("to", (float)info.gameScore);
		hash.Add("easetype", iTween.EaseType.easeOutCubic);
		hash.Add("onupdatetarget", gameObject);
		hash.Add("onupdate", "OnScoreUpdate");
		hash.Add("oncompletetarget", gameObject);
		hash.Add("oncomplete", "OnScoreComplete");
		hash.Add("time", 2);
		iTween.ValueTo(currentGroup.gameObject, hash);
	}

	public	void OnScoreUpdate(float value) {
		currentGroup.SetScore(value);
		if (value>=info.bestScore) {
			currentGroup.SetColor(colorUpdate);
		}
	}

	public	void OnScoreComplete() {
		int delta = (int)currentGroup.GetScore () - (int)bestGroup.GetScore ();
		if (delta > 5000) {
			flash = 21;
		} else if (delta > 1000) {
			flash = 11;
		} else if (delta > 100) {
			flash = 6;
		}
		interactable = true;
		Invoke("Flash", 0.3f);
	}

	public	void Flash() {
		flash--;
		if (flash > 0) {
			float rndX = UnityEngine.Random.Range (-0.1f, 0.1f);
			float rndY = UnityEngine.Random.Range (-0.1f, 0.1f);
			rndX += (rndX > 0) ? 0.1f : -0.1f;
			rndY += (rndY > 0) ? 0.1f : -0.1f;
			EffectComponent.Show (EffectType.FACE_DRAG_OUT, boxHolder.position + new Vector3 (rndX, rndY, 0f));
			Invoke ("Flash", 0.5f);
		} else if (!info.flagRate) {
			if ((DateTime.Now.Subtract(info.dateRate).TotalHours >= 12) && (StoreInventory.GetItemBalance(StoreAssetInfo.COIN) > 3300)) {
				RateOverlay overlay = GameObject.Instantiate<RateOverlay>(overlayRate);
				overlay.transform.SetParent(transform, false);
				overlay.Show();
			}
		}
	}
}
