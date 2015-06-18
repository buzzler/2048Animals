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

		PlayerInfoKeeper keeper = PlayerInfoKeeper.GetInstance ();
		info = keeper.playerInfo;
		keeper.Save();

		// change medium image to selected animal's one

		SetDeltaCoin ();
		SetBestScore();
		SetCurrentScore ();
	}

	public	void OnClickRetry() {
		SendMessageUpwards ("PlayFx", "fx_click");
		OnUIReserve (UIType.GAME);
		OnUIChange ();
	}

	public	void OnClickTitle() {
		SendMessageUpwards ("PlayFx", "fx_click");
		OnUIReserve (UIType.TITLE);
		OnUIChange ();
	}

	public	void OnClickShare() {
		SendMessageUpwards ("PlayFx", "fx_click");
	}

	public	void OnClickRank() {
		SendMessageUpwards ("PlayFx", "fx_click");
	}

	public	void OnClickAd() {
		SendMessageUpwards ("PlayFx", "fx_click");
	}
	
	public	void OnClickGift() {
		SendMessageUpwards ("PlayFx", "fx_click");
	}
	
	public	void OnClickGacha() {
		SendMessageUpwards ("PlayFx", "fx_click");
	}

	public	void SetDeltaCoin() {
		labelCoin.text = StoreInventory.GetItemBalance(StoreAssetInfo.COIN).ToString();
		labelCoinDelta.text = info.coinDelta.ToString();
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
