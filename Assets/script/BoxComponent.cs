using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class BoxComponent : MonoBehaviour {

	public	string id;
	public	int level;
	private	SlotComponent current;
	private	Animator animator;
	private	bool tween;

	void Start() {
		animator = GetComponent(typeof(Animator)) as Animator;
		animator.SetTrigger("trigger_init");
	}

	public bool moving {
		get {
			return tween;
		}
	}

	private bool Move(SlotComponent slot) {
		if (current!=slot) {
			if (slot.IsEmpty) {
				slot.ReserveHold(this);
				SetSlot(slot);
			} else if (slot.IsMergable(this)) {
				slot.ReserveMerger(this);
				SetSlot(slot);
			} else {
				DebugComponent.Error("maybe wrong box moving logic (id:" + id + ")");
				return false;
			}
			Hashtable hash = new Hashtable();
			hash.Add("position", slot.transform.position);
			hash.Add("speed", 5);
			hash.Add("easetype", iTween.EaseType.linear);
			hash.Add("oncompletetarget", gameObject);
			hash.Add("oncomplete", "HandleMove");
			tween = true;
			iTween.MoveTo(gameObject, hash);
			return true;
		}
		return false;
	}

	public bool Left() {
		if (current) {
			if (Move (current.MostLeft(this))) {
				animator.SetTrigger("trigger_left");
			} else {
				return false;
			}
		} else {
			DebugComponent.Error("slot reference missing (id:" + id + ")");
		}
		return true;
	}

	public bool Right() {
		if (current) {
			if (Move (current.MostRight(this))) {
				animator.SetTrigger("trigger_right");
			} else {
				return false;
			}
		} else {
			DebugComponent.Error("slot reference missing (id:" + id + ")");
		}
		return true;
	}

	public bool Up() {
		if (current) {
			if (Move (current.MostUp(this))) {
				animator.SetTrigger("trigger_up");
			} else {
				return false;
			}
		} else {
			DebugComponent.Error("slot reference missing (id:" + id + ")");
		}
		return true;
	}

	public bool Down() {
		if (current) {
			if (Move (current.MostDown(this))) {
				animator.SetTrigger("trigger_down");
			} else {
				return false;
			}
		} else {
			DebugComponent.Error("slot reference missing (id:" + id + ")");
		}
		return true;
	}

	public void SetSlot(SlotComponent slot) {
		if (current!=null) {
			current.Clear();
		}
		current = slot;
	}

	public void HandleMove() {
		animator.SetTrigger ("trigger_stop");
		tween = false;
		current.HandleMove(this);
	}

	public	void OnClick() {
		SendMessageUpwards ("OnErase", current);
	}
}
