using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseComponent : UIComponent {
	public	Text labelPause;
	public	Text labelHome;
    public  Text labelOption;
	public	Text labelRetry;

	void Update() {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			OnClickClose();
		}
	}

	public override void OnUIChangeLanguage (SmartLocalization.LanguageManager lm) {
		base.OnUIChangeLanguage (lm);
		labelPause.text	= lm.GetTextValue ("fnf.ui.pause");
		labelHome.text	= lm.GetTextValue ("fnf.ui.home");
        labelOption.text = lm.GetTextValue ("fnf.ui.option");
		labelRetry.text = lm.GetTextValue ("fnf.ui.retry");
	}

	public	void OnClickClose() {
		AudioPlayerComponent.Play ("fx_click");
		OnUIReserve(parent);
		OnUIBackward ();
	}

	public	void OnClickHome() {
		AudioPlayerComponent.Play ("fx_click");
		OnUIReserve (UIType.TITLE);
		OnUIChange ();
	}

    public  void OnClickSetting() {
        AudioPlayerComponent.Play ("fx_click");
        OnUIReserve (UIType.SETTING);
        OnUIChange ();
    }

	public	void OnClickRetry() {
		AudioPlayerComponent.Play ("fx_click");
		OnUIReserve (UIType.GAME);
		OnUIChange ();
	}
}
