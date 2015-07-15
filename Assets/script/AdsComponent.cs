﻿using UnityEngine;
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
		info = PlayerInfoKeeper.GetInstance ().playerInfo;
		button = GetComponent<Button> ();
	}

	void Update() {
#if UNITY_EDITOR || UNITY_ANDROID || UNITY_IOS
		if (Advertisement.isReady ()) {
			double t = DateTime.Now.Subtract (info.ads).TotalSeconds - timerMinute*60.0;
			button.interactable = (t >= 0);
		} else {
			button.interactable = false;
		}
#endif
	}

	public	void OnClick() {
#if UNITY_EDITOR || UNITY_ANDROID || UNITY_IOS
		if (Advertisement.isReady()) {
			ShowOptions opt = new ShowOptions();
			opt.resultCallback = OnShowComplete;
			Advertisement.Show(null, opt);
		}
#endif
	}

	public	void OnShowComplete(ShowResult result) {
		switch (result) {
		case ShowResult.Finished:
			StoreInventory.GiveItem(StoreAssetInfo.COIN, Mathf.Max((int)info.buffInfoReward.Calculate(1),1));
			info.ads = DateTime.Now;
			PlayerInfoKeeper.GetInstance().Save();
			EffectComponent.Show(EffectType.BONUS, Vector3.zero);
			break;
		case ShowResult.Failed:
			DebugComponent.Log("UNITY AD FAILED");
			break;
		case ShowResult.Skipped:
			DebugComponent.Log("UNITY AD SKIPPED");
			break;
		}
	}
}
