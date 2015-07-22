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
        textAmount.text = Utility.ToNumber(vcp.CurrencyAmount);
        textPrice.text = Utility.ToCurrency((vcp.PurchaseType as PurchaseWithMarket).MarketItem.Price);
	}

	public	void SetIcon(Sprite sprite) {
		imageIcon.sprite = sprite;
	}

	public	void OnClickIcon() {
		SendMessageUpwards ("OnClickCoinPack", currencypack);
	}
}
