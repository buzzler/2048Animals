using UnityEngine;
using System.Collections;

public class SlotComponent : MonoBehaviour {
	public	string			id;
	public	SlotComponent	UP;
	public	SlotComponent	DOWN;
	public	SlotComponent	LEFT;
	public	SlotComponent	RIGHT;

	private	BoxComponent hold;
	private BoxComponent merger;

	public	void Clear() {
		hold = null;
		merger = null;
	}

	public	BoxComponent box {
		get {
			return hold;
		}
	}

	public	BoxComponent target {
		get {
			return merger;
		}
	}

	public	bool IsEmpty {
		get {
			return (hold==null && merger==null);
		}
	}

	public	bool IsMergable(BoxComponent box) {
		if ((!IsEmpty) && (merger==null)) {
			return (hold.level==box.level);
		}
		return false;
	}

	public	bool IsNeighbor() {
		if (LEFT) {
			if (LEFT.IsEmpty) {
				return true;
			} else if (LEFT.box.level==box.level){
				return true;
			}
		}
		if (RIGHT) {
			if (RIGHT.IsEmpty) {
				return true;
			} else if (RIGHT.box.level==box.level){
				return true;
			}
		}
		if (UP) {
			if (UP.IsEmpty) {
				return true;
			} else if (UP.box.level==box.level){
				return true;
			}
		}
		if (DOWN) {
			if (DOWN.IsEmpty) {
				return true;
			} else if (DOWN.box.level==box.level){
				return true;
			}
		}
		return false;
	}

	public	void SetPosition(BoxComponent box) {
		if (IsEmpty) {
			hold = box;
			hold.transform.localPosition = transform.localPosition;
		}
	}

	public	void ReserveHold(BoxComponent box) {
		if (IsEmpty) {
			hold = box;

			//translate box

		}
	}

	public	void ReserveMerger(BoxComponent box) {
		if (IsMergable(box)) {
			merger = box;

			//translate box
		}
	}

	public	SlotComponent MostLeft(BoxComponent box) {
		if (LEFT!=null) {
			if (LEFT.IsEmpty) {
				return LEFT.MostLeft(box);
			} else if (LEFT.IsMergable(box)) {
				return LEFT;
			} else {
				return this;
			}
		} else {
			return this;
		}
	}

	public	SlotComponent MostRight(BoxComponent box) {
		if (RIGHT!=null) {
			if (RIGHT.IsEmpty) {
				return RIGHT.MostRight(box);
			} else if (RIGHT.IsMergable(box)) {
				return RIGHT;
			} else {
				return this;
			}
		} else {
			return this;
		}
	}

	public	SlotComponent MostUp(BoxComponent box) {
		if (UP!=null) {
			if (UP.IsEmpty) {
				return UP.MostUp(box);
			} else if (UP.IsMergable(box)) {
				return UP;
			} else {
				return this;
			}
		} else {
			return this;
		}
	}

	public	SlotComponent MostDown(BoxComponent box) {
		if (DOWN!=null) {
			if (DOWN.IsEmpty) {
				return DOWN.MostDown(box);
			} else if (DOWN.IsMergable(box)) {
				return DOWN;
			} else {
				return this;
			}
		} else {
			return this;
		}
	}

	public	void HandleMove(BoxComponent box) {
		if (merger!=null) {
			SendMessageUpwards("OnMerge", this);
		} else if (hold!=null) {
			SendMessageUpwards("OnMoved", this);
		} else {
			// error !
			DebugComponent.Error("????? (id:" + id+ ")");
		}
	}
}
