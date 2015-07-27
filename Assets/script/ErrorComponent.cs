using UnityEngine;
using UnityEngine.UI;

public class ErrorComponent : UIComponent {
	public	Text	textTitle;
	public	Text	textMessage;
	public	float	timeClose;
	private	float	timeStart;

	public override void OnUIChangeLanguage (SmartLocalization.LanguageManager lm) {
		base.OnUIChangeLanguage (lm);
		textTitle.text = lm.GetTextValue ("fnf.ui.connect");
		textMessage.text = lm.GetTextValue ("fnf.ui.connect.error.net");
	}

	public override void OnUIStart () {
		base.OnUIStart ();
		timeStart = Time.time;
	}

	void Update() {
		if (timeClose < (Time.time - timeStart)) {
			OnUIReserve (parent);
			OnUIBackward ();
		}
	}

	public	void OnClickClose() {
		AudioPlayerComponent.Play ("fx_click");
		OnUIReserve (parent);
		OnUIBackward ();
	}
}
