using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Soomla;
using Soomla.Store;

public class HeaderComponent : UIComponent {

	public	PlayerInfo	info;
	public	Text		labelCoin;
	public	Animator	animatorCoin;

	private	int			lastCoin;

	void Start() {
		info = PlayerInfoKeeper.GetInstance().playerInfo;
	}

	void OnEnable() {
		int coin = StoreInventory.GetItemBalance(StoreAssetInfo.COIN);
		labelCoin.text = coin.ToString();
		lastCoin = coin;

		Observer ob = Observer.GetInstance();
		ob.currencyChange += OnCurrencyChange;
	}

	void OnDisable() {
		Observer ob = Observer.GetInstance();
		ob.currencyChange -= OnCurrencyChange;
	}

	public	void OnCurrencyChange(int balance, int delta) {
		if (balance != lastCoin) {
			if (balance>lastCoin) {
				animatorCoin.SetTrigger("increase");
			}
			labelCoin.text = balance.ToString();
			lastCoin = balance;
		}
	}

    public  void OnClickFoot() {
        AudioPlayerComponent.Play ("fx_click");
        OnUIReserve(UIType.FOOTPACK);
        OnUIChange();
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
