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
	public	Button		buttonYes;
	public	Button		buttonNo;

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
	}

	public override void OnShowComplete () {
		base.OnShowComplete ();
		buttonYes.interactable = true;
		buttonNo.interactable = true;
	}

	public	void OnClickYes() {
		AudioPlayerComponent.Play ("fx_click");

		OnOK ();
	}

	public	void OnClickNo() {
		AudioPlayerComponent.Play ("fx_click");
		OnClose ();
	}
}
