using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MaskComponent : UIComponent {

	public	Image		mask;
	public	MaskState	defaultState;
	private	MaskState	currentState;
	private	Observer	observer;

	void Start() {
		currentState = defaultState;
		observer = Observer.GetInstance ();
	}

	public	bool IsOpened {
		get {
			return (currentState==MaskState.OPEN);
		}
	}

	public	MaskState maskState {
		get {
			return currentState;
		}
	}

	public	void OpenMask() {
		currentState = MaskState.OPEN;

		if (mask.rectTransform.sizeDelta.x != 2300f) {
			Hashtable hash = new Hashtable ();
			hash.Add ("from", 0f);
			hash.Add ("to", 2300f);
			hash.Add ("time", 0.5f);
			hash.Add ("easeType", iTween.EaseType.easeInCubic);
			hash.Add ("oncomplete", "OnOpenMask");
			hash.Add ("oncompletetarget", gameObject);
			hash.Add ("onupdate", "OnMaskUpdate");
			hash.Add ("onupdatetarget", gameObject);
			iTween.ValueTo (mask.gameObject, hash);
		}
	}

	public	void OnMaskUpdate(float value) {
		mask.rectTransform.sizeDelta = new Vector2 (value, value);
	}

	public	void CloseMask() {
		currentState = MaskState.CLOSE;

		if (mask.rectTransform.sizeDelta.x != 0f) {
			mask.gameObject.SetActive (true);
			Hashtable hash = new Hashtable ();
			hash.Add ("from", 2300f);
			hash.Add ("to", 0f);
			hash.Add ("time", 0.25f);
			hash.Add ("easeType", iTween.EaseType.easeInCubic);
			hash.Add ("oncomplete", "OnCloseMask");
			hash.Add ("oncompletetarget", gameObject);
			hash.Add ("onupdate", "OnMaskUpdate");
			hash.Add ("onupdatetarget", gameObject);
			iTween.ValueTo (mask.gameObject, hash);
		}
	}

	public	void OnOpenMask() {
		if (observer.maskOpen != null) {
			observer.maskOpen();
		}
		mask.gameObject.SetActive (false);
	}

	public	void OnCloseMask() {
		Invoke ("OnWaitDelay", 0.3f);
	}

	public	void OnWaitDelay() {
		if (observer.maskClose != null) {
			observer.maskClose();
		}
		OnUIChange ();
		Invoke ("OpenMask", 0.3f);
	}
}
