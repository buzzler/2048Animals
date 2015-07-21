using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Soomla;
using Soomla.Store;

public class StoreAssetInfo : IStoreAssets {
	public	const string COIN = "coin";
    public  const string FOOT = "foot";
	public	static VirtualCurrency 							currencyCoin	= new VirtualCurrency("Coin", "Main Currency", COIN);
	public	static List<VirtualCurrency>					listCurrency	= new List<VirtualCurrency>(new VirtualCurrency[] {currencyCoin});
	public	static List<VirtualCurrencyPack>				listCurrencyPack= new List<VirtualCurrencyPack>();
	public	static List<VirtualGood>						listGood		= new List<VirtualGood>();
	public	static List<VirtualCategory>					listCategory	= new List<VirtualCategory>();
	public	static Dictionary<string, VirtualCurrencyPack>	dicCurrencyPack	= new Dictionary<string, VirtualCurrencyPack>();
	public	static Dictionary<string, VirtualGood>			dicGood			= new Dictionary<string, VirtualGood>();
	public	static Dictionary<string, VirtualCategory>		dicCategory		= new Dictionary<string, VirtualCategory>();

	public	static void Register(VirtualCurrency vc) {
		listCurrency.Add(vc);
	}

	public	static void Register(VirtualCurrencyPack vcp) {
		listCurrencyPack.Add(vcp);
		dicCurrencyPack.Add(vcp.ItemId, vcp);
	}

	public	static void Register(VirtualGood vg) {
		listGood.Add(vg);
		dicGood.Add(vg.ItemId, vg);
	}

	public	static void Register(VirtualCategory vc) {
		listCategory.Add(vc);
		dicCategory.Add(vc.Name, vc);
	}

	public int 						GetVersion ()		{return 0;}
	public VirtualCurrency[]		GetCurrencies ()	{return listCurrency.ToArray();}
	public VirtualGood[]			GetGoods ()			{return listGood.ToArray();}
	public VirtualCurrencyPack[]	GetCurrencyPacks ()	{return listCurrencyPack.ToArray();}
	public VirtualCategory[]		GetCategories ()	{return listCategory.ToArray();}
}
