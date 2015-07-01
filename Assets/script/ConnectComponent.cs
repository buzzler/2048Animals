using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ConnectComponent : UIComponent {
	public	Text					labelTitle;
	public	Text					labelMessage;
	public	Text					labelRetry;
	public	Button					buttonRetry;
	private	int						step;
	private	string					keyMessage = "fnf.ui.connect.wait";
	private	SystemCheckComponent	checker;

	public	override void OnUIChangeLanguage (SmartLocalization.LanguageManager lm) {
		base.OnUIChangeLanguage (lm);
		labelTitle.text		= lm.GetTextValue ("fnf.ui.connect.login");
		labelMessage.text	= lm.GetTextValue (keyMessage);
		labelRetry.text		= lm.GetTextValue ("fnf.ui.retry");
	}

	public	override void OnUIStart () {
		base.OnUIStart ();
		Observer observer = Observer.GetInstance ();
		observer.fbConnet += OnFBConnect;
		observer.fbLogin += OnFBLogin;

		step = 0;
		checker	= GetComponentInParent<SystemCheckComponent> ();
		OnClickRetry ();
	}

	public	override void OnUIStop () {
		base.OnUIStop ();
		Observer observer = Observer.GetInstance ();
		observer.fbConnet -= OnFBConnect;
		observer.fbLogin -= OnFBLogin;
	}

	public	void OnClickClose() {
		AudioPlayerComponent.Play ("fx_click");
		OnUIReserve(parent);
		OnUIBackward ();
	}

	public	void OnClickRetry() {
		AudioPlayerComponent.Play ("fx_click");
		keyMessage = "fnf.ui.connect.wait";
		labelMessage.text = SmartLocalization.LanguageManager.Instance.GetTextValue(keyMessage);
		buttonRetry.gameObject.SetActive(false);
		if (step >= 0 && step <= 2) {
			Invoke ("Step" + step.ToString (), 1f);
		}
	}

	private	void Step0() {
		OnNetwork();
		labelRetry.text += ".";
	}

	private void Step1() {
		checker.ConnectFacebook();
		labelRetry.text += ".";
	}

	private void Step2() {
		checker.LoginFacebook ();
		labelRetry.text += ".";
	}

	private void OnNetwork() {
		if (SystemCheckComponent.network) {
			buttonRetry.gameObject.SetActive(false);
			step = 1;
			Invoke("Step1", 1f);
		} else {
			keyMessage = "fnf.ui.connect.error.net";
			labelMessage.text = SmartLocalization.LanguageManager.Instance.GetTextValue(keyMessage);
			buttonRetry.gameObject.SetActive(true);
		}
	}

	private	void OnFBConnect(bool connectivity) {
		if (connectivity) {
			buttonRetry.gameObject.SetActive(false);
			step = 2;
			Invoke("Step2", 1f);
		} else {
			keyMessage = "fnf.ui.connect.error.fail";
			labelMessage.text = SmartLocalization.LanguageManager.Instance.GetTextValue(keyMessage);
			buttonRetry.gameObject.SetActive(true);
		}
	}

	private void OnFBLogin(bool login) {
		if (login) {
			foreach (ThemeInfo info in ThemeInfo.dictionary.Values) {
				if (info.costType==CostType.CONNECT) {
					Soomla.Store.StoreInventory.GiveItem(info.id, 1);
				}
			}
			OnUIReserve(parent);
			OnUIBackward ();
			step = 3;
		} else {
			keyMessage = "fnf.ui.connect.error.fail";
			labelMessage.text = SmartLocalization.LanguageManager.Instance.GetTextValue(keyMessage);
			buttonRetry.gameObject.SetActive(true);
		}
	}
}
