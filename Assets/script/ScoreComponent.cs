using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreComponent : MonoBehaviour {

	public	Text	score;
	public	Image	animal;

	void OnEnable() {
		// animal theme
	}

	public	void SetScore(uint score) {
		this.score.text = score.ToString();
	}

	public	void SetScore(float score) {
		SetScore((uint)score);
	}

	public	void SetColor(Color color) {
		score.color = color;
	}

	public	uint GetScore() {
		return uint.Parse(score.text);
	}
}
