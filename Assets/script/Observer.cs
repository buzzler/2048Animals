using UnityEngine;
using System.Collections;

public class Observer
{
	private	static Observer instance;
	public	static Observer GetInstance() {
		if (instance==null) {
			instance = new Observer();
		}
		return instance;
	}

	public	DelegateBeat		beat;
	public	DelegateBeatNormal	beatNormal;
	public	DelegateBeatFever	beatFever;
	public	DelegateThemeChange themeChange;
	public	DelegateHighLevel	highLevelChange;
	public	DelegateChangeInventory	inventoryChange;
	public	DelegateCurrencyChange currencyChange;
	public	DelegateMaskOpen	maskOpen;
	public	DelegateMaskClose	maskClose;
}

public	delegate void DelegateBeat(float time);
public	delegate void DelegateBeatNormal();
public	delegate void DelegateBeatFever();
public	delegate void DelegateThemeChange(ThemeInfo theme);
public	delegate void DelegateHighLevel(int level);
public	delegate void DelegateChangeInventory(string id, int balance);
public	delegate void DelegateMaskClose();
public	delegate void DelegateMaskOpen();
public	delegate void DelegateCurrencyChange(int balance, int delta);