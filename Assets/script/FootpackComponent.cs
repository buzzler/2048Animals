using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Soomla.Store;

public class FootpackComponent : UIComponent {
	public	GridLayoutGroup			grid;
	public	Text					labelBuy;
	public	RectTransform			content;
	public	FootpackItemComponent	item;
	public	Sprite[]				icons;

	public override void OnUIChangeLanguage (SmartLocalization.LanguageManager lm) {
		base.OnUIChangeLanguage (lm);
		labelBuy.text = lm.GetTextValue ("fnf.ui.buy");
	}

	public	override void OnUIStart() {
		base.OnUIStart();
        float count = 0;
        foreach (PackInfo info in PackInfo.footInfos.Values) {
            SingleUsePackVG sup = StoreInfo.GetItemByItemId(info.id) as SingleUsePackVG;
            FootpackItemComponent child = Instantiate(item) as FootpackItemComponent;
            int index = Mathf.FloorToInt((float)icons.Length * (count / (float)PackInfo.footInfos.Count));
            child.SetItem(sup);
            child.SetIcon(icons[index]);
            child.transform.SetParent(grid.transform);
            child.transform.localScale = new Vector3(1,1,1);
            count += 1f;
        }
        float rows = Mathf.Ceil((float)PackInfo.footInfos.Count / (float)grid.constraintCount);
		float h = grid.padding.top + grid.padding.bottom + grid.spacing.y * (rows-1) + grid.cellSize.y * rows;
		(grid.transform as RectTransform).sizeDelta = new Vector2 (500, h);
        Observer.GetInstance().inventoryChange += OnUpdateInventory;
	}

	public	override void OnUIStop() {
		base.OnUIStop ();
        for (int i = grid.transform.childCount ; i > 0 ; i--) {
            Transform child = grid.transform.GetChild(0);
            child.SetParent(null);
            Destroy(child.gameObject);
        }
        Observer.GetInstance().inventoryChange -= OnUpdateInventory;
	}

	public	void OnClickClose() {
		AudioPlayerComponent.Play ("fx_click");
		OnUIReserve(parent);
		OnUIBackward ();
	}

	public	void OnClickFootPack(SingleUsePackVG sup) {
		AudioPlayerComponent.Play ("fx_click");
		StoreInventory.BuyItem (sup.ItemId);
	}

    private void OnUpdateInventory(string id, int balance, int delta = 0) {
        if ((id == StoreAssetInfo.FOOT) && (balance > 0)) {
            AnalyticsComponent.LogItemEvent(AnalyticsComponent.ACTION_PURCHASE, delta);
            OnUIReserve(parent);
            OnUIBackward ();
        }
    }
}
