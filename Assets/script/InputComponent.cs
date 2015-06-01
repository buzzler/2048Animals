using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(CoreComponent))]
public class InputComponent : MonoBehaviour {
	public	Slider	slider;
	private CoreComponent core;
	private float	thresholdInch;	// in inch
	private float	threshold;		// in pixel
	private float	distance;		// in pixel
	private Vector2	pivot;
	private bool	pressed;

	void Start () {
		core = GetComponent(typeof(CoreComponent)) as CoreComponent;
		sensitivity = 0.4f;
		ClearInput();
	}

	void Update () {
		if (Input.touchCount > 0) {
			Touch t = Input.GetTouch(0);
			switch (t.phase) {
			case TouchPhase.Began:
				if (!pressed) {
					pivot = t.position;
					distance = 0;
					pressed = true;
				}
				break;
			case TouchPhase.Ended:
				if (pressed)
					CheckInput(t.position);
				break;
			case TouchPhase.Moved:
				if (pressed)
					CheckInput(t.position);
				break;
			}

		} else {
			ClearInput();
		}
	}

	public	float sensitivity {
		set {
			thresholdInch = value;
			threshold = (Screen.dpi>0) ? Screen.dpi * thresholdInch:300*thresholdInch;
			ClearInput();
		}

		get {
			return thresholdInch;
		}
	}

	private	void ClearInput() {
		pivot = Vector2.zero;
		distance = 0;
		pressed = false;
	}

	private	void CheckInput(Vector2 position) {
		distance = Vector2.Distance(pivot, position);
		if (distance > threshold) {
			Vector2 direct = position - pivot;
			if (Mathf.Abs(direct.x) <= Mathf.Abs(direct.y)) {
				if (direct.y > 0) {
					core.Up();
				} else {
					core.Down();
				}
			} else {
				if (direct.x > 0) {
					core.Right();
				} else {
					core.Left();
				}
			}
			ClearInput();
		}
	}

	/**
	 * for debug
	 */
	public void SliderHandler() {
		sensitivity = slider.value;
		DebugComponent.Log("SENSITIVITY: " + sensitivity.ToString());
	}
}
