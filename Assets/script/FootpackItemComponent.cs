using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Soomla.Store;

public class FootpackItemComponent : MonoBehaviour {

	public	Image				imageIcon;
	public	Text				textAmount;
	public	Text				textPrice;
    public	SingleUsePackVG     pack;

	public	void SetItem(SingleUsePackVG sup) {
		pack = sup;
		textAmount.text = sup.GoodAmount.ToString () + " Pack";
		textPrice.text = "$" + (sup.PurchaseType as PurchaseWithMarket).MarketItem.Price.ToString ();
	}

	public	void SetIcon(Sprite sprite) {
		imageIcon.sprite = sprite;
	}

	public	void OnClickIcon() {
		SendMessageUpwards ("OnClickFootPack", pack);
	}
}
