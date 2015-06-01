using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GyroComponent : MonoBehaviour {
	public float max_x;
	public float max_y;
	public float focus;
	public float followrate;
	public Slider focus_slider;
	public Slider follow_slider;

	private Dictionary<Transform, Vector2> centers;
	private int fix;
	private int back_start;
	private int back_end;
	private int back_count;
	private int front_start;
	private int front_end;
	private int front_count;
	public void Refocus() {
		int total = transform.childCount;
		int min = 0;
		int max = Mathf.Max(total-1,0);

		fix = (int)Mathf.Round((float)total * Mathf.Clamp(focus,0,1));
		fix = Mathf.Clamp(fix, min, max);
		back_start = min;
		back_end = Mathf.Max(fix-1, min);
		back_count = Mathf.Clamp(back_end - back_start + 1, 0, total);
		front_start = Mathf.Min(fix+1, max);
		front_end = max;
		front_count = Mathf.Clamp(front_end - front_start + 1, 0, total);
	}

	void Start() {
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		Input.gyro.enabled = true;

		centers = new Dictionary<Transform, Vector2>();
		for (int ci = 0 ; ci < transform.childCount ; ci++) {
			Transform child = transform.GetChild(ci);
			centers.Add(child, child.localPosition);
		}
		Refocus();
	}

	void Update () {
		Vector3 g = Input.gyro.gravity * 2;
		int ci;
		Transform child;
		Vector2 pos;
		float rate;

		for (ci = 0 ; ci < back_count ; ci++) {
			child = transform.GetChild(back_start+ci);
			pos = centers[child];
			rate = (float)(back_count-ci) / (float)back_count;
			pos.x -= Mathf.Clamp(g.x, -1, 1) * max_x * rate;
			pos.y -= Mathf.Clamp(g.y+1, -1, 1) * max_y * rate;
//			child.localPosition = pos;
			Vector2 local = child.localPosition;
			child.localPosition = local + (pos - local) * followrate;
		}

		child = transform.GetChild(fix);
		child.localPosition = centers[child];

		for (ci = 0 ; ci < front_count ; ci++) {
			child = transform.GetChild(front_start+ci);
			pos = centers[child];
			rate = (float)(ci+1) / (float)front_count;
			pos.x += Mathf.Clamp(g.x, -1, 1) * max_x * rate;
			pos.y += Mathf.Clamp(g.y+1, -1, 1) * max_y * rate;
//			child.localPosition = pos;
			Vector2 local = child.localPosition;
			child.localPosition = local + (pos - local) * followrate;
		}
	}

	/**
	 * for debug
	 */
	public void FocusSliderHandler() {
		focus = focus_slider.value;
		DebugComponent.Log("FOCUS: " + focus.ToString());
		Refocus();
	}

	public void FollowSliderHandler() {
		followrate = follow_slider.value;
		DebugComponent.Log("FOLLOW: " + followrate.ToString());
	}
}
