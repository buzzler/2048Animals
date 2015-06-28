using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Soomla.Store;

public class TitlePurchaseComponent : UIComponent {
	public	Text		textName;
	public	Text		textDescription;
	public	Button		buttonCoin;
	public	Text		textCoin;
	private	ThemeInfo	info;
	
	void OnEnable() {
		Observer.GetInstance ().currencyChange += OnCurrencyChange;
	}

	void OnDisable() {
		Observer.GetInstance ().currencyChange -= OnCurrencyChange;
	}

	public	void OnCurrencyChange(int balance, int delta) {
		CheckAvailability ();
	}

	private bool CheckAvailability() {
		if ((info != null) && StoreInventory.CanAfford (info.id)) {
			textCoin.color = Color.white;
			return true;
		} else {
			textCoin.color = Color.red;
			return false;
		}
	}

	public	void ClearThemeInfo() {
		textName.gameObject.SetActive(false);
		textDescription.gameObject.SetActive(false);
		buttonCoin.gameObject.SetActive(false);
		this.info = null;
	}

	public	void Refresh() {
		SetThemeInfo (this.info, buttonCoin.interactable);
	}

	public	void SetThemeInfo(ThemeInfo info, bool interactable = true) {
		this.info = info;

		if (this.info != null) {
			SmartLocalization.LanguageManager lm = SmartLocalization.LanguageManager.Instance;
			textName.text = lm.GetTextValue (info.name);
			textDescription.text = lm.GetTextValue (info.description);
			textCoin.text = info.coin.ToString ();

			textName.gameObject.SetActive (true);
			textDescription.gameObject.SetActive (true);
			buttonCoin.gameObject.SetActive (true);
			buttonCoin.interactable = interactable;
		}
		CheckAvailability ();
	}

	public	void OnClick() {
		if (CheckAvailability ()) {
			SendMessageUpwards ("OnClickBuy", info);
		} else {
			OnUIReserve(UIType.COINPACK);
			OnUIChange();
		}
	}
}
