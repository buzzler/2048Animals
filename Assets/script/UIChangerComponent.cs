using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIChangerComponent : MonoBehaviour {

	public	MaskComponent					ui_mask_back;
	public	UIComponent						ui_preload;
	public	UIComponent						ui_title;
	public	UIComponent						ui_game;
	public	UIComponent						ui_result;
	public	UIComponent 					ui_setting;
	public	UIComponent						ui_coinpack;
	public	UIComponent						ui_tutorial;
	public	UIComponent						ui_pause;
	public	UIComponent						ui_connect;
	private Dictionary<UIType, UIComponent>	dictionary;
	private	UIType							current;
	private UIType							reserved;
	private bool							changeNow;
	private	bool							backwardNow;

	void Start() {
		dictionary = new Dictionary<UIType, UIComponent>();
		dictionary.Add(UIType.PRELOAD,	ui_preload);
		dictionary.Add(UIType.TITLE,	ui_title);
		dictionary.Add(UIType.GAME,		ui_game);
		dictionary.Add(UIType.RESULT,	ui_result);
		dictionary.Add(UIType.SETTING,	ui_setting);
		dictionary.Add(UIType.COINPACK,	ui_coinpack);
		dictionary.Add(UIType.TUTORIAL,	ui_tutorial);
		dictionary.Add(UIType.PAUSE,	ui_pause);
		dictionary.Add(UIType.CONNECT, ui_connect);
//		current = UIType.PRELOAD;
//		reserved = UIType.NONE;
		current = UIType.NONE;
		reserved = UIType.PRELOAD;
		changeNow = true;
		backwardNow = false;
//		ForcePreload();
	}

	void Update() {
		if (changeNow) {
			changeNow = false;
			Change();
		}
		if (backwardNow) {
			backwardNow = false;
			ChangeBackward();
		}
	}

	public void ReserveNextUI(UIType type) {
		reserved = type;
		if (type == UIType.NONE) {
			DebugComponent.Error("wrong UIType");
		}
	}
	
	public	void ChangeImmediately() {
		changeNow = true;
	}

	public	void ChangeBckward() {
		backwardNow = true;
	}

	private void ChangeBackward() {
		if (!dictionary.ContainsKey(current)) {
			return;
		}
		UIComponent cur = dictionary[current];
		if (!dictionary.ContainsKey(cur.parent)) {
			return;
		}
		UIComponent res = dictionary[cur.parent];
		cur.OnUIStop ();
		cur.parent = UIType.NONE;
		res.child = UIType.NONE;

		res.OnUIResume();
		current = reserved;
		reserved = UIType.NONE;
	}

	private void Change() {
		if (!dictionary.ContainsKey(reserved)) {
			return;
		}

		UIComponent res = dictionary[reserved];
		if ((current!=UIType.NONE) && (res.IsPopup != true) && ui_mask_back.IsOpened) {
			ui_mask_back.CloseMask();
			return;
		}

		if (dictionary.ContainsKey(current)) {
			UIComponent cur = dictionary[current];

			if (res.IsPopup) {
				cur.OnUIPause();
				res.parent = current;
				cur.child = reserved;
			} else {
				cur.OnUIStop();
				if (dictionary.ContainsKey(cur.parent)) {
					dictionary[cur.parent].OnUIStop();
				}
				res.parent = UIType.NONE;
				cur.child = UIType.NONE;
			}
		} else {
			ui_mask_back.OpenMask();
		}

		res.OnUIStart();
		current = reserved;
		reserved = UIType.NONE;
	}

	public void ClearAllUI() {
		ui_title.OnUIStop();
		ui_game.OnUIStop();
		ui_result.OnUIStop();
		ui_setting.OnUIStop();
		ui_coinpack.OnUIStop ();
		ui_tutorial.OnUIStop();
		ui_pause.OnUIStop();
		ui_connect.OnUIStop();

		current = UIType.NONE;
		reserved = UIType.NONE;
	}

	public	void ForceUI(UIType type) {
		foreach (UIType t in dictionary.Keys) {
			if (type!=t) {
				dictionary[t].OnUIStop();
			}
		}
		dictionary[type].OnUIStart();

		current = type;
		reserved = UIType.NONE;
	}

	public void ForcePreload() {
		ui_title.OnUIStop();
		ui_game.OnUIStop();
		ui_result.OnUIStop();
		ui_setting.OnUIStop();
		ui_coinpack.OnUIStop();
		ui_tutorial.OnUIStop();
		ui_pause.OnUIStop();
		ui_connect.OnUIStop();

		ui_preload.OnUIStart();
	}

	public void ForceTitle() {
		ui_preload.OnUIStop();
		ui_game.OnUIStop();
		ui_result.OnUIStop();
		ui_setting.OnUIStop();
		ui_coinpack.OnUIStop();
		ui_tutorial.OnUIStop();
		ui_pause.OnUIStop();
		ui_connect.OnUIStop();

		ui_title.OnUIStart();
	}

	public void ForceGame() {
		ui_preload.OnUIStop();
		ui_title.OnUIStop();
		ui_result.OnUIStop();
		ui_setting.OnUIStop();
		ui_coinpack.OnUIStop();
		ui_tutorial.OnUIStop();
		ui_pause.OnUIStop();
		ui_connect.OnUIStop();

		ui_game.OnUIStart();
	}

	public void ForceResult() {
		ui_preload.OnUIStop();
		ui_title.OnUIStop();
		ui_game.OnUIStop();
		ui_setting.OnUIStop();
		ui_coinpack.OnUIStop();
		ui_tutorial.OnUIStop();
		ui_pause.OnUIStop();
		ui_connect.OnUIStop();

		ui_result.OnUIStart();
	}
}
