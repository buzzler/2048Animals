using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SmartLocalization;

public class GameEraseOverlay : OverlayComponent {
	public	Text		text;
	public	Color		fromColor;
	public	Color		toColor;
	private	RawImage	shadow;

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
		this.shadow = shadow;
		shadow.gameObject.SetActive (true);

		Hashtable hash = new Hashtable ();
		hash.Add ("from", fromColor);
		hash.Add ("to", toColor);
		hash.Add ("time", 0.3f);
		hash.Add ("easeType", iTween.EaseType.easeInCubic);
		hash.Add ("onupdate", "OnShadowUpdate");
		hash.Add ("onupdatetarget", gameObject);
		hash.Add ("oncomplete", "OnText");
		hash.Add ("oncompletetarget", gameObject);
		iTween.ValueTo (shadow.gameObject, hash);
	}

	public	void OnShadowUpdate(Color color) {
		shadow.color = color;
	}

	public	void OnText() {
		Hashtable hash = new Hashtable ();
		hash.Add ("from", Color.clear);
		hash.Add ("to", Color.white);
		hash.Add ("time", 0.2f);
		hash.Add ("easeType", iTween.EaseType.easeOutCubic);
		hash.Add ("onupdate", "OnTextUpdate");
		hash.Add ("onupdatetarget", gameObject);
		hash.Add ("oncomplete", "OnTextComplete");
		hash.Add ("oncompletetarget", gameObject);
		iTween.ValueTo (text.gameObject, hash);
	}

	public	void OnTextUpdate(Color color) {
		text.color = color;
	}

	public	void OnTextComplete() {
	}

	public	void Quit() {
		Hashtable hash = new Hashtable ();
		hash.Add ("from", Color.white);
		hash.Add ("to", Color.clear);
		hash.Add ("time", 0.1f);
		hash.Add ("easeType", iTween.EaseType.easeInCubic);
		hash.Add ("onupdate", "OnTextUpdate");
		hash.Add ("onupdatetarget", gameObject);
		hash.Add ("oncomplete", "OnQuiting");
		hash.Add ("oncompletetarget", gameObject);
		iTween.ValueTo (text.gameObject, hash);
	}

	public	void OnQuiting() {
		Hashtable hash = new Hashtable ();
		hash.Add ("from", toColor);
		hash.Add ("to", fromColor);
		hash.Add ("time", 0.2f);
		hash.Add ("easeType", iTween.EaseType.easeOutCubic);
		hash.Add ("onupdate", "OnShadowUpdate");
		hash.Add ("onupdatetarget", gameObject);
		hash.Add ("oncomplete", "OnQuitComplete");
		hash.Add ("oncompletetarget", gameObject);
		iTween.ValueTo (shadow.gameObject, hash);
	}

	public	void OnQuitComplete() {
		shadow.gameObject.SetActive (false);
		OnQuit ();
	}
}
