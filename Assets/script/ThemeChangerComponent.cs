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
	private	ThemeInfo		reserved;

	public	TextAsset		assetTheme;
	public	TextAsset		assetStore;
    public  TextAsset       assetFoot;

	void Start() {
		info = PlayerInfoManager.instance;
		observer = Observer.GetInstance();

		List<string> category = new List<string>();
		string[][] grid = CSVReader.SplitCsvJaggedGrid(assetTheme.text);
		int count = grid.Length-1;		// without header

        // initialize theme
		themes = new ThemeInfo[count];
		for (int y = 0 ; y < count ; y++) {
			ThemeInfo t = ThemeInfo.Parse(grid[y+1]);
			themes[y] = t;
			ThemeInfo.Resister(t);
			StoreAssetInfo.Register(new EquippableVG(EquippableVG.EquippingModel.GLOBAL, t.name, t.description, t.id, new PurchaseWithVirtualItem(StoreAssetInfo.COIN, t.costAmount)));
			category.Add(t.id);
		}
		StoreAssetInfo.Register(new VirtualCategory("cat_theme", category));

        // initialize coinpack
		category = new List<string>();
		grid = CSVReader.SplitCsvJaggedGrid(assetStore.text);
		count = grid.Length-1;		// without header
		for (int y = 0 ; y < count ; y++) {
            PackInfo sinfo = PackInfo.Parse(grid[y+1]);
            PackInfo.Resister(sinfo);
			StoreAssetInfo.Register(new VirtualCurrencyPack(sinfo.name, sinfo.description, sinfo.id, sinfo.amount, StoreAssetInfo.COIN, new PurchaseWithMarket(sinfo.productId, sinfo.price)));
			category.Add(sinfo.id);
		}
		StoreAssetInfo.Register(new VirtualCategory("cat_coin", category));

        // initialize footpack
        StoreAssetInfo.Register(new SingleUseVG("foot", "break a jelly", StoreAssetInfo.FOOT, new PurchaseWithVirtualItem(StoreAssetInfo.COIN, 1000)));
        category = new List<string>();
        grid = CSVReader.SplitCsvJaggedGrid(assetFoot.text);
        count = grid.Length-1;      // without header
        for (int y = 0 ; y < count ; y++) {
            PackInfo sinfo = PackInfo.Parse(grid[y+1]);
            PackInfo.Resister(sinfo);
            StoreAssetInfo.Register(new SingleUsePackVG(StoreAssetInfo.FOOT, sinfo.amount, sinfo.name, sinfo.description, sinfo.id, new PurchaseWithVirtualItem(StoreAssetInfo.COIN, (int)sinfo.price)));
//            StoreAssetInfo.Register(new SingleUsePackVG(StoreAssetInfo.FOOT, sinfo.amount, sinfo.name, sinfo.description, sinfo.id, new PurchaseWithMarket(sinfo.productId, sinfo.price)));
            category.Add(sinfo.id);
        }
        StoreAssetInfo.Register(new VirtualCategory("cat_foot", category));

		SoomlaStore.Initialize(new StoreAssetInfo());
		SetTheme(info.lastAnimalType);
	}

	public	void SetTheme(AnimalType type, bool newBackground = true) {
		info.lastAnimalType = type;
		ThemeInfo selected = ThemeInfo.Find(type);
		if (newBackground) {
			background = GameObject.Instantiate(Resources.Load("bg/" + selected.bg.ToString().ToLower()), Vector3.zero, Quaternion.identity) as GameObject;
			background.GetComponent<BackgroundComponent>().SetStatusNormal();
		}

		PlayerInfoManager.Save();
		if (observer.themeChange!=null) {
			observer.themeChange(selected);
		}
	}

	public	void ChangeTheme(ThemeInfo selected) {
		ThemeInfo current = ThemeInfo.Find(info.lastAnimalType);

		if ((current==selected)||(selected==null)||(current==null)) {
			return;
		}

		if (current.bg!=selected.bg) {
			GameObject.Destroy(background);
			background = null;
			SetTheme(selected.type, true);
		} else if (current.bgNormal!=selected.bgNormal) {
			background.GetComponent<BackgroundComponent>().SetTheme(selected.bgNormal);
			SetTheme(selected.type, false);
		}
		else {
			SetTheme(selected.type, false);
		}
	}

	public	void ReserveTheme(ThemeInfo info) {
		reserved = info;
		observer.maskClose += OnMaskClose;
		ui_mask_back.CloseMask ();
	}

	public	void OnMaskClose() {
		if (reserved != null) {
			observer.maskClose -= OnMaskClose;
			ChangeTheme(reserved);
			reserved = null;
		}
	}
}