using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Soomla.Store;
using SmartLocalization;

public class GameFootOverlay : PopupOverlay {
	public  Text		textTitle;
	public  Text		textFoot;
	public  Text		textMessage;
	public  Text		textYes;
	public	Image		panel;
	public	RawImage	shadow;
	public	Button		buttonYes;
	public	Button		buttonNo;
	public	Vector3		fromPosition;
	public	Vector3		toPosition;
	public	Color		fromColor;
	public	Color		toColor;

	void OnEnable() {
		OnUIChangeLanguage (LanguageManager.Instance);
		LanguageManager.Instance.OnChangeLanguage += OnUIChangeLanguage;
		Observer.GetInstance().inventoryChange += OnUpdateInventory;
	}
	
	void OnDisable() {
		LanguageManager.Instance.OnChangeLanguage -= OnUIChangeLanguage;
		Observer.GetInstance().inventoryChange -= OnUpdateInventory;
	}
	
	private void OnUIChangeLanguage (SmartLocalization.LanguageManager lm) {
		textTitle.text = lm.GetTextValue("fnf.ui.gameover") + "?";
		textFoot.text = StoreInventory.GetItemBalance(StoreAssetInfo.FOOT).ToString();
		textMessage.text = lm.GetTextValue("fnf.ui.nomoremove") + "\n" + lm.GetTextValue("fnf.ui.paw");
		textYes.text = lm.GetTextValue("fnf.ui.yes");
	}
	
	private void OnUpdateInventory(string id, int balance, int delta) {
		if (id == StoreAssetInfo.FOOT) {
			textFoot.text = balance.ToString();
		}
	}

	public	void Question(OverlayEventHandler yesHandler, OverlayEventHandler noHandler) {
		onOK += yesHandler;
		onClose += noHandler;

		OnShow ();
//		panel.transform.position = fromPosition;
//		shadow.color = fromColor;
//
//		Hashtable hash = new Hashtable ();
//		hash.Add ("position", Vector3.zero);
//		hash.Add ("time", 0.5f);
//		hash.Add ("delay", 0.5f);
//		hash.Add ("easeType", iTween.EaseType.easeOutCubic);
//		hash.Add ("oncomplete", "OnQuestionComplete");
//		hash.Add ("oncompletetarget", gameObject);
//		iTween.MoveTo (panel.gameObject, hash);
//
//		hash = new Hashtable ();
//		hash.Add ("from", fromColor);
//		hash.Add ("to", toColor);
//		hash.Add ("time", 0.5f);
//		hash.Add ("delay", 0.5f);
//		hash.Add ("easeType", iTween.EaseType.easeOutCubic);
//		hash.Add ("onupdate", "OnColorUpdate");
//		hash.Add ("onupdatetarget", gameObject);
//		iTween.ValueTo (shadow.gameObject, hash);
	}

//	public	void OnColorUpdate(Color color) {
//		shadow.color = color;
//	}

//	public	void OnQuestionComplete() {
//		buttonYes.interactable = true;
//		buttonNo.interactable = true;
//	}

	public override void OnShowComplete () {
		base.OnShowComplete ();
		buttonYes.interactable = true;
		buttonNo.interactable = true;
	}

	public	void OnClickYes() {
		AudioPlayerComponent.Play ("fx_click");

		OnOK ();
//		buttonYes.interactable = false;
//		buttonNo.interactable = false;
//
//		Hashtable hash = new Hashtable ();
//		hash.Add ("position", fromPosition);
//		hash.Add ("time", 0.3f);
//		hash.Add ("easeType", iTween.EaseType.easeInCubic);
//		hash.Add ("oncomplete", "OnYesComplete");
//		hash.Add ("oncompletetarget", gameObject);
//		iTween.MoveTo (panel.gameObject, hash);
//		
//		hash = new Hashtable ();
//		hash.Add ("from", toColor);
//		hash.Add ("to", fromColor);
//		hash.Add ("time", 0.3f);
//		hash.Add ("easeType", iTween.EaseType.easeInCubic);
//		hash.Add ("onupdate", "OnColorUpdate");
//		hash.Add ("onupdatetarget", gameObject);
//		iTween.ValueTo (shadow.gameObject, hash);
	}

	public	void OnClickNo() {
		AudioPlayerComponent.Play ("fx_click");
		OnClose ();
//		buttonYes.interactable = false;
//		buttonNo.interactable = false;
//		
//		Hashtable hash = new Hashtable ();
//		hash.Add ("position", toPosition);
//		hash.Add ("time", 0.3f);
//		hash.Add ("easeType", iTween.EaseType.easeInCubic);
//		hash.Add ("oncomplete", "OnNoComplete");
//		hash.Add ("oncompletetarget", gameObject);
//		iTween.MoveTo (panel.gameObject, hash);
//		
//		hash = new Hashtable ();
//		hash.Add ("from", toColor);
//		hash.Add ("to", fromColor);
//		hash.Add ("time", 0.3f);
//		hash.Add ("easeType", iTween.EaseType.easeInCubic);
//		hash.Add ("onupdate", "OnColorUpdate");
//		hash.Add ("onupdatetarget", gameObject);
//		iTween.ValueTo (shadow.gameObject, hash);
	}

//	public	void OnYesComplete() {
//		OnOK ();
//	}
//
//	public	void OnNoComplete() {
//		OnCancel ();
//	}
}
