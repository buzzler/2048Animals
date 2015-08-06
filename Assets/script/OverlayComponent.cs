using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public	delegate void OverlayEventHandler();

public class OverlayComponent : MonoBehaviour {
	public	Graphic		objectShadow;
	public	GameObject	objectPanel;
	public	Color		colorHide;
	public	Color		colorShow;
	public	Vector3		positionUp;
	public	Vector3		positionDown;
	public	float		timeShow;
	public	float		timeShowDelay;
	public	float		timeHide;
	public	float		timeHideDelay;

	public	event OverlayEventHandler onShow;
	public	event OverlayEventHandler onShowComplete;
	public	event OverlayEventHandler onOK;
	public	event OverlayEventHandler onCancel;
	public	event OverlayEventHandler onClose;
	public	event OverlayEventHandler onHide;
	public	event OverlayEventHandler onHideComplete;

	private	int	selected;

	public	virtual void OnShow() {
		if (onShow!=null) {
			onShow();
		}
	}

	public	virtual void OnShowComplete() {
		if (onShowComplete!=null) {
			onShowComplete();
		}
	}

	public	virtual void OnOK() {
		selected = 1;
		OnHide (positionUp);
	}

	public	virtual void OnCancel() {
		selected = 2;
		OnHide (positionUp);
	}

	public	virtual void OnClose() {
		selected = 3;
		OnHide (positionDown);
	}

	public	virtual void OnHide(Vector3 direction) {
		if (onHide != null) {
			onHide();
		}
	}

	public	virtual void OnHideComplete() {
		switch (selected) {
		case 1:
			if (onOK != null) onOK(); break;
		case 2:
			if (onCancel != null) onCancel(); break;
		case 3:
			if (onClose != null) onClose(); break;
		}

		if (onHideComplete != null) {
			onHideComplete();
		}
		OnQuit ();
	}

	public	virtual void OnQuit() {
		GameObject.Destroy(gameObject);
	}
}
