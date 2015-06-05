using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioPlayerComponent : MonoBehaviour {
	public	Transform					objMain;
	public	Transform					objBgm;
	public	Transform					objFx;
	public	Transform					prefabBgm;
	public	Transform					prefabFx;
	[Range(0.0f, 1.0f)]
	public	float						volumnMaster;
	[Range(0.0f, 1.0f)]
	public	float						volumnBgm;
	[Range(0.0f, 1.0f)]
	public	float						volumnFx;
	public	int							channelBgm;
	public	int							channelFx;
	[Range(0.0f, 22000.0f)]
	public	int							insideLowPassCutOff;
	[Range(0.0f, 1.0f)]
	public	float						insideVolumn;
	[Range(0.0f, 22000.0f)]
	public	int							outsideLowPassCutOff;
	[Range(0.0f, 1.0f)]
	public	float						outsideVolumn;


	public	TextAsset					asset;
	public	AudioInfo[]					infos;
	private	AudioInfo					currentBgm;
	private	Dictionary<string,AudioInfo>dictionary;
	private AudioSource[]				bgm;
	private	AudioSource[]				fx;
	private	int							prevBgmChannel;
	private int							nextBgmChannel;
	private	int							nextFxChannel;
	private	string						reserved;
	private	Observer					observer;

	void Start() {
		// init channels
		bgm = new AudioSource[channelBgm];
		for (int i = 0 ; i < channelBgm ; i++) {
			Transform t = Instantiate(prefabBgm) as Transform;
			t.SetParent(objBgm);
			t.name = "ch"+i.ToString();
			bgm[i] = t.GetComponent<AudioSource>();
		}
		fx = new AudioSource[channelFx];
		for (int i = 0 ; i < channelFx ; i++) {
			Transform t = Instantiate(prefabFx) as Transform;
			t.name = "ch"+i.ToString();
			t.SetParent(objFx);
			fx[i] = t.GetComponent<AudioSource>();
		}
		prevBgmChannel = 0;
		nextBgmChannel = 0;
		nextFxChannel = 0;

		// read asset file
		string[][] grid = CSVReader.SplitCsvJaggedGrid(asset.text);
		int count = grid.Length-1;		// without header
		infos = new AudioInfo[count];
		dictionary = new Dictionary<string, AudioInfo>();
		for (int y = 0 ; y < count ; y++) {
			AudioInfo i = AudioInfo.Parse(grid[y+1]);
			infos[y] = i;
			dictionary.Add(i.id, i);
		}

		if (reserved!=null) {
			PlayBgm(reserved);
			reserved = null;
		}
	}

	void Update() {
	}

	void OnEnable() {
		if (observer==null) {
			observer = Observer.GetInstance();
		}
		observer.themeChange += OnThemeChange;
	}

	void OnDisable() {
		if (observer==null) {
			observer = Observer.GetInstance();
		}
		observer.themeChange -= OnThemeChange;
	}

	public	void OnThemeChange(ThemeInfo info) {
		if (bgm==null) {
			reserved = info.bgm;
		} else {
			PlayBgm(info.bgm);
		}
	}

	public	void InvokeBpm() {
		if ((observer.beat!=null) && (currentBgm!=null)) {
			observer.beat (currentBgm.spb);
		}
	}

	public	void PlayBgm(string id) {
		if (prevBgmChannel!=nextBgmChannel) {
			AudioSource prevSource = bgm[prevBgmChannel];
			if (prevSource.isPlaying) {
				currentBgm = null;
				CancelInvoke("InvokeBpm");
				iTween.AudioTo(prevSource.gameObject, 0, 1, 0.5f);
			}
		}

		AudioSource aSource = bgm[nextBgmChannel];
		if (aSource.isPlaying) {
			aSource.Stop();
		}

		if (dictionary.ContainsKey(id)) {
			currentBgm = dictionary[id];
			AudioClip aClip = Resources.Load("audio/"+currentBgm.id, typeof(AudioClip)) as AudioClip;

			aSource.loop = true;
			aSource.volume = volumnMaster*volumnBgm;
			aSource.clip = aClip;
			aSource.enabled = true;
			aSource.Play();

			InvokeRepeating("InvokeBpm", currentBgm.spb, currentBgm.spb);
			iTween.AudioFrom(aSource.gameObject, 0, 1, 1f);
		}

		prevBgmChannel = nextBgmChannel;
		nextBgmChannel = (nextBgmChannel + 1) % channelBgm;
	}

	public	void PlayFx(string id) {
		AudioSource aSource = fx[nextFxChannel];
		if (aSource.isPlaying) {
			aSource.Stop();
		}
		
		if (dictionary.ContainsKey(id)) {
			AudioInfo aInfo = dictionary[id];
			aSource.loop = false;
			aSource.volume = volumnMaster*volumnFx;
			aSource.clip = Resources.Load("audio/"+aInfo.id, typeof(AudioClip)) as AudioClip;
			aSource.Play();
		}

		nextFxChannel = (nextFxChannel + 1) % channelFx;
	}

	public	void TurnOnFilter() {
		AudioSource aSource = bgm[prevBgmChannel];
		if (aSource.isPlaying) {
			aSource.GetComponent<AudioLowPassFilter>().enabled = true;
			aSource.GetComponent<AudioReverbFilter>().enabled = true;

			Hashtable hash = new Hashtable();
			hash.Add("from", insideLowPassCutOff);
			hash.Add("to", outsideLowPassCutOff);
			hash.Add("time", 2f);
			hash.Add("onupdatetarget", gameObject);
			hash.Add("onupdate", "OnFilterUpdate");
			iTween.ValueTo(aSource.gameObject, hash);
			iTween.AudioTo(aSource.gameObject, outsideVolumn, 1f, 2f);
		}
	}
	
	public	void OnFilterUpdate(float value) {
		bgm[prevBgmChannel].GetComponent<AudioLowPassFilter>().cutoffFrequency = value;
	}
	
	public	void TurnOffFilter() {
		AudioSource aSource = bgm[prevBgmChannel];
		if (aSource.isPlaying) {
			aSource.GetComponent<AudioReverbFilter>().enabled = false;

			Hashtable hash = new Hashtable();
			hash.Add("from", outsideLowPassCutOff);
			hash.Add("to", insideLowPassCutOff);
			hash.Add("time", 2f);
			hash.Add("onupdatetarget", gameObject);
			hash.Add("onupdate", "OnFilterUpdate");
			hash.Add("oncompletetarget", gameObject);
			hash.Add("oncomplete", "OnFilterComplete");
			iTween.ValueTo(aSource.gameObject, hash);
			iTween.AudioTo(aSource.gameObject, insideVolumn, 1f, 2f);
		}
	}
	
	public	void OnFilterComplete() {
		bgm[prevBgmChannel].GetComponent<AudioLowPassFilter>().enabled = false;
	}
}
