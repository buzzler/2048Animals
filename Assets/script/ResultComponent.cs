using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Soomla;
using Soomla.Store;

public class ResultComponent : UIComponent {

	public	Text			labelCoin;
	public	Text			labelCoinDelta;
	public	ScoreComponent	bestGroup;
	public	ScoreComponent	currentGroup;
	public	Color			colorUpdate;
	public	Color			colorNormal;
	private	PlayerInfo		info;

	public	override void OnUIStart () {
		base.OnUIStart();
		info = PlayerInfoKeeper.GetInstance().playerInfo;
		// change medium image to selected animal's one
		SetBestScore();
		labelCoin.text = StoreInventory.GetItemBalance(StoreAssetInfo.COIN).ToString();
		labelCoinDelta.text = info.coinDelta.ToString();
	}

	public	void OnClickRetry() {
		OnUIReserve (UIType.GAME);
		OnUIChange ();
	}

	public	void OnClickTitle() {
		OnUIReserve (UIType.TITLE);
		OnUIChange ();
	}

	public	void OnClickShare() {

	}

	public	void OnClickRank() {

	}

	public	void OnClickAd() {
		
	}
	
	public	void OnClickGift() {
		
	}
	
	public	void OnClickGacha() {
		
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

	public	void OnScoreComplete() {
		PlayerInfoKeeper.GetInstance().Save();
	}
}
