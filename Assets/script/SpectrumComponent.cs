using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioLowPassFilter))]
[RequireComponent(typeof(AudioReverbFilter))]
public class SpectrumComponent : MonoBehaviour {

	public	AudioClip			clipNormal;
	public	AudioClip			clipFever;
	public	AudioLowPassFilter	filterLow;
	public	AudioReverbFilter	filterReverb;
	[Range(0.0f, 1.0f)]
	public	float				thresholdBig;
	[Range(0.0f, 1.0f)]
	public	float				thresholdSmall;
	public	bool				beatBig;
	public	bool				beatSmall;
	[Range(0.0f, 22000.0f)]
	public	int					insideLowPassCutOff;
	[Range(0.0f, 1.0f)]
	public	float				insideVolumn;
	[Range(0.0f, 22000.0f)]
	public	int					outsideLowPassCutOff;
	[Range(0.0f, 1.0f)]
	public	float				outsideVolumn;

	private	bool				reserveNormal;
	private bool				reserveFever;
	private	bool				checkinNormal;
	private	bool				checkinFever;

	void Start() {
		PlayNormal();
		TurnOnFilter();
	}

	void Update() {
		if (checkinNormal) {
			reserveNormal = false;
			checkinNormal = false;
			PlayNormal();
		}
		if (checkinFever) {
			reserveFever = false;
			checkinFever = false;
			PlayFever();
		}
	}

	void OnAudioFilterRead(float[] data, int channels) {
		float temp;
		float max = 0f;
		for (int i = data.Length-1 ; i > 0 ; i-=2) {
			temp = data[i-1];
			if (max<temp) {
				max = temp;
			}
			data[i-1] = data[i];
		}

		beatSmall		= max > thresholdSmall;
		beatBig			= max > thresholdBig;

		if (reserveNormal && beatBig) {
			checkinNormal = true;
		}
		if (reserveFever && beatBig) {
			checkinFever = true;
		}
	}

	private	void PlayClip(AudioClip clip) {
		GetComponent<AudioSource>().clip = clip;
		GetComponent<AudioSource>().Play();
		iTween.AudioTo(gameObject, 1, 1, 2f);
	}

	public	void PlayNormal() {
		PlayClip(clipNormal);
	}

	public	void PlayFever() {
		PlayClip (clipFever);
	}

	public	void PlayNormalWithEffect() {

	}

	public	void PlayFeverWithEffect() {

	}

	public	void ReserveFever() {
		reserveFever = true;
	}

	public	void ReserveNormal() {
		reserveNormal = true;
	}

	public	void TurnOnFilter() {
		filterLow.enabled = true;
		filterReverb.enabled = true;

		Hashtable hash = new Hashtable();
		hash.Add("from", insideLowPassCutOff);
		hash.Add("to", outsideLowPassCutOff);
		hash.Add("time", 2f);
		hash.Add("onupdatetarget", gameObject);
		hash.Add("onupdate", "OnFilterUpdate");
		iTween.ValueTo(gameObject, hash);
		iTween.AudioTo(gameObject, outsideVolumn, 1f, 2f);
	}

	private	void OnFilterUpdate(float value) {
		filterLow.cutoffFrequency = value;
	}

	public	void TurnOffFilter() {
		filterReverb.enabled = false;

		Hashtable hash = new Hashtable();
		hash.Add("from", outsideLowPassCutOff);
		hash.Add("to", insideLowPassCutOff);
		hash.Add("time", 2f);
		hash.Add("onupdatetarget", gameObject);
		hash.Add("onupdate", "OnFilterUpdate");
		hash.Add("oncompletetarget", gameObject);
		hash.Add("oncomplete", "OnFilterComplete");
		iTween.ValueTo(gameObject, hash);
		iTween.AudioTo(gameObject, insideVolumn, 1f, 2f);
	}

	public	void OnFilterComplete() {
		filterLow.enabled = false;
	}

	public	void Stop() {
		if (GetComponent<AudioSource>().isPlaying) {
			Hashtable hash = new Hashtable();
			hash.Add("volume", 0f);
			hash.Add("pitch", 1f);
			hash.Add("time", 2f);
			hash.Add("oncompletetarget", gameObject);
			hash.Add("oncomplete", "OnStopComplete");
			iTween.AudioTo(gameObject, hash);
		}
	}

	public	void OnStopComplete() {
		GetComponent<AudioSource>().Stop();
		GetComponent<AudioSource>().clip = null;
		filterLow.enabled = false;
		filterReverb.enabled = false;
	}
}
