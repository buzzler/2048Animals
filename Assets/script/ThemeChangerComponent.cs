using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Soomla;
using Soomla.Store;

public class ThemeChangerComponent : MonoBehaviour
{
	public	MaskComponent	ui_mask_back;
	public	ThemeInfo[]		themes;
	private	GameObject		background;
	private	PlayerInfo		info;
	private	Observer		observer;
	private	AnimalType		reserved;

	public	TextAsset		assetTheme;
	public	TextAsset		assetStore;

	void Start() {
		info = PlayerInfoKeeper.GetInstance().playerInfo;
		observer = Observer.GetInstance();
		reserved = AnimalType.NONE;

		List<string> category = new List<string>();
//		List<string> catPrice= new List<string>();
		string[][] grid = CSVReader.SplitCsvJaggedGrid(assetTheme.text);
		int count = grid.Length-1;		// without header

		themes = new ThemeInfo[count];
		for (int y = 0 ; y < count ; y++) {
			ThemeInfo t = ThemeInfo.Parse(grid[y+1]);
			themes[y] = t;
			ThemeInfo.Resister(t);
			StoreAssetInfo.Register(new EquippableVG(EquippableVG.EquippingModel.GLOBAL, t.name, t.description, t.id, new PurchaseWithVirtualItem(StoreAssetInfo.COIN, t.coin)));
//			StoreAssetInfo.Register(new EquippableVG(EquippableVG.EquippingModel.GLOBAL, t.name, t.description, t.productId, new PurchaseWithMarket(t.productId, t.price)));
//			StoreAssetInfo.Register(new VirtualCategory(t.categoryName, new List<string>(new string[] {t.id, t.productId})));
			category.Add(t.id);
//			catPrice.Add(t.productId);
		}
		StoreAssetInfo.Register(new VirtualCategory("cat_theme", category));
//		StoreAssetInfo.Register(new VirtualCategory("cat_price", catPrice));

		category = new List<string>();
		grid = CSVReader.SplitCsvJaggedGrid(assetStore.text);
		count = grid.Length-1;		// without header
		for (int y = 0 ; y < count ; y++) {
			StoreInfo sinfo = StoreInfo.Parse(grid[y+1]);
			StoreInfo.Resister(sinfo);
			StoreAssetInfo.Register(new VirtualCurrencyPack(sinfo.name, sinfo.description, sinfo.id, sinfo.amount, StoreAssetInfo.COIN, new PurchaseWithMarket(sinfo.productId, sinfo.price)));
			category.Add(sinfo.id);
		}
		StoreAssetInfo.Register(new VirtualCategory("cat_coin", category));

		SoomlaStore.Initialize(new StoreAssetInfo());
		SetTheme(info.type);
	}

	public	void SetTheme(AnimalType type, bool newBackground = true) {
		ThemeInfo selected = ThemeInfo.Find(type);
		if (newBackground) {
			background = GameObject.Instantiate(Resources.Load("bg/" + selected.bg.ToString().ToLower()), Vector3.zero, Quaternion.identity) as GameObject;
			background.GetComponent<BackgroundComponent>().SetStatusNormal();
		}

		info.type = type;

		PlayerInfoKeeper.GetInstance().Save();
		if (observer.themeChange!=null) {
			observer.themeChange(selected);
		}
	}

	public	void ChangeTheme(AnimalType type) {
		ThemeInfo current = ThemeInfo.Find(info.type);
		ThemeInfo selected = ThemeInfo.Find(type);

		if ((current==selected)||(selected==null)||(current==null)) {
			return;
		}

		if (current.bg!=selected.bg) {
			GameObject.Destroy(background);
			background = null;
			SetTheme(type, true);
		} else {
			SetTheme(type, false);
		}
	}

	public	void ReserveTheme(AnimalType type) {
		reserved = type;
		observer.maskClose += OnMaskClose;
		ui_mask_back.CloseMask ();
	}

	public	void OnMaskClose() {
		if (reserved != AnimalType.NONE) {
			observer.maskClose -= OnMaskClose;
			ChangeTheme(reserved);
		}
	}
}