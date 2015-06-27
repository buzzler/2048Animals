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
	public	Text labelPurchases;
	public	Text labelRank;
	public	Text labelTutorial;
	public	Text labelCredits;
	public	Text labelEnglish;
	public	Text labelKorean;

	private Animator animator;

	public	override void OnUIStart() {
		base.OnUIStart();
		animator = GetComponent<Animator>();
	}

	public	override void OnUIChangeLanguage(LanguageManager lm) {
		base.OnUIChangeLanguage(lm);
		labelOption.text	= lm.GetTextValue("TGM.Option");
		labelLanguage.text	= lm.GetTextValue("TGM.Language");
		labelEnglish.text	= lm.GetTextValue("TGM.English");
		labelKorean.text	= lm.GetTextValue("TGM.Korean");
		labelCredits.text	= lm.GetTextValue("TGM.Credits");
		labelMute.text		= lm.GetTextValue("TGM.Mute");
		labelUnmute.text	= lm.GetTextValue("TGM.Unmute");
		labelPurchases.text	= lm.GetTextValue("TGM.Restore");
		labelRank.text		= lm.GetTextValue("TGM.Rank");
		labelTutorial.text	= lm.GetTextValue("TGM.Tutorial");
	}

	public	void OnClickLanguage() {
		AudioPlayerComponent.Play ("fx_click");
		animator.SetTrigger("trigger_language");
	}

	public	void OnClickMute() {
		AudioListener.volume = 0f;
		labelMute.transform.parent.gameObject.SetActive (false);
		labelUnmute.transform.parent.gameObject.SetActive (true);
	}

	public	void OnClickUnmute() {
		AudioListener.volume = 1f;
		labelMute.transform.parent.gameObject.SetActive (true);
		labelUnmute.transform.parent.gameObject.SetActive (false);
	}

	public	void OnClickRestore() {
		AudioPlayerComponent.Play ("fx_click");
	}

	public	void OnClickCredits() {
		AudioPlayerComponent.Play ("fx_click");
	}

	public	void OnClickRank() {
		AudioPlayerComponent.Play ("fx_click");
	}

	public	void OnClickTutorial() {
		AudioPlayerComponent.Play ("fx_click");
		OnUIReserve(UIType.TUTORIAL);
		OnUIChange();
	}
	
	public	void OnClickClose() {
		AudioPlayerComponent.Play ("fx_click");
		AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
		if (info.IsName("Base Layer.language")!=true) {
			OnUIReserve(parent);
			OnUIBackward();
		}
		animator.SetTrigger("trigger_exit");
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

	public	void OnClickChineseTranditional() {

	}

	public	void OnClickArabic() {
		ChangeLanguage("ar");
	}
}
