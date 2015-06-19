using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Soomla;
using Soomla.Store;

[RequireComponent (typeof(Animator))]
public class GameComponent : UIComponent {

	public	CoreComponent		core;
	public	Text				textBest;
	public	Text				textScore;
	public	GameObject			fever;
	private	Animator			animator;
	private PlayerInfo			playerInfo;
	private	uint				score;
	private	BackgroundComponent	bg;

	public	override void OnUIStart() {
		base.OnUIStart();
		animator = GetComponent(typeof(Animator)) as Animator;
		playerInfo = PlayerInfoKeeper.GetInstance().playerInfo;
		bg = GameObject.FindObjectOfType<BackgroundComponent>();
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
		score = 0;
		UpdateScore();
	}

	public	void AppendScore(uint delta = 0) {
		score += delta;
		UpdateScore();
	}

	public	void UpdateScore() {
		textBest.text = playerInfo.bestScore.ToString();
		textScore.text = score.ToString();
	}

	public	void ClearCoin() {
		playerInfo.coinDelta = 0;
	}

	public	void AppendCoin(int delta = 1) {
		playerInfo.coinDelta += delta;
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
