﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PsedoGIF : MonoBehaviour {
	public	Image			target;
	public	Sprite[]		sprites;
	public	int				indexStart;
	public	float			fpsStart;
	public	float			fpsEnd;
	public	float			fpsCurrent;
	public	CoreComponent	core;

	private	void IntervalFunc() {
		indexStart = (indexStart+1) % sprites.Length;
		target.sprite = sprites[indexStart];

		Invoke("IntervalFunc", 1f/fpsCurrent);
	}

	void OnEnable() {
		target = GetComponent<Image>();
		target.sprite = sprites[indexStart];
		fpsCurrent = fpsStart;

		Hashtable hash = new Hashtable ();
		hash.Add("name", "fps");
		hash.Add ("from", fpsStart);
		hash.Add ("to", fpsEnd);
		hash.Add ("time", core.timeFever);
		hash.Add ("onupdate", "OnUpdate");
		hash.Add ("onupdatetarget", gameObject);
		iTween.ValueTo (gameObject, hash);

		IntervalFunc ();
	}

	void OnDisable() {
		CancelInvoke ();
//		iTween.StopByName ("fps");
		fpsCurrent = fpsStart;
	}

	public	void OnUpdate(float value) {
		fpsCurrent = value;
	}
}
