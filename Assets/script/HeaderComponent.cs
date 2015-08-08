using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Soomla;
using Soomla.Store;

public class HeaderComponent : UIComponent {

	public	PlayerInfo	info;
	public	Text		labelCoin;
	public	Text		labelFoot;
	public	Animator	animatorCoin;

	private	int			lastCoin;

	void Start() {
		info = PlayerInfoManager.instance;
	}

	void OnEnable() {
		int coin = StoreInventory.GetItemBalance(StoreAssetInfo.COIN);
        labelCoin.text = Utility.ToNumber(coin);
		lastCoin = coin;
		int foot = StoreInventory.GetItemBalance (StoreAssetInfo.FOOT);
        labelFoot.text = Utility.ToNumber(foot);
		Observer ob = Observer.GetInstance();
		ob.currencyChange += OnCurrencyChange;
		ob.inventoryChange += OnInventoryChange;
	}

	void OnDisable() {
		Observer ob = Observer.GetInstance();
		ob.currencyChange -= OnCurrencyChange;
		ob.inventoryChange -= OnInventoryChange;
	}

	public	void OnCurrencyChange(int balance, int delta) {
		if (balance != lastCoin) {
			if (balance>lastCoin) {
				animatorCoin.SetTrigger("increase");
			}
            labelCoin.text = Utility.ToNumber(balance);
			lastCoin = balance;
		}
	}

	public	void OnInventoryChange(string id, int balance, int delta) {
		if (id == StoreAssetInfo.FOOT) {
            labelFoot.text = Utility.ToNumber(balance);
		}
	}

    public  void OnClickFoot() {
        AudioPlayerComponent.Play ("fx_click");

		GameComponent gc = GetComponentInParent<GameComponent>();
		int balance = StoreInventory.GetItemBalance(StoreAssetInfo.FOOT);
		if ((gc != null) && (balance > 0)) {
			gc.OnClickYes ();
		} else {
			OnUIReserve(UIType.FOOTPACK);
			OnUIChange();
		}
    }

	public	void OnClickCoin() {
		AudioPlayerComponent.Play ("fx_click");
		OnUIReserve(UIType.COINPACK);
		OnUIChange();
	}

	public	void OnClickSetting() {
		AudioPlayerComponent.Play ("fx_click");
		OnUIReserve(UIType.SETTING);
		OnUIChange();
	}

	public	void OnClickPause() {
		AudioPlayerComponent.Play ("fx_click");
		OnUIReserve(UIType.PAUSE);
		OnUIChange();
	}
}
