using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class TutorialComponent : UIComponent {
	public	Text labelPage1_1;
	public	Text labelPage2_1;
	public	Text labelPage2_2;
	public	Text labelPage3_1;
	public	Text labelOk;

	public override void OnUIChangeLanguage (SmartLocalization.LanguageManager lm) {
		base.OnUIChangeLanguage (lm);
		labelOk.text = lm.GetTextValue ("fnf.ui.ok");
		labelPage1_1.text = lm.GetTextValue ("fnf.ui.tutorial.1");
		labelPage2_1.text = lm.GetTextValue ("fnf.ui.tutorial.2");
		labelPage2_2.text = lm.GetTextValue ("fnf.ui.tutorial.3");
		labelPage3_1.text = lm.GetTextValue ("fnf.ui.tutorial.4");
	}

	public	void OnClickClose() {
		OnUIReserve(parent);
		OnUIBackward();
	}
}
