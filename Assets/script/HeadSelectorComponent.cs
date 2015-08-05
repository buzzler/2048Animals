using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Button))]
public class HeadSelectorComponent : MonoBehaviour {
	public	int level;
	public	int star;
	public	HeadSelectorComponent[] group;
	public	Sprite spriteDisable;
	public	Sprite spriteSelected;
	private Observer	observer;
	private	PlayerInfo	playerInfo;
	private	Button		button;

	void Awake() {
		observer = Observer.GetInstance();
		playerInfo = PlayerInfoManager.instance;
		button = GetComponent<Button>();
	}

	void OnEnable() {
		OnChangeTheme(null);
		observer.themeChange += OnChangeTheme;
	}

	void OnDisable() {
		observer.themeChange -= OnChangeTheme;
	}

	private	void OnChangeTheme(ThemeInfo info) {
		if (star==0) {
			ChangeToPressed();

		} else {
			ChangeToReleased();
		}
	}

	private	void ChangeToPressed() {
		SpriteState ss = button.spriteState;
		ss.disabledSprite = spriteSelected;
		button.spriteState = ss; 
		button.interactable = false;
	}

	private	void ChangeToReleased() {
		SpriteState ss = button.spriteState;
		ss.disabledSprite = spriteDisable;
		button.spriteState = ss;
		int star = playerInfo.stars[playerInfo.GetThemeInfo().order];
		button.interactable = (star>=this.star);
	}

	public	void OnClick() {
		AudioPlayerComponent.Play("fx_click");
		ChangeToPressed();
		foreach (HeadSelectorComponent hsc in group) {
			hsc.OnClickOther();
		}
		SendMessageUpwards("OnClickLevel", level);
	}

	public	void OnClickOther() {
		ChangeToReleased();
	}
}
