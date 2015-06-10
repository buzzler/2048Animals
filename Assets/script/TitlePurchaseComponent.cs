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
		Observer.GetInstance ().currencyChange += OnCurrentChange;
	}

	void OnDisable() {
		Observer.GetInstance ().currencyChange -= OnCurrentChange;
	}

	public	void OnCurrentChange(int balance, int delta) {
		CheckAvailability ();
	}

	private bool CheckAvailability() {
		if (StoreInventory.CanAfford (info.id)) {
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
	}

	public	void SetThemeInfo(ThemeInfo info, bool interactable = true) {
		this.info = info;
		textName.text = info.name;
		textDescription.text = info.description;
		textCoin.text = info.coin.ToString();

		textName.gameObject.SetActive(true);
		textDescription.gameObject.SetActive(true);
		buttonCoin.gameObject.SetActive(true);
		buttonCoin.interactable = interactable;

		CheckAvailability ();
	}

	public	void OnClick() {
		SendMessageUpwards ("PlayFx", "fx_click");
		if (CheckAvailability ()) {
			SendMessageUpwards ("OnClickBuy", info);
		} else {
			OnUIReserve(UIType.COINPACK);
			OnUIChange();
		}
	}
}
