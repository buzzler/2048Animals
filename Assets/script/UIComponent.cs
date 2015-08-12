using UnityEngine;
using System.Collections;
using SmartLocalization;

public class UIComponent : MonoBehaviour {

	public	bool IsPopup;
	private UIState	_state = UIState.STOPPED;
	private	UIType	_parent;
	private	UIType	_child;
	protected bool	escapable;

	public	UIState state {
		get {
			return _state;
		}
	}

	public	UIType parent {
		get {
			return _parent;
		}
		set {
			_parent = value;
		}
	}

	public	UIType child {
		get {
			return _child;
		}
		set {
			_child = value;
		}
	}

	public	virtual	void OnUIStart() {
		if (_state!=UIState.START) {
			Observer.GetInstance().maskOpen += OnMaskOpen;
			Observer.GetInstance().maskClose += OnMaskClose;
			OnUIChangeLanguage(LanguageManager.Instance);
			LanguageManager.Instance.OnChangeLanguage += OnUIChangeLanguage;
			_state = UIState.START;
			gameObject.SetActive(true);
		}
	}

	public	virtual	void OnUIPause() {
		if (_state==UIState.START) {
			Observer.GetInstance().maskOpen -= OnMaskOpen;
			Observer.GetInstance().maskClose -= OnMaskClose;
			LanguageManager.Instance.OnChangeLanguage -= OnUIChangeLanguage;
			_state = UIState.PAUSED;
			gameObject.SetActive(false);
		}
	}

	public	virtual	void OnUIResume() {
		if (_state==UIState.PAUSED) {
			Observer.GetInstance().maskOpen += OnMaskOpen;
			Observer.GetInstance().maskClose += OnMaskClose;
			OnUIChangeLanguage(LanguageManager.Instance);
			LanguageManager.Instance.OnChangeLanguage += OnUIChangeLanguage;
			_state = UIState.START;
			gameObject.SetActive(true);
		}
	}

	public	virtual	void OnUIStop() {
		Observer.GetInstance().maskOpen -= OnMaskOpen;
		Observer.GetInstance().maskClose -= OnMaskClose;
		LanguageManager.Instance.OnChangeLanguage -= OnUIChangeLanguage;
		_state = UIState.STOPPED;
		gameObject.SetActive(false);
	}

	public	virtual	void OnUIReserve(UIType type) {
		SendMessageUpwards("ReserveNextUI", type);
	}

	public	virtual void OnUIChange() {
		SendMessageUpwards("ChangeImmediately");
	}

	public	virtual void OnUIBackward() {
		SendMessageUpwards("ChangeBackward");
	}

	public	virtual	void OnUIChangeLanguage(LanguageManager lm) {

	}

	private	void OnMaskOpen() {
		escapable = true;
	}

	private	void OnMaskClose() {
		escapable = false;
	}
}
