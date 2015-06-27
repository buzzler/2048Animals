using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseComponent : UIComponent {
	public	Text labelPause;
	public	Text labelHome;
	public	Text labelRetry;

	public override void OnUIChangeLanguage (SmartLocalization.LanguageManager lm) {
		base.OnUIChangeLanguage (lm);
		labelPause.text	= lm.GetTextValue ("fnf.ui.pause");
		labelHome.text	= lm.GetTextValue ("fnf.ui.home");
		labelRetry.text = lm.GetTextValue ("fnf.ui.retry");
	}

	public	void OnClickClose() {
		AudioPlayerComponent.Play ("fx_click");
		OnUIReserve(parent);
		OnUIBackward ();
	}

	public	void OnClickChange() {
		AudioPlayerComponent.Play ("fx_click");
		OnUIReserve (UIType.TITLE);
		OnUIChange ();
	}

	public	void OnClickRetry() {
		AudioPlayerComponent.Play ("fx_click");
		OnUIReserve (UIType.GAME);
		OnUIChange ();
	}
}
