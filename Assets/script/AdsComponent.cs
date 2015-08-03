using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System;
using System.Collections;
using Soomla.Store;

[RequireComponent(typeof(Button))]
public class AdsComponent : MonoBehaviour {

	public	double		timerMinute;
	private	PlayerInfo	info;
	private	Button		button;

	void OnEnable() {
		info = PlayerInfoManager.instance;
		button = GetComponent<Button> ();
	}

	void Update() {
		if (Advertisement.isReady ()) {
			double t = DateTime.Now.Subtract (info.ads).TotalSeconds - timerMinute*60.0;
			button.interactable = (t >= 0);
		} else {
			button.interactable = false;
		}
	}

	public	void OnClick() {
		if (Advertisement.isReady()) {
			ShowOptions opt = new ShowOptions();
			opt.resultCallback = OnShowComplete;
			Advertisement.Show(null, opt);
            AnalyticsComponent.LogAdEvent(AnalyticsComponent.ACTION_SHOW);
		}
	}

	public	void OnShowComplete(ShowResult result) {
		switch (result) {
		case ShowResult.Finished:
			StoreInventory.GiveItem(StoreAssetInfo.COIN, Mathf.Max((int)info.buffInfoReward.Calculate(1),5));
			info.ads = DateTime.Now;
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
	}
}
