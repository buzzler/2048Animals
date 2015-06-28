using UnityEngine;
using UnityEngine.UI;
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
	public	RawImage		rawImage;
	public	Transform		boxHolder;
	public	Transform[]		boxes;
	public	Color			colorUpdate;
	public	Color			colorNormal;
	private	PlayerInfo		info;

	public override void OnUIChangeLanguage (SmartLocalization.LanguageManager lm) {
		base.OnUIChangeLanguage (lm);
		labelTitle.text = lm.GetTextValue ("fnf.ui.result");
		labelBest.text = lm.GetTextValue ("fnf.ui.best");
		labelHome.text = lm.GetTextValue ("fnf.ui.home");
		labelRank.text = lm.GetTextValue ("fnf.ui.rank");
		labelShare.text = lm.GetTextValue ("fnf.ui.share");
		labelRetry.text = lm.GetTextValue ("fnf.ui.retry");
	}

	public	override void OnUIStart () {
		base.OnUIStart();

		PlayerInfoKeeper keeper = PlayerInfoKeeper.GetInstance ();
		info = keeper.playerInfo;

		SetDeltaCoin ();
		SetBestScore();
		SetCurrentScore ();
		SetBox();

		keeper.Save();
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
		AudioPlayerComponent.Play ("fx_click");
	}

	public	void OnClickRank() {
		AudioPlayerComponent.Play ("fx_click");
	}

	public	void OnClickAd() {
		AudioPlayerComponent.Play ("fx_click");
	}
	
	public	void OnClickGift() {
		AudioPlayerComponent.Play ("fx_click");
	}
	
	public	void OnClickGacha() {
		AudioPlayerComponent.Play ("fx_click");
	}

	public	void SetBox() {
//		rawImage.texture = boxes[info.highLevel-1];
		Transform box = Instantiate(boxes[info.highLevel-1], boxHolder.position, Quaternion.identity) as Transform;
		box.SetParent (boxHolder);
	}

	public	void SetDeltaCoin() {
		textCoin.text = StoreInventory.GetItemBalance(StoreAssetInfo.COIN).ToString();
		textCoinDelta.text = info.coinDelta.ToString();
	}

	public	void SetBestScore() {
		bestGroup.SetScore(info.bestScore);
		bestGroup.SetColor(colorNormal);
		currentGroup.SetColor(colorNormal);
	}

	public	void SetCurrentScore() {
		Hashtable hash = new Hashtable();
		hash.Add("from", 0f);
		hash.Add("to", (float)info.score);
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
}
