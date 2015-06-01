using UnityEngine;
using System.Collections;
using SmartLocalization;

public class PreloadComponent : UIComponent {
	public	override void OnUIStart() {
		base.OnUIStart();
		LanguageManager lm = LanguageManager.Instance;
		PlayerInfoKeeper pk = PlayerInfoKeeper.GetInstance();
		if (pk.playerInfo.language==null) {
			string code = lm.GetSupportedSystemLanguageCode();
			if (lm.IsLanguageSupported(code)) {
				pk.playerInfo.language = code;
			} else {
				pk.playerInfo.language = "en";
			}
			pk.Save();
		}
		lm.ChangeLanguage(pk.playerInfo.language);
		Invoke ("OnDelay", 2);
	}

	public	void OnDelay() {
		OnUIReserve (UIType.TITLE);
		OnUIChange ();
	}
}
