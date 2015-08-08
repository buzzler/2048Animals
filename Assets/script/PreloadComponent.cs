using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System.Collections;
using Soomla.Store;
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
		PlayerInfo info = PlayerInfoManager.instance;
		if (info.language==null) {
			string code = lm.GetSupportedSystemLanguageCode();
			if (lm.IsLanguageSupported(code)) {
				info.language = code;
			} else {
				info.language = "en";
			}
			PlayerInfoManager.Save();
		}
		lm.ChangeLanguage(info.language);

        #if UNITY_EDITOR || UNITY_ANDROID
		Advertisement.Initialize("44690", false);
        #elif UNITY_IOS
        Advertisement.Initialize("57316", false);
        #endif

		if (info.flagFoot != true) {
			StoreInventory.GiveItem (StoreAssetInfo.FOOT, 5);
			info.flagFoot = true;
			PlayerInfoManager.Save ();
		}

		Invoke ("OnDelay", 2);
	}

	public	void OnDelay() {
		OnUIReserve (UIType.TITLE);
		OnUIChange ();
	}
}
