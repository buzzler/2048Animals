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
//	private	int					_balItem;

	public	bool SetGetAnimalType(PlayerInfo info) {
		theme = ThemeInfo.Find((AnimalType)System.Enum.Parse(typeof(AnimalType), GetComponent<Image>().sprite.name, true));
//		_balItem		= StoreInventory.GetItemBalance(theme.id);

//		if ((double)theme.costAmount==0.0f) {
//			if (_balItem==0) {
//				StoreInventory.GiveItem(theme.id, 1);
//			}
//			Unlocked();
//		}
//		else if (_balItem>0) {
//			Unlocked();
//		} else {
//			Blinded();
//		}

		int balance = StoreInventory.GetItemBalance(theme.id);
		if ((theme.costType==CostType.FREE) || (balance>0)) {
			switch (theme.buffInfo.type) {
			case BuffType.COIN:
				info.buffInfoCoin = BuffInfo.Max(info.buffInfoCoin, theme.buffInfo);
				break;
			case BuffType.REWARD:
				info.buffInfoReward = BuffInfo.Max(info.buffInfoReward, theme.buffInfo);
				break;
			case BuffType.SCORE:
				info.buffInfoScore = BuffInfo.Max(info.buffInfoScore, theme.buffInfo);
				break;
			}

			Unlocked();
		} else {
			Blinded();
		}
		return (theme.type==info.type);
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
		if (_state != ThemeSelectorState.UNLOCKED) {
			_state = ThemeSelectorState.UNLOCKED;
			animator.SetTrigger("trigger_unlocked");
		}
	}

	public	void Blinded() {
		if (_state != ThemeSelectorState.BLINDED) {
			_state = ThemeSelectorState.BLINDED;
			animator.SetTrigger("trigger_blinded");
		}
	}

	public	void Locked() {
		if (_state != ThemeSelectorState.LOCKED) {
			_state = ThemeSelectorState.LOCKED;
			animator.SetTrigger("trigger_locked");
		}
	}
}
