using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(Animator))]
public class MaskComponent : UIComponent {

	public	Animator	animator;
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
		animator.SetBool("bool_open", true);
	}

	public	void CloseMask() {
		currentState = MaskState.CLOSE;
		animator.SetBool("bool_open", false);
	}

	public	void OnOpenMask() {
		if (observer.maskOpen != null) {
			observer.maskOpen();
		}
	}

	public	void OnCloseMask() {
		if (observer.maskClose != null) {
			observer.maskClose();
		}
		OnUIChange ();
		Invoke ("OpenMask", 0.3f);
	}
}
