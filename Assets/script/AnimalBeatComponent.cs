using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class AnimalBeatComponent : MonoBehaviour {
	private Animator animator;

	void Awake() {
		animator = GetComponent<Animator> ();
	}

	void OnEnable() {
		Observer ob = Observer.GetInstance();
		ob.beatNormal += OnBeatNormal;
		ob.beatFever += OnBeatFever;
	}

	void OnDisable() {
		Observer ob = Observer.GetInstance();
		ob.beatNormal -= OnBeatNormal;
		ob.beatFever -= OnBeatFever;
	}

	public	void OnBeatNormal() {
		if (isActiveAndEnabled) {
			animator.SetTrigger("trigger_beat");
		}
	}
	
	public	void OnBeatFever() {
		if (isActiveAndEnabled) {
			animator.SetTrigger("trigger_fever");
		}
	}
}
