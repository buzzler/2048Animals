using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Soomla.Store;

public class CoinpackComponent : UIComponent {
	public	GridLayoutGroup			grid;
	public	Text					labelBuy;
	public	RectTransform			content;
	public	CoinpackItemComponent	item;
	public	Sprite[]				icons;

	public override void OnUIChangeLanguage (SmartLocalization.LanguageManager lm) {
		base.OnUIChangeLanguage (lm);
		labelBuy.text = lm.GetTextValue ("fnf.ui.buy");
	}

	public	override void OnUIStart() {
		base.OnUIStart();
		List<VirtualCurrencyPack> list = StoreAssetInfo.listCurrencyPack;
		float count = 0;
		foreach (VirtualCurrencyPack vcp in list) {
			CoinpackItemComponent child = Instantiate(item) as CoinpackItemComponent;
			int index = Mathf.FloorToInt((float)icons.Length * (count / (float)list.Count));
			child.SetItem(vcp);
			child.SetIcon(icons[index]);
			child.transform.SetParent(grid.transform);
			child.transform.localScale = new Vector3(1,1,1);
			count += 1f;
		}
		float rows = Mathf.Ceil((float)list.Count / (float)grid.constraintCount);
		float h = grid.padding.top + grid.padding.bottom + grid.spacing.y * (rows-1) + grid.cellSize.y * rows;
		(grid.transform as RectTransform).sizeDelta = new Vector2 (500, h);
        Observer.GetInstance().currencyChange += OnUpdateCurrency;
	}

	public	override void OnUIStop() {
		base.OnUIStop ();
        for (int i = grid.transform.childCount ; i > 0 ; i--) {
            Transform child = grid.transform.GetChild(0);
            child.SetParent(null);
            Destroy(child.gameObject);
        }
        Observer.GetInstance().currencyChange -= OnUpdateCurrency;
	}

	public	void OnClickClose() {
		AudioPlayerComponent.Play ("fx_click");
		OnUIReserve(parent);
		OnUIBackward ();
	}

	public	void OnClickCoinPack(VirtualCurrencyPack vcp) {
		AudioPlayerComponent.Play ("fx_click");
		if (SystemCheckComponent.network) {
			StoreInventory.BuyItem (vcp.ItemId);
		} else {
			OnUIReserve(UIType.ERROR);
			OnUIChange();
		}
	}

    private void OnUpdateCurrency(int balance, int delta) {
        if (delta > 0) {
            OnUIReserve(parent);
            OnUIBackward ();
        }
    }
}
