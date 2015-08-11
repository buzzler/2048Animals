using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System;
using System.Collections;
using Soomla.Store;

[RequireComponent(typeof(Button))]
public class AdsComponent : MonoBehaviour {
	public	double		timerMinute;
	public	Button		button;
	public	Button		buttonBlockable;
	public	Image		imageTarget;
	public	Sprite		imageWait;
	public	Sprite		imageReward;
	public	Text		textWait;
	public	Text		textReward;
	public	GameObject	objectCoin;
	private	PlayerInfo	info;

	void Awake() {
		info = PlayerInfoManager.instance;
	}

	void OnDisable() {
		CancelInvoke();
	}

	void Update() {
		if (Advertisement.IsReady ()) {
			double t = DateTime.Now.Subtract (info.dateAds).TotalSeconds - timerMinute*60.0;
			bool ontime = (t >= 0);
			button.interactable = ontime;
			if (ontime) {
				SetReward();
			} else {
				SetWait(t);
			}

		} else {
			button.interactable = false;
			SetReward();
		}
	}

	public	void OnClick() {
		if (Advertisement.IsReady()) {
			ShowOptions opt = new ShowOptions();
			opt.resultCallback = OnShowComplete;
			Advertisement.Show(null, opt);
			if (buttonBlockable!=null) {
				buttonBlockable.interactable = false;
			}
            AnalyticsComponent.LogAdEvent(AnalyticsComponent.ACTION_SHOW);
		}
	}

	public	void OnShowComplete(ShowResult result) {
		switch (result) {
		case ShowResult.Finished:
			StoreInventory.GiveItem(StoreAssetInfo.COIN, Mathf.Max((int)info.buffInfoReward.Calculate(1100),1100));
			info.dateAds = DateTime.Now;
			PlayerInfoManager.Save();
			EffectComponent.Show(EffectType.BONUS, Vector3.zero);
			AudioPlayerComponent.Play ("fx_reward");
            AnalyticsComponent.LogAdEvent(AnalyticsComponent.ACTION_FINISH);
			break;
		case ShowResult.Failed:
			DebugComponent.Log("UNITY AD FAILED");
            AnalyticsComponent.LogAdEvent(AnalyticsComponent.ACTION_FAIL);
			break;
		case ShowResult.Skipped:
			DebugComponent.Log("UNITY AD SKIPPED");
            AnalyticsComponent.LogAdEvent(AnalyticsComponent.ACTION_SKIP);
			break;
		}

		Invoke("Unblock", 1f);
	}

	private	void Unblock() {
		if (buttonBlockable!=null) {
			buttonBlockable.interactable = true;
		}
	}

	private	void SetWait(double t) {
		t = -t;
		int min = Mathf.FloorToInt((float)t / 60f);
		int sec = Mathf.FloorToInt((float)t % 60f);
		textWait.text = ((min < 10) ? "0":"") + min.ToString() + ":" + ((sec < 10) ? "0":"") + sec.ToString();

		if (objectCoin.activeSelf) {
			imageTarget.sprite = imageWait;
			objectCoin.SetActive(false);
			textWait.gameObject.SetActive(true);
		}
	}

	private	void SetReward() {
		int reward = Mathf.Max((int)info.buffInfoReward.Calculate(1100),1100);
		textReward.text = reward.ToString();

		if (!objectCoin.activeSelf) {
			imageTarget.sprite = imageReward;
			objectCoin.SetActive(true);
			textWait.gameObject.SetActive(false);
		}
	}
}
