using UnityEngine;
using System.Collections;

public class PauseComponent : UIComponent {

	public	void OnClickClose() {
		OnUIReserve(parent);
		OnUIBackward ();
	}

	public	void OnClickChange() {
		OnUIReserve (UIType.TITLE);
		OnUIChange ();
	}

	public	void OnClickRetry() {
		OnUIReserve (UIType.GAME);
		OnUIChange ();
	}
}
