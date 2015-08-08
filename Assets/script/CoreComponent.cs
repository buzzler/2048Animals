using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CoreComponent : MonoBehaviour {
	public	const int ROW		= 4;
	public	const int COLUMN	= 4;
	public	BoxComponent[]	prefabs;
	public	SlotComponent[]	slots;
	public	EffectType[]	effectBang;
	public	EffectType[]	effectCoin;
	public	EffectType[]	effectCombo;
	
	private Dictionary<string, BoxComponent> boxes;
	private int				count;
	private	int				boxCount;
	public	int				combo;
	public	bool			fever;
	public	float			timeLastCombo;
	public	float			timeDuration;
	public	float			timeFever;
	public	bool			block;

	private	Observer		observer;
	private	GameComponent	game;

	void Start() {
		Init();
	}

	void OnDisable() {
		CancelInvoke ();
	}

	private void Init() {
		boxes = new Dictionary<string, BoxComponent>();
		count = 0;
		boxCount = 0;
		combo = 0;
		fever = false;
		block = false;
		game = GetComponentInParent<GameComponent> ();
	}

	public	int GetBoxCount() {
		return boxCount;
	}

	public void RandomNew() {
		New(Random.value<0.05 ? 1:0);
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
				DebugComponent.Error("nowhere found to create box");
				return null;
			}
			slot = list[(Random.Range(0, list.Count-1))] as SlotComponent;
		}

		// instantiate box
		count++;
		boxCount++;
//		BoxComponent box = Instantiate(prefabs[level], slot.transform.position, Quaternion.identity) as BoxComponent;
		BoxComponent box = ObjectPool.Spawn<BoxComponent> (prefabs[level], transform);
		box.transform.localPosition = slot.transform.localPosition;
//		box.transform.SetParent(transform);
//		box.id = count.ToString();
//		box.level = level;
//		box.SetSlot(slot);
		box.Init (count.ToString (), level, slot);
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
//			foreach (BoxComponent box in boxes.Values) {
//				GameObject.Destroy(box.gameObject);
//			}
			boxes.Clear();
			ObjectPool.RecycleAll();
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
		if (block) {
			return false;
		}
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
		if (combo >= effectCombo.Length) {
			fever = true;
			game.FeverOn();
			Invoke("OnFeverComplete", timeFever);
		}
	}

	private	void OnFeverComplete() {
		combo = 0;
		fever = false;
		game.FeverOff ();
	}

	public	void OnErase(SlotComponent slot) {
		BoxComponent box = slot.box;
		slot.Clear ();
		boxCount--;
		boxes.Remove(box.id);
//		GameObject.DestroyImmediate (box.gameObject);
		ObjectPool.Recycle<BoxComponent> (box);
		AudioPlayerComponent.Play ("fx_combo");
		EffectComponent.Show (effectBang [0], slot.transform.position);
		block = false;
	}

	public void OnMerge(SlotComponent slot) {
		BoxComponent box1 = slot.box;
		BoxComponent box2 = slot.target;

		int level = box1.level+1;
		if (level >= prefabs.Length) {
			DebugComponent.Error("biggest box can't merge");
			return;
		}

		slot.Clear();
		boxCount -= 2;
		boxes.Remove(box1.id);
		boxes.Remove(box2.id);
//		GameObject.DestroyImmediate(box1.gameObject);
//		GameObject.DestroyImmediate(box2.gameObject);
		ObjectPool.Recycle<BoxComponent> (box1);
		ObjectPool.Recycle<BoxComponent> (box2);
		New(level, slot);
		// insert score increament
		bool isWin = game.AppendScore (level);

		Vector3 pos = slot.transform.position;

		// insert effect 'bang'
		EffectComponent.Show (effectBang [level], pos);

		// insert coin increment
		if (EffectComponent.Show (effectCoin [level], pos) != null) {
			AudioPlayerComponent.Play ("fx_coin");
			game.AppendCoin();
		}

		// insert effect 'combo'
		if (EffectComponent.Show (effectCombo [(Mathf.Min(combo, effectCombo.Length-1))], pos) != null) {
			AudioPlayerComponent.Play ("fx_combo");
		}

		if (isWin) {
			game.Win();
			block = true;
		} else {
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
                game.NoMoreMove();
				block = true;
            }
		}
	}

	/**
	 * for debug
	 */

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
