using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioPlayerComponent : MonoBehaviour {
	private	static AudioPlayerComponent _instance;
	public	static AudioPlayerComponent GetInstance() {
		return _instance;
	}

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
	private AudioInfoSource[]			bgm;
	private	AudioInfoSource[]			fx;
	private	int							prevBgmChannel;
	private int							nextBgmChannel;
	private	int							nextFxChannel;
	private	string						reserved;
	private	Observer					observer;

	void Start() {
		_instance = this;

		// init channels
		bgm = new AudioInfoSource[channelBgm];
		for (int i = 0 ; i < channelBgm ; i++) {
			Transform t = Instantiate(prefabBgm) as Transform;
			t.SetParent(objBgm);
			t.name = "ch"+i.ToString();
			bgm[i] = t.GetComponent<AudioInfoSource>();
		}
		fx = new AudioInfoSource[channelFx];
		for (int i = 0 ; i < channelFx ; i++) {
			Transform t = Instantiate(prefabFx) as Transform;
			t.name = "ch"+i.ToString();
			t.SetParent(objFx);
			fx[i] = t.GetComponent<AudioInfoSource>();
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

//		if (reserved!=null) {
//			PlayBgm(reserved);
//			reserved = null;
//		}
	}

	void Update() {
		if (reserved!=null) {
			Play(reserved);
			reserved = null;
		}
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
			Play(info.bgm);
		}
	}

	public	void InvokeBpm() {
		if ((observer.beat!=null) && (currentBgm!=null)) {
			observer.beat (currentBgm.spb);
		}
	}

	public	static void Play(string id) {
		if (_instance.dictionary.ContainsKey (id)) {
			AudioInfo info = _instance.dictionary[id];
			if (info.type == AudioType.BGM) {
				_instance.PlayBgm(id);
			} else if (info.type == AudioType.FX) {
				_instance.PlayFx(id);
			}
		}
	}

	private	void PlayBgm(string id) {
		AudioInfoSource prevSource = bgm[prevBgmChannel];
		currentBgm = dictionary[id];
		if (prevSource.current==currentBgm) {
			return;
		}

		if (prevBgmChannel!=nextBgmChannel) {
			if (prevSource.isPlaying) {
				currentBgm = null;
				CancelInvoke("InvokeBpm");
				prevSource.Stop(true);
			}
		}

		bgm[nextBgmChannel].Play(currentBgm, true, volumnMaster*volumnBgm, true);
		InvokeRepeating("InvokeBpm", currentBgm.spb, currentBgm.spb);

		prevBgmChannel = nextBgmChannel;
		nextBgmChannel = (nextBgmChannel + 1) % channelBgm;
	}

	private	void PlayFx(string id) {
		fx[nextFxChannel].Play(dictionary[id], false, volumnMaster*volumnFx, false);
		nextFxChannel = (nextFxChannel + 1) % channelFx;
	}

	public	void TurnOnFilter() {
		bgm[prevBgmChannel].TweenFilterOn(insideLowPassCutOff, outsideLowPassCutOff, outsideVolumn);
	}
	
	public	void TurnOffFilter() {
		bgm [prevBgmChannel].TweenFilterOff (insideLowPassCutOff, outsideLowPassCutOff, insideVolumn);
	}
}
