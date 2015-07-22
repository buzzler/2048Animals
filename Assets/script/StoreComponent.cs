using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Soomla;
using Soomla.Store;

public class StoreComponent : MonoBehaviour {
	void Start () {
		StoreEvents.OnSoomlaStoreInitialized		+= onSoomlaStoreInitialized;
		StoreEvents.OnCurrencyBalanceChanged		+= onCurrencyBalanceChanged;
		StoreEvents.OnGoodBalanceChanged			+= onGoodBalanceChanged;
		StoreEvents.OnMarketPurchaseStarted			+= onMarketPurchaseCancelled;
		StoreEvents.OnMarketPurchase				+= onMarketPurchase;
		StoreEvents.OnMarketPurchaseCancelled		+= onMarketPurchaseCancelled;
		StoreEvents.OnMarketRefund					+= onMarketRefund;
		StoreEvents.OnMarketItemsRefreshStarted		+= onMarketItemsRefreshStarted;
		StoreEvents.OnMarketItemsRefreshFinished	+= onMarketItemsRefreshFinished;
		StoreEvents.OnRestoreTransactionsStarted	+= onRestoreTransactionsStarted;
		StoreEvents.OnRestoreTransactionsFinished	+= onRestoreTransactionsFinished;
		StoreEvents.OnItemPurchaseStarted			+= onItemPurchaseStarted;
		StoreEvents.OnItemPurchased					+= onItemPurchased;
		StoreEvents.OnGoodEquipped					+= onGoodEquipped;
		StoreEvents.OnGoodUnEquipped				+= onGoodUnequipped;
		StoreEvents.OnGoodUpgrade					+= onGoodUpgrade;
		StoreEvents.OnBillingSupported				+= onBillingSupported;
		StoreEvents.OnBillingNotSupported			+= onBillingNotSupported;
		StoreEvents.OnUnexpectedErrorInStore		+= onUnexpectedErrorInStore;
	}
	
	private	void onSoomlaStoreInitialized() {
		DebugComponent.Log("STORE Initialized");
	}

	private void onCurrencyBalanceChanged(VirtualCurrency currency, int balance, int delta) {
		DebugComponent.Log("CURRENCY ("+ currency.Name + ") : " + balance.ToString() + "(" + (delta>0?"+":"") + delta.ToString()+ ")");
        Observer ob = Observer.GetInstance ();
		if (ob.currencyChange != null) {
            ob.currencyChange (balance, delta);
        }
	}

	private	void onGoodBalanceChanged(VirtualGood good, int balance, int delta) {
		DebugComponent.Log("GOOD ("+ good.Name + ") : " + balance.ToString() + "(" + (delta>0?"+":"") + delta.ToString()+ ")");
        Observer ob = Observer.GetInstance ();
        if (ob.inventoryChange != null) {
		    ob.inventoryChange(good.ItemId, balance);
        }
	}

	private	void onMarketPurchaseStarted(PurchasableVirtualItem pvi) {
		DebugComponent.Log("PURCHASING..");
	}

	public void onMarketPurchase(PurchasableVirtualItem pvi, string payload, Dictionary<string, string> extra) {
		DebugComponent.Log("PURCHASED");
	}

	public void onMarketPurchaseCancelled(PurchasableVirtualItem pvi) {
		DebugComponent.Log("PURCHASE CANCELLED");
	}

	public void onMarketRefund(PurchasableVirtualItem pvi) {
		DebugComponent.Log("REFUND");
	}

	public void onMarketItemsRefreshStarted() {
	}

	public void onMarketItemsRefreshFinished(List<MarketItem> items) {
	}

	public void onRestoreTransactionsStarted() {
	}

	public void onRestoreTransactionsFinished(bool success) {
	}

	public void onItemPurchaseStarted(PurchasableVirtualItem pvi) {
		DebugComponent.Log("ITEM PURCHASING..");
	}

	public void onItemPurchased(PurchasableVirtualItem pvi, string payload) {
		DebugComponent.Log("ITEM PURCHASED");
	}

	public void onGoodEquipped(EquippableVG good) {
		DebugComponent.Log("GOOD ("+ good.Name +") EQUIPPED");
	}

	public void onGoodUnequipped(EquippableVG good) {
		DebugComponent.Log("GOOD ("+ good.Name +")UNEQUIPPED");
	}

	public void onGoodUpgrade(VirtualGood good, UpgradeVG currentUpgrade) {
	}

	public void onBillingSupported() {
		DebugComponent.Log("BILLING SUPPORTED");
	}

	public void onBillingNotSupported() {
		DebugComponent.Log("BILLING NOT SUPPORTED");
	}

	public void onUnexpectedErrorInStore(string message) {
		DebugComponent.Error(message);
	}
}
