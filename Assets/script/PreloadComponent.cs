using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System.Collections;
using SmartLocalization;

public class PreloadComponent : UIComponent {
	public	RawImage bi;

	public override void OnUIChangeLanguage (LanguageManager lm) {
		base.OnUIChangeLanguage (lm);
		bi.texture = lm.GetTexture ("fnf.ui.bi");
	}

	public	override void OnUIStart() {
		base.OnUIStart();

		// for International Language
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

        #if UNITY_EDITOR || UNITY_ANDROID
		Advertisement.Initialize("44690", false);
        #elif UNITY_IOS
        Advertisement.Initialize("57316", false);
        #endif

		Invoke ("OnDelay", 2);
	}

	public	void OnDelay() {
		OnUIReserve (UIType.TITLE);
		OnUIChange ();
	}
}
