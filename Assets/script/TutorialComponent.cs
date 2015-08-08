using UnityEngine;
using UnityEngine.UI;
using SmartLocalization;

public class TutorialComponent : PopupOverlay {
	public	Text labelPage1_1;
	public	Text labelPage2_1;
	public	Text labelPage2_2;
	public	Text labelPage3_1;
	public	Text labelOk;

	void OnEnable() {
		LanguageManager.Instance.OnChangeLanguage += OnUIChangeLanguage;
	}

	void OnDisable() {
		LanguageManager.Instance.OnChangeLanguage -= OnUIChangeLanguage;
	}

	public	void Show(OverlayEventHandler OkHandler = null) {
		if (OkHandler != null) {
			onOK += OkHandler;
		}
		OnShow ();
	}

	public	void OnUIChangeLanguage (SmartLocalization.LanguageManager lm) {
		labelOk.text = lm.GetTextValue ("fnf.ui.ok");
		labelPage1_1.text = lm.GetTextValue ("fnf.ui.tutorial.1");
		labelPage2_1.text = lm.GetTextValue ("fnf.ui.tutorial.2");
		labelPage2_2.text = lm.GetTextValue ("fnf.ui.tutorial.3");
		labelPage3_1.text = lm.GetTextValue ("fnf.ui.tutorial.4");
	}

	public	void OnClick() {
		AudioPlayerComponent.Play ("fx_click");
		OnOK ();
	}
}
