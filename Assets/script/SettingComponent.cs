using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SmartLocalization;

[RequireComponent (typeof(Animator))]
public class SettingComponent : UIComponent {
	public	Text labelOption;
	public	Text labelLanguage;
	public	Text labelMute;
	public	Text labelUnmute;
	public	Text labelRank;
	public	Text labelTutorial;
	public	Text labelEnglish;
	public	Text labelKorean;
	public	Text labelLogin;
	public	Text labelLogout;
	public	Button	buttonLogin;
	public	Button	buttonLogout;

	private Animator animator;
	private	bool		lastMute;
	private	bool		lastFB;

	public	override void OnUIChangeLanguage(LanguageManager lm) {
		base.OnUIChangeLanguage(lm);
		labelOption.text	= lm.GetTextValue("fnf.ui.option");
		labelLanguage.text	= lm.GetTextValue("fnf.ui.language");
		labelEnglish.text	= lm.GetTextValue("fnf.lang.english");
		labelKorean.text	= lm.GetTextValue("fnf.lang.korean");
		labelMute.text		= lm.GetTextValue("fnf.ui.mute");
		labelUnmute.text	= lm.GetTextValue("fnf.ui.unmute");
		labelRank.text		= lm.GetTextValue("fnf.ui.rank");
		labelTutorial.text	= lm.GetTextValue("fnf.ui.tutorial");
		labelLogin.text		= lm.GetTextValue ("fnf.ui.connect.login");
		labelLogout.text	= lm.GetTextValue ("fnf.ui.connect.logout");
	}

	public	override void OnUIStart() {
		base.OnUIStart();
		animator = GetComponent<Animator>();
		lastMute = false;
		lastFB = false;

		WhichMute (AudioListener.volume == 0);
		WhichInOut (SystemCheckComponent.network && FB.IsInitialized && FB.IsLoggedIn);
	}

	void Update() {
		bool curMute = (AudioListener.volume == 0);
		bool curFB = (SystemCheckComponent.network && FB.IsInitialized && FB.IsLoggedIn);

		if (curMute != lastMute) {
			WhichMute(curMute);
			lastMute = curMute;
		}
		if (curFB != lastFB) {
			WhichInOut(curFB);
			lastFB = curFB;
		}
	}

	public	override void OnUIStop() {
		base.OnUIStop();
	}

	private void WhichMute(bool mute) {
		if (mute) {
			labelMute.transform.parent.gameObject.SetActive (false);
			labelUnmute.transform.parent.gameObject.SetActive (true);
		} else {
			labelMute.transform.parent.gameObject.SetActive (true);
			labelUnmute.transform.parent.gameObject.SetActive (false);
		}
	}

	private	void WhichInOut(bool login) {
		if (login) {
			buttonLogout.interactable = true;
			buttonLogout.gameObject.SetActive (true);
			buttonLogin.interactable = false;
			buttonLogin.gameObject.SetActive (false);
		} else {
			buttonLogin.interactable = true;
			buttonLogin.gameObject.SetActive (true);
			buttonLogout.interactable = false;
			buttonLogout.gameObject.SetActive (false);
		}
	}

	public	void OnClickLanguage() {
		AudioPlayerComponent.Play ("fx_click");
		animator.SetTrigger("trigger_language");
	}

	public	void OnClickMute() {
		AudioListener.volume = 0f;
	}

	public	void OnClickUnmute() {
		AudioListener.volume = 1f;
	}

	public	void OnClickRank() {
		AudioPlayerComponent.Play ("fx_click");
	}

	public	void OnClickTutorial() {
		AudioPlayerComponent.Play ("fx_click");
		OnUIReserve(UIType.TUTORIAL);
		OnUIChange();
	}

	public	void OnClickLogin() {
		AudioPlayerComponent.Play ("fx_click");
		OnUIReserve(UIType.CONNECT);
		OnUIChange();
	}

	public	void OnClickLogout() {
		AudioPlayerComponent.Play ("fx_click");
		GetComponentInParent<SystemCheckComponent> ().LogoutFacebook ();
	}

	public	void OnClickClose() {
		AudioPlayerComponent.Play ("fx_click");
		AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
		animator.SetTrigger("trigger_exit");
		if (info.IsName("Base Layer.language")!=true) {
			OnUIReserve(parent);
			OnUIBackward();
		}
	}

	private	void ChangeLanguage(string code) {
		PlayerInfoKeeper pk = PlayerInfoKeeper.GetInstance();
		pk.playerInfo.language = code;
		pk.Save();
		
		LanguageManager.Instance.ChangeLanguage(code);
		animator.SetTrigger("trigger_exit");

		AudioPlayerComponent.Play ("fx_click");
	}

	public	void OnClickEnglish() {
		ChangeLanguage("en");
	}

	public	void OnClickDeutch() {
		ChangeLanguage("de");
	}

	public	void OnClickEspanol() {
		ChangeLanguage("es");
	}

	public	void OnClickJapanese() {
		ChangeLanguage("ja");
	}

	public	void OnClickChineseSimple() {
		ChangeLanguage("zh-CN");
	}

	public	void OnClickChineseTraditional() {
		ChangeLanguage("zh-TW");
	}

	public	void OnClickRussian() {
		ChangeLanguage("ru");
	}

	public	void OnClickFrancais() {
		ChangeLanguage("fr");
	}

	public	void OnClickItaliano() {
		ChangeLanguage("it");
	}

	public	void OnClickPortugues() {
		ChangeLanguage("pt");
	}

	public	void OnClickKorean() {
		ChangeLanguage("ko");
	}

	public	void OnClickArabic() {
		ChangeLanguage("ar");
	}
}
