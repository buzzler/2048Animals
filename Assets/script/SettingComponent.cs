using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SmartLocalization;

[RequireComponent (typeof(Animator))]
public class SettingComponent : UIComponent {
	public	Text labelOption;
	public	Text labelLanguage;
	public	Text labelMute;
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
		labelPurchases.text	= lm.GetTextValue("TGM.Restore");
		labelRank.text		= lm.GetTextValue("TGM.Rank");
		labelTutorial.text	= lm.GetTextValue("TGM.Tutorial");
	}

	public	void OnClickLanguage() {
		animator.SetTrigger("trigger_language");
	}

	public	void OnClickMute() {

	}

	public	void OnClickRestore() {

	}

	public	void OnClickCredits() {

	}

	public	void OnClickRank() {

	}

	public	void OnClickTutorial() {
		OnUIReserve(UIType.TUTORIAL);
		OnUIChange();
	}
	
	public	void OnClickClose() {
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
	}

	public	void OnClickEnglish() {
		ChangeLanguage("en");
	}

	public	void OnClickKorean() {
		ChangeLanguage("ko");
	}
}
