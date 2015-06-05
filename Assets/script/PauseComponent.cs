using UnityEngine;
using System.Collections;

public class PauseComponent : UIComponent {

	public	void OnClickClose() {
		SendMessageUpwards ("PlayFx", "fx_click");
		OnUIReserve(parent);
		OnUIBackward ();
	}

	public	void OnClickChange() {
		SendMessageUpwards ("PlayFx", "fx_click");
		OnUIReserve (UIType.TITLE);
		OnUIChange ();
	}

	public	void OnClickRetry() {
		SendMessageUpwards ("PlayFx", "fx_click");
		OnUIReserve (UIType.GAME);
		OnUIChange ();
	}
}
