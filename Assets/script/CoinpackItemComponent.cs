using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Soomla.Store;

public class CoinpackItemComponent : MonoBehaviour {

	public	Image				imageIcon;
	public	Text				textAmount;
	public	Text				textPrice;
	public	VirtualCurrencyPack	currencypack;

	public	void SetItem(VirtualCurrencyPack vcp) {
		currencypack = vcp;
		textAmount.text = vcp.CurrencyAmount.ToString ();
		textPrice.text = "$" + (vcp.PurchaseType as PurchaseWithMarket).MarketItem.Price.ToString ();
	}

	public	void SetIcon(Sprite sprite) {
		imageIcon.sprite = sprite;
	}

	public	void OnClickIcon() {
		SendMessageUpwards ("OnClickCoinPack", currencypack);
	}
}
