using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System;
using System.Collections;

[RequireComponent(typeof(Button))]
public class AdsComponent : MonoBehaviour {

	public	double timerMinute;

	void OnEnable() {
		if (Advertisement.isReady ()) {
			PlayerInfo info = PlayerInfoKeeper.GetInstance ().playerInfo;
			TimeSpan ts = DateTime.Now.Subtract(info.ads);
			GetComponent<Button> ().interactable = (ts.TotalMinutes >= timerMinute);
		}
	}

	void OnDisable() {
	}

	public	void OnClick() {
		if (Advertisement.isReady()) {
			PlayerInfo info = PlayerInfoKeeper.GetInstance ().playerInfo;
			info.ads = DateTime.Now;
			PlayerInfoKeeper.GetInstance().Save();

			ShowOptions opt = new ShowOptions();
			opt.resultCallback = OnShowComplete;
			Advertisement.Show(null, opt);
		}
	}

	public	void OnShowComplete(ShowResult result) {
		switch (result) {
		case ShowResult.Finished:
			DebugComponent.Log("UNITY AD FINISHED");
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
