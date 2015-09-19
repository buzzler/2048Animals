using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AnimalThemeComponent : MonoBehaviour {
	public	AnimalSize	size;
	public	bool		highlevel;
	private	Image[]		targets;
	private	string		path;
	private	Sprite[]	sprites;

	void Awake() {
		targets = GetComponentsInChildren<Image> ();
	}

	void OnEnable() {
		if (highlevel) {
			OnHighLevel (1);
			Observer.GetInstance ().highLevelChange += OnHighLevel;
		} else {
			GenerateSprite();
		}
	}

	void OnDisable() {
		if (highlevel) {
			Observer.GetInstance ().highLevelChange -= OnHighLevel;
		}
		path = null;
		sprites = null;
		CancelInvoke ();
	}

	public	void OpenEyes() {
		foreach (Image img in targets) {
			img.sprite = sprites[0];
		}
		Invoke ("CloseEyes", 0.8f);
	}

	public	void CloseEyes() {
		foreach (Image img in targets) {
			img.sprite = sprites[1];
		}
		Invoke ("OpenEyes", 0.2f);
	}

	private	void OnHighLevel(int level) {
		if (level < 4) {
			size = AnimalSize.SMALL;
		} else if (level < 7) {
			size = AnimalSize.MEDIUM;
		} else if (level < 10) {
			size = AnimalSize.LARGE;
		} else {
			size = AnimalSize.EXTRA;
		}
		GenerateSprite ();
	}

	private	void GenerateSprite() {
		ThemeInfo info = PlayerInfoManager.instance.GetThemeInfo ();
		path = "icon/icon_" + info.code + "_" + ConvertSize () + "_0";
		
		// normal case
		sprites = new Sprite[2];
		sprites[0] = CachedResource.Load<Sprite> (path + "1");
		sprites[1] = CachedResource.Load<Sprite> (path + "2");
		CancelInvoke ();
		OpenEyes ();
		
//		sprites = new Sprite[4];
//		sprites[0] = CachedResource.Load<Sprite> (path + "1");
//		sprites[1] = CachedResource.Load<Sprite> (path + "2");
//		sprites[2] = CachedResource.Load<Sprite> (path + "3");
//		sprites[3] = CachedResource.Load<Sprite> (path + "4");
	}

	private	string ConvertSize() {
		switch (size) {
		case AnimalSize.EXTRA:
			return "160";
		case AnimalSize.LARGE:
			return "120";
		case AnimalSize.MEDIUM:
			return "80";
		case AnimalSize.SMALL:
			return "50";
		default:
			return "50";
		}
	}
}
