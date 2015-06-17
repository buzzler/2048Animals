using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CoreComponent : MonoBehaviour {
	public	const int ROW		= 4;
	public	const int COLUMN	= 4;
	public	BoxComponent[]	prefabs;
	public	SlotComponent[]	slots;
	public	Transform[]		fx1;
	public	Transform[]		fx_coin;
	public	Transform[]		fx_combo;
	public	int				combo_threshold;
	private Dictionary<string, BoxComponent> boxes;
	private int				count;
	public	int				combo;
	public	bool			fever;
	public	float			timeLastCombo;
	public	float			timeDuration;
	public	float			timeFever;
	
	public	Image			speaker;
	public	Sprite			speakerNormal;
	public	Sprite			speakerBoom;
	private	Observer		observer;

	void Start() {
		Init();
	}

	void OnEnable() {
		if (observer==null) {
			observer = Observer.GetInstance();
		}
		observer.beat += OnBeat;
	}

	void OnDisable() {
		if (observer==null) {
			observer = Observer.GetInstance();
		}
		observer.beat -= OnBeat;
	}

	public	void OnBeat(float time) {
		if (isActiveAndEnabled) {
			speaker.sprite = (speaker.sprite==speakerBoom) ? speakerNormal:speakerBoom;
//			speaker.sprite	= speakerBoom;
//			Invoke("OnBoom", time/4f);
			if (fever && (observer.beatFever!=null)) {
				observer.beatFever();
			} else {
				observer.beatNormal();
			}
		}
	}

//	public	void OnBoom() {
//		speaker.sprite = speakerNormal;
//	}

	private void Init() {
		boxes = new Dictionary<string, BoxComponent>();
		count = 0;
		combo = 0;
		fever = false;
	}

	public BoxComponent New(int level = 0, SlotComponent slot = null) {
		level = Mathf.Clamp(level, 0, prefabs.Length-1);

		// set position
		if (slot==null) {
			ArrayList list = new ArrayList();
			foreach(SlotComponent s in slots) {
				if (s.IsEmpty) {
					list.Add(s);
				}
			}
			if (list.Count==0) {
				SendMessageUpwards("GameOver");
				return null;
			}
			slot = list[(Random.Range(0, list.Count-1))] as SlotComponent;
		}

		// instantiate box
		count++;
		BoxComponent box = Instantiate(prefabs[level], slot.transform.position, Quaternion.identity) as BoxComponent;
		box.transform.SetParent(transform);
		box.id = count.ToString();
		box.level = level;
		box.SetSlot(slot);
		slot.ReserveHold(box);
		boxes.Add(box.id, box);
		
		// set animal type

		return box;
	}

	public void Clear() {
		foreach (SlotComponent slot in slots) {
			slot.Clear();
		}
		if (boxes!=null) {
			foreach (BoxComponent box in boxes.Values) {
				GameObject.Destroy(box.gameObject);
			}
			boxes.Clear();
		}
		Init();
	}

	public void Left() {
		int count = 0;
		if (IsMovable()) {
			for (int x = 0 ; x < COLUMN ; x++) {
				for (int y = 0 ; y < ROW ; y++) {
					BoxComponent box = slots[y*COLUMN+x].box;
					if (box) {
						if (box.Left()) {
							count++;
						}
					}
				}
			}
		}
		CheckCombo(count);
	}

	public void Right() {
		int count = 0;
		if (IsMovable()) {
			for (int x = COLUMN-1 ; x >= 0 ; x--) {
				for (int y = 0 ; y < ROW ; y++) {
					BoxComponent box = slots[y*COLUMN+x].box;
					if (box) {
						if (box.Right()) {
							count++;
						}
					}
				}
			}
		}
		CheckCombo(count);
	}

	public void Up() {
		int count = 0;
		if (IsMovable()) {
			for (int y = 0 ; y < ROW ; y++) {
				for (int x = 0 ; x < COLUMN ; x++) {
					BoxComponent box = slots[y*COLUMN+x].box;
					if (box) {
						if (box.Up()) {
							count++;
						}
					}
				}
			}
		}
		CheckCombo(count);
	}

	public void Down() {
		int count = 0;
		if (IsMovable()) {
			for (int y = ROW-1 ; y >= 0 ; y--) {
				for (int x = COLUMN-1 ; x >= 0 ; x--) {
					BoxComponent box = slots[y*COLUMN+x].box;
					if (box) {
						if (box.Down()) {
							count++;
						}
					}
				}
			}
		}
		CheckCombo(count);
	}

	private bool IsMovable() {
		foreach (BoxComponent box in boxes.Values) {
			if (box.moving) {
				return false;
			}
		}
		return true;
	}

	private	void CheckCombo(int count) {
		if (fever) {
			return;
		}

		float timeDelta = Time.time - timeLastCombo;
		timeLastCombo = Time.time;

		if ((count == 0) || (timeDelta > timeDuration)) {
			combo = 0;
			return;
		}

		combo++;
		if (combo >= combo_threshold) {
			fever = true;
			SendMessageUpwards("FeverOn");
			Invoke("OnFeverComplete", timeFever);
		}
	}

	private	void OnFeverComplete() {
		combo = 0;
		fever = false;
		SendMessageUpwards("FeverOff");
	}

	public void OnMerge(SlotComponent slot) {
		BoxComponent box1 = slot.box;
		BoxComponent box2 = slot.target;

		try {
			int level = box1.level+1;
			if (level >= prefabs.Length) {
				DebugComponent.Error("biggest box can't merge");
				return;
			}

			slot.Clear();
			boxes.Remove(box1.id);
			boxes.Remove(box2.id);
			GameObject.DestroyImmediate(box1.gameObject);
			GameObject.DestroyImmediate(box2.gameObject);
			New(level, slot);
			// insert score increament
			if (fever) {
				SendMessageUpwards("AppendScore", (uint)Mathf.Pow(2, level+2));
			} else {
				SendMessageUpwards("AppendScore", (uint)Mathf.Pow(2, level+1));
			}

			Vector3 pos = slot.transform.position;

			// insert effect 'bang'
			if (fx1[level]!=null) {
				Instantiate(fx1[level], pos, Quaternion.identity);
			}

			// insert coin increment
			if (fx_coin[level]!=null) {
				SendMessageUpwards ("PlayFx", "fx_coin");
				SendMessageUpwards("AppendCoin", 1);
				Instantiate(fx_coin[level], pos, Quaternion.identity);
			}

			// insert effect 'combo'
			if (combo>0) {
				SendMessageUpwards ("PlayFx", "fx_combo");
				int fx = Mathf.Min(combo, fx_combo.Length-1);
				if (fx_combo[fx]!=null) {
					Instantiate(fx_combo[fx], pos, Quaternion.identity);
				}
			}

			if (level==(prefabs.Length-1)) {
				SendMessageUpwards("FeverOff");
				SendMessageUpwards("Win");
			} 
		} catch (UnityException error) {
			DebugComponent.Error("something wrong on merging (slot id:" +slot.id+ ") " + error.Message);
		} finally {
			OnMoved(slot);
		}
	}

	public void OnMoved(SlotComponent slot) {
		if (IsMovable()) {
			RandomNew();

			bool gameover = true;
			foreach (SlotComponent s in slots) {
				if (s.IsEmpty || s.IsNeighbor()) {
					gameover = false;
					break;
				}
			}

			if (gameover) {
				SendMessageUpwards("FeverOff");
				SendMessageUpwards("GameOver");
			}
		}
	}

	/**
	 * for debug
	 */
	public void RandomNew() {
		New(Random.value<0.05 ? 1:0);
	}

	void Update() {
		if (Input.GetKeyDown("left")) {
			Left();
		}
		if (Input.GetKeyDown("right")) {
			Right();
		}
		if (Input.GetKeyDown("up")) {
			Up();
		}
		if (Input.GetKeyDown("down")) {
			Down();
		}
	}
	/**
	 * for debug
	 */
}
