using UnityEngine;
using System.Collections;

public	delegate void OverlayEventHandler();

public class OverlayComponent : MonoBehaviour {
	public	event OverlayEventHandler onOK;
	public	event OverlayEventHandler onCancel;
	public	event OverlayEventHandler onClose;

	public	virtual void OnOK() {
		if (onOK!=null) {
			onOK();
		}
		OnQuit();
	}

	public	virtual void OnCancel() {
		if (onCancel!=null) {
			onCancel();
		}
		OnQuit();
	}

	public	virtual void OnClose() {
		if (onClose!=null) {
			onClose();
		}
		OnQuit();
	}

	public	virtual void OnQuit() {
		GameObject.Destroy(gameObject);
	}
}
