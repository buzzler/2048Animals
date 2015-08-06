using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SmartLocalization;

public class GameEraseOverlay : PopupOverlay {
	public	Text		text;

	void OnEnable() {
		OnUIChangeLanguage (LanguageManager.Instance);
		LanguageManager.Instance.OnChangeLanguage += OnUIChangeLanguage;
	}
	
	void OnDisable() {
		LanguageManager.Instance.OnChangeLanguage -= OnUIChangeLanguage;
	}
	
	private void OnUIChangeLanguage (SmartLocalization.LanguageManager lm) {
		text.text = lm.GetTextValue ("fnf.ui.erase");
	}

	public	void Erase(RawImage shadow) {
		objectShadow = shadow;
		shadow.gameObject.SetActive (true);

		OnShow ();
	}

	public override void OnShowComplete () {
		base.OnShowComplete ();

		Hashtable hash = new Hashtable ();
		hash.Add ("from", Color.clear);
		hash.Add ("to", Color.white);
		hash.Add ("time", 0.2f);
		hash.Add ("easeType", iTween.EaseType.easeOutCubic);
		hash.Add ("onupdate", "OnTextUpdate");
		hash.Add ("onupdatetarget", gameObject);
		iTween.ValueTo (text.gameObject, hash);
	}

	public	void OnTextUpdate(Color color) {
		text.color = color;
	}

	public	void Quit() {
		Hashtable hash = new Hashtable ();
		hash.Add ("from", Color.white);
		hash.Add ("to", Color.clear);
		hash.Add ("time", 0.1f);
		hash.Add ("easeType", iTween.EaseType.easeInCubic);
		hash.Add ("onupdate", "OnTextUpdate");
		hash.Add ("onupdatetarget", gameObject);
		hash.Add ("oncomplete", "OnOK");
		hash.Add ("oncompletetarget", gameObject);
		iTween.ValueTo (text.gameObject, hash);
	}

	public override void OnQuit() {
		objectShadow.gameObject.SetActive (false);
		base.OnQuit ();
	}
}
