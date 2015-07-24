using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Soomla.Store;

public class FootpackItemComponent : MonoBehaviour {

	public	Image				imageIcon;
	public	Text				textAmount;
	public	Text				textPrice;
    public  Button              buttonPrice;
    public  Color               colorNormal;
    public  Color               colorRed;
    public	SingleUsePackVG     pack;

	public	void SetItem(SingleUsePackVG sup) {
		pack = sup;
		textAmount.text = Utility.ToNumber(sup.GoodAmount) + " Pack";
		textPrice.text = Utility.ToNumber((sup.PurchaseType as PurchaseWithVirtualItem).Amount);

        bool afford = sup.CanAfford();
        textPrice.color = afford ? colorNormal:colorRed;
//        buttonPrice.interactable = afford;
	}

	public	void SetIcon(Sprite sprite) {
		imageIcon.sprite = sprite;
	}

	public	void OnClickIcon() {
        SendMessageUpwards ("OnClickFootPack", pack);
	}
}
