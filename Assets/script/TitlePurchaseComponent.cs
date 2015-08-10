using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Soomla.Store;

public class TitlePurchaseComponent : UIComponent {
	public	Text		textName;
	public	Text		textDescription;
	public	Button		buttonCoin;
	public	Button		buttonConnect;
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
//		if ((info != null) && StoreInventory.CanAfford (info.id)) {
		if ((info !=null) && (info.costAmount <= StoreInventory.GetItemBalance(StoreAssetInfo.COIN))) {
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
		buttonConnect.gameObject.SetActive (false);
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
			textName.gameObject.SetActive (true);
			textDescription.text = lm.GetTextValue (info.description);
			textDescription.gameObject.SetActive (true);

			if (info.costType==CostType.COIN) {
				textCoin.text = info.costAmount.ToString ();
				buttonCoin.interactable = interactable;
				buttonCoin.gameObject.SetActive (true);
				buttonConnect.gameObject.SetActive(false);
			} else if (info.costType==CostType.CONNECT) {
				buttonConnect.gameObject.SetActive(true);
				buttonCoin.gameObject.SetActive (false);
			}
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

	public	void OnClickConnect() {
#if UNITY_EDITOR || UNITY_ANDROID || UNITY_IOS
		OnUIReserve(UIType.CONNECT);
		OnUIChange();
#endif
	}
}
