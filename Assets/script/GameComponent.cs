using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Soomla;
using Soomla.Store;

[RequireComponent (typeof(Animator))]
public class GameComponent : UIComponent {
	public	CoreComponent		core;
	public	Text				labelBest;
	public	Text				textBest;
	public	Text				textScore;
	public	GameObject			fever;
	private	Animator			animator;
	private PlayerInfo			playerInfo;
	private	uint				score;
	private	BackgroundComponent	bg;
	private	Observer			observer;

	public override void OnUIChangeLanguage (SmartLocalization.LanguageManager lm) {
		base.OnUIChangeLanguage (lm);
		labelBest.text = lm.GetTextValue ("fnf.ui.best");
	}

	public	override void OnUIStart() {
		base.OnUIStart();
		animator = GetComponent(typeof(Animator)) as Animator;
		playerInfo = PlayerInfoKeeper.GetInstance().playerInfo;
		bg = GameObject.FindObjectOfType<BackgroundComponent>();
		observer = Observer.GetInstance();
		// change medium image to selected animal's one
		ClearScore();
		ClearCoin();
		core.Clear();
		SendMessageUpwards("TurnOffFilter");
		animator.SetTrigger("trigger_init");
	}

	public	override void OnUIStop() {
		base.OnUIStop();
		FeverOff();
		SendMessageUpwards("TurnOnFilter");
	}

	void Update() {
	}

	public	void OnPop() {
		Invoke("OnInvoke", 0.1f);
	}

	public	void OnInvoke() {
		core.RandomNew();
		core.RandomNew();
	}

	public	void OnClickImage() {

	}

	public	void ClearScore() {
		playerInfo.highLevel = 0;
		score = 0;
		UpdateScore();
	}

	public	void AppendScore(int level) {
		score += (uint)playerInfo.buffInfoScore.Calculate(Mathf.Pow(2, fever.activeSelf ? level+1:level));
		UpdateScore();

		level += 1;
		if (((level)>playerInfo.highLevel) && (observer.highLevelChange!=null)) {
			playerInfo.highLevel = level;
			observer.highLevelChange(level);
		}
	}

	public	void UpdateScore() {
		textBest.text = playerInfo.bestScore.ToString();
		textScore.text = score.ToString();
	}

	public	void ClearCoin() {
		playerInfo.coinDelta = 0;
	}

	public	void AppendCoin(int delta = 1) {
		playerInfo.coinDelta += (int)playerInfo.buffInfoCoin.Calculate((float)delta);
		StoreInventory.GiveItem(StoreAssetInfo.COIN, delta);
	}

	public	void GameOver() {
		SaveScore();
		FeverOff();
		SendMessageUpwards("ReserveNextUI", UIType.RESULT);
		animator.SetTrigger("trigger_gameover");
	}

	public	void Win() {
		SaveScore();
		FeverOff();
		SendMessageUpwards("ReserveNextUI", UIType.RESULT);
		animator.SetTrigger("trigger_win");
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
		playerInfo.score = score;
	}
}
