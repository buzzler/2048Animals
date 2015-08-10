using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RateOverlay : PopupOverlay {
	public	Text		textTitle;
	public	Text		textMessage;
	public	Text		textYes;
	public	Text		textCancel;
	public	Text		textNo;
	public	string		urlAndroid;
	public	string		urliOS;
	public	Button[]	buttons;

	void OnEnable() {
		SmartLocalization.LanguageManager lm = SmartLocalization.LanguageManager.Instance;
		OnUIChangeLanguage (lm);
		lm.OnChangeLanguage += OnUIChangeLanguage;
	}

	void OnDisable() {
		SmartLocalization.LanguageManager.Instance.OnChangeLanguage -= OnUIChangeLanguage;
	}

	private	void OnUIChangeLanguage(SmartLocalization.LanguageManager lm) {
		textTitle.text	= lm.GetTextValue ("fnf.ui.rate.title");
		textMessage.text= lm.GetTextValue ("fnf.ui.rate.message");
		textYes.text	= lm.GetTextValue ("fnf.ui.rate.ok");
		textCancel.text	= lm.GetTextValue ("fnf.ui.rate.cancel");
		textNo.text		= lm.GetTextValue ("fnf.ui.rate.no");
	}

	public	void Show() {
		OnShow ();
	}

	public override void OnShowComplete () {
		base.OnShowComplete ();
		foreach (Button b in buttons) {
			b.interactable = true;
		}
	}

	public override void OnHide (Vector3 direction) {
		foreach (Button b in buttons) {
			b.interactable = false;
		}
		base.OnHide (direction);
	}

	public override void OnHideComplete () {
		switch (selected) {
		case 1:
			// set flag and..
			PlayerInfoManager.instance.flagRate = true;
			Soomla.Store.StoreInventory.GiveItem(StoreAssetInfo.FOOT, 5);
			#if UNITY_IPHONE
			Application.OpenURL(urliOS);
			#elif UNITY_ANDROID && UNITY_EDITOR
			Application.OpenURL(urlAndroid);
			#endif
			break;
		case 2:
			PlayerInfoManager.instance.dateRate = System.DateTime.Now;
			break;
		case 3:
			PlayerInfoManager.instance.flagRate = true;
			break;
		}
		PlayerInfoManager.Save ();
		OnQuit ();
	}
}
