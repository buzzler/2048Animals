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
	
	public	bool SetGetAnimalType(PlayerInfo info) {
		theme = ThemeInfo.Find((AnimalType)System.Enum.Parse(typeof(AnimalType), GetComponent<Image>().sprite.name, true));

		int balance = StoreInventory.GetItemBalance(theme.id);
		if (theme.costType==CostType.FREE) {
			Unlocked();
		} else if (theme.costType==CostType.CONNECT) {
			if (balance>0) {
				Unlocked();
			} else {
				Personal();
			}
		} else if ((theme.costType==CostType.COIN) && (balance>0)) {
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
		return (theme.type==info.lastAnimalType);
	}

	public	void OnClickButton() {
		switch (state) {
		case ThemeSelectorState.UNLOCKED:
		case ThemeSelectorState.LOCKED:
		case ThemeSelectorState.BLINDED:
		case ThemeSelectorState.PERSONAL:
			AudioPlayerComponent.Play ("fx_click");
			SendMessageUpwards("ReserveTheme", theme);
			break;
		}
	}

	public	void CheckState() {
		switch (_state) {
		case ThemeSelectorState.BLINDED:
			Blinded(true);
			break;
		case ThemeSelectorState.LOCKED:
			Locked(true);
			break;
		case ThemeSelectorState.PERSONAL:
			Personal(true);
			break;
		case ThemeSelectorState.UNLOCKED:
			Unlocked(true);
			break;
		}
	}

	public	void Unlocked(bool forced = false) {
		if (_state != ThemeSelectorState.UNLOCKED || forced) {
			_state = ThemeSelectorState.UNLOCKED;
			animator.SetTrigger("trigger_unlocked");
		}
	}

	public	void Blinded(bool forced = false) {
		if (_state != ThemeSelectorState.BLINDED || forced) {
			_state = ThemeSelectorState.BLINDED;
			animator.SetTrigger("trigger_blinded");
		}
	}

	public	void Locked(bool forced = false) {
		if (_state != ThemeSelectorState.LOCKED || forced) {
			_state = ThemeSelectorState.LOCKED;
			animator.SetTrigger("trigger_locked");
		}
	}

	public	void Personal(bool forced = false) {
		if (_state != ThemeSelectorState.PERSONAL || forced) {
			_state = ThemeSelectorState.PERSONAL;
			animator.SetTrigger("trigger_personal");
		}
	}
}
