using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Soomla;
using Soomla.Store;

[RequireComponent(typeof(Image))]
public class ThemeSelectorComponent : MonoBehaviour {
	public	ThemeInfo			theme;
	public	Animator			animator;
	public	ThemeSelectorState	state {get{return _state;}}
	private	ThemeSelectorState	_state;
	private	int					_balItem;

	void OnEnable() {
//		Observer ob = Observer.GetInstance();
//		ob.inventoryChange += OnUpdateInventory;
	}

	void OnDisable() {
//		Observer ob = Observer.GetInstance();
//		ob.inventoryChange -= OnUpdateInventory;
	}

//	public	void OnUpdateInventory(string id, int balance) {
//		if (id == theme.id) {
//			_balItem = balance;
//			if ((balance > 0) && (state == ThemeSelectorState.LOCKED)) {
//				Unlocked();
//				SendMessageUpwards("RefreshHead", this);
//				SendMessageUpwards("CheckNextUnlock");
//			}
//		}
//	}

	public	AnimalType SetGetAnimalType() {
		theme = ThemeInfo.Find((AnimalType)System.Enum.Parse(typeof(AnimalType), GetComponent<Image>().sprite.name, true));
		_balItem		= StoreInventory.GetItemBalance(theme.id);

		if ((double)theme.coin==0.0f) {
			if (_balItem==0) {
				StoreInventory.GiveItem(theme.id, 1);
			}
			Unlocked();
		}
		else if (_balItem>0) {
			Unlocked();
		} else {
			Blinded();
		}

		return theme.type;
	}

	public	void OnClickButton() {
		switch (state) {
		case ThemeSelectorState.UNLOCKED:
		case ThemeSelectorState.LOCKED:
		case ThemeSelectorState.BLINDED:
			SendMessageUpwards("OnClickSelector", this);
			break;
		}
	}

	public	void Unlocked() {
		_state = ThemeSelectorState.UNLOCKED;
		animator.SetTrigger("trigger_unlocked");
	}

	public	void Blinded() {
		_state = ThemeSelectorState.BLINDED;
		animator.SetTrigger("trigger_blinded");
	}

	public	void Locked() {
		_state = ThemeSelectorState.LOCKED;
		animator.SetTrigger("trigger_locked");
	}
}
