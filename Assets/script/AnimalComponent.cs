using UnityEngine;
using System.Collections;

public class AnimalComponent : MonoBehaviour {

	public	bool			animated;
	public	bool			dependLevel;
	public	bool			independent;
	public	AnimalType		type;
	public	Animator		themeAnimator;
	public	Animator		motion;
	public	AnimalSize		size;

	void OnEnable() {
		if (independent!=true) {
			PlayerInfo info = PlayerInfoManager.instance;
			Observer ob = Observer.GetInstance();

			type = info.lastAnimalType;
			ob.beatNormal += OnBeatNormal;
			ob.beatFever += OnBeatFever;
			ob.themeChange += OnChangeTheme;
			if (dependLevel) {
				ob.highLevelChange += OnChangeHighLevel;
				size = ConvertLevelToSize(info.highLevel);
			}

			SetTheme(type);
		} else {
			SetTheme(type);
		}
	}

	void OnDisable() {
		if (independent!=true) {
			type = AnimalType.NONE;
			Observer ob = Observer.GetInstance();
			ob.beatNormal -= OnBeatNormal;
			ob.beatFever -= OnBeatFever;
			ob.themeChange -= OnChangeTheme;
			if (dependLevel) {
				ob.highLevelChange -= OnChangeHighLevel;
			}
		}
	}

	public	AnimalSize ConvertLevelToSize(int level) {
		if (level < 4) {
			return AnimalSize.SMALL;
		} else if (level < 7) {
			return AnimalSize.MEDIUM;
		} else if (level < 10) {
			return AnimalSize.LARGE;
		} else {
			return AnimalSize.EXTRA;
		}
	}

	public	void OnChangeHighLevel(int level) {
		AnimalSize newSize = ConvertLevelToSize(level);
		if (size!=newSize) {
			size = newSize;
			SetTheme(type);
		}
	}

	public	void SetTheme(AnimalType type) {
		this.type = type;
		OnChangeTheme(ThemeInfo.Find(type));
	}

	public	void OnChangeTheme(ThemeInfo themeInfo) {
		if ((themeAnimator==null)||(themeInfo==null)) {
			return;
		}

		switch (size) {
		case AnimalSize.EXTRA:	themeAnimator.SetInteger("size", 160);	break;
		case AnimalSize.LARGE:	themeAnimator.SetInteger("size", 120);	break;
		case AnimalSize.MEDIUM:	themeAnimator.SetInteger("size", 80);	break;
		case AnimalSize.SMALL:	themeAnimator.SetInteger("size", 50);	break;
		}

		themeAnimator.SetTrigger(themeInfo.trigger);
	}

	public	void OnBeatNormal() {
		if (animated && isActiveAndEnabled) {
			motion.SetTrigger("trigger_beat");
		}
	}

	public	void OnBeatFever() {
		if (animated && isActiveAndEnabled) {
			motion.SetTrigger("trigger_fever");
		}
	}
}
