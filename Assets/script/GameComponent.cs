using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Soomla;
using Soomla.Store;

public class GameComponent : UIComponent {
	public	CoreComponent		core;
	public	Text				labelBest;
	public	Text				textBest;
	public	Text				textScore;
	public	GameObject			fever;
    public  RawImage            rawShadow;
    public  RawImage            rawSpeaker;
	public	GameStartOverlay	overlayStart;
	public	GameFootOverlay		overlayFoot;
	public	GameEraseOverlay	overlayErase;
	public	GameResultOverlay	overlayResult;
    private Texture2D           textureNormal;
    private Texture2D           textureBoom;
	private PlayerInfo			playerInfo;
	private	ThemeInfo			themeInfo;
	private	uint				score;
	private	BackgroundComponent	bg;
	private	Observer			observer;

	void Update() {
	}

	public override void OnUIChangeLanguage (SmartLocalization.LanguageManager lm) {
		base.OnUIChangeLanguage (lm);
		labelBest.text = lm.GetTextValue ("fnf.ui.best");
	}

	public	override void OnUIStart() {
		base.OnUIStart();
		playerInfo = PlayerInfoManager.instance;
		bg = GameObject.FindObjectOfType<BackgroundComponent>();
		observer = Observer.GetInstance();
        observer.beat += OnBeat;

        themeInfo = playerInfo.GetThemeInfo();
        textureNormal = Resources.Load<Texture2D>("bg/" + themeInfo.bg.ToString().ToLower()+"_1");
        textureBoom = Resources.Load<Texture2D>("bg/" + themeInfo.bg.ToString().ToLower()+"_2");
        rawSpeaker.texture = textureNormal;

		// change medium image to selected animal's one
		ClearScore();
		ClearCoin();
		core.Clear();
		SendMessageUpwards("TurnOffFilter");

		GameStartOverlay overlay = GameObject.Instantiate<GameStartOverlay>(overlayStart);
		overlay.transform.SetParent(transform, false);
		overlay.Ready(OnPop);
	}

	public	override void OnUIStop() {
		base.OnUIStop();
        observer.beat -= OnBeat;
		FeverOff();
		SendMessageUpwards("TurnOnFilter");
	}

    public  void OnBeat(float time) {
        if (isActiveAndEnabled) {
            rawSpeaker.texture = (rawSpeaker.texture == textureBoom) ? textureNormal:textureBoom;
            if (fever && (observer.beatFever!=null)) {
                observer.beatFever();
            } else {
                observer.beatNormal();
            }
        }
    }

	public	void OnPop() {
		Invoke("OnInvoke", 0.1f);
	}

	public	void OnInvoke() {
		core.RandomNew();
		core.RandomNew();
	}

	public	void ClearScore() {
		playerInfo.highLevel = 0;
		score = 0;
		UpdateScore();
        if (observer.highLevelChange != null) {
            observer.highLevelChange(0);
        }
	}

	public	bool AppendScore(int level) {
		score += (uint)playerInfo.buffInfoScore.Calculate(Mathf.Pow(2, fever.activeSelf ? level+1:level));
		UpdateScore();

		level += 1;
		if (((level)>playerInfo.highLevel) && (observer.highLevelChange!=null)) {
			playerInfo.highLevel = level;
			observer.highLevelChange(level);
		}

		if (themeInfo.star <= level) {
			if ((playerInfo.stars[themeInfo.order] < level) || (level == 12)) {
				return true;
			} else {
				return false;
			}
		} else {
			return false;
		}
	}

	public	void UpdateScore() {
		textBest.text = playerInfo.bestScore.ToString();
		textScore.text = score.ToString();
	}

	public	void ClearCoin() {
		playerInfo.gameCoin = 0;
	}

	public	void AppendCoin(int delta = 110) {
		playerInfo.gameCoin += (int)playerInfo.buffInfoCoin.Calculate((float)delta);
		StoreInventory.GiveItem(StoreAssetInfo.COIN, delta);
	}

    public  void NoMoreMove() {
		GameFootOverlay overlay = GameObject.Instantiate<GameFootOverlay>(overlayFoot);
		overlay.transform.SetParent(transform, false);
		overlay.Question (OnClickYes, GameOver);
    }

	public	void GameOver() {
		OnWinOverPrefare().GameOver(OnWinOverComplete);
	}

	public	void Win() {
		OnWinOverPrefare().Victory(OnWinOverComplete);
	}

	public	GameResultOverlay OnWinOverPrefare() {
		SaveScore();
		FeverOff();
		SendMessageUpwards("ReserveNextUI", UIType.RESULT);
		GameResultOverlay overlay = GameObject.Instantiate<GameResultOverlay>(overlayResult);
		overlay.transform.SetParent(transform, false);
		return overlay;
	}

	public	void OnWinOverComplete() {
		GetComponentInParent<SystemCheckComponent>().TakeScreenshot(OnUIChange);
//		OnUIChange ();
	}

	public	void FeverOn() {
		fever.SetActive(true);
		bg.SetStatusFever();
	}

	public	void FeverOff() {
		fever.SetActive(false);
		bg.SetStatusNormal();
	}

	public	void FeverToggle() {
		if (fever.activeSelf) {
			FeverOff();
		} else {
			FeverOn();
		}
	}

	private	void SaveScore() {
		playerInfo.gameScore = score;
	}

    public  void OnClickYes() {
		if (core.GetBoxCount() < 3) {
			return;
		} else {
			Debug.Log("BOX:"+core.GetBoxCount().ToString());
		}

		GameEraseOverlay overlay = GameObject.Instantiate<GameEraseOverlay>(overlayErase);
		overlay.transform.SetParent(transform, false);
		overlay.Erase (rawShadow);

		Button[] buttons = core.GetComponentsInChildren<Button> ();
		foreach (Button b in buttons) {
			b.interactable = true;
		}
    }

	public	void OnErase(SlotComponent slot) {
		Button[] buttons = core.GetComponentsInChildren<Button> ();
		foreach (Button b in buttons) {
			b.interactable = false;
		}
		StoreInventory.TakeItem (StoreAssetInfo.FOOT, 1);
        AnalyticsComponent.LogItemEvent(AnalyticsComponent.ACTION_USE, 1);
		AudioPlayerComponent.Play ("fx_foot");

		GameEraseOverlay overlay = GetComponentInChildren<GameEraseOverlay> ();
		if (overlay != null) {
			overlay.Quit ();
		}
	}
}
