using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RewardComponent : MonoBehaviour {
	public	Animator	buttonAd;
	public	Animator	buttonGift;
	public	Animator	buttonGacha;
	public	float		delay;
	public	float		interval;

	void OnEnable() {

		float sum = delay;

		if (CheckAd()) {
			Invoke("ShowAd", sum);
			sum += interval;
		} else {
			HideButton(buttonAd);
		}

		if (CheckGift()) {
			Invoke("ShowGift", sum);
			sum += interval;
		} else {
			HideButton(buttonGift);
		}

	}

	private	bool CheckAd() {
		return true;
	}

	private	bool CheckGift() {
		return true;
	}

	private void ShowAd() {
		ShowButton(buttonAd);
	}

	private	void ShowGift() {
		ShowButton(buttonGift);
	}

	private	void ShowGacha() {
		ShowButton(buttonGacha);
	}

	private	void ShowButton(Animator ani) {
		ani.SetTrigger("trigger_show");
	}

	private void HideButton(Animator ani) {
		ani.SetTrigger("trigger_hide");
	}
}
