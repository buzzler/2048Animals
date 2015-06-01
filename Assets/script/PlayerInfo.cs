using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SmartLocalization;

[Serializable]
public class PlayerInfo {
	public bool			resistered;
	public uint			bestScore;
	public uint			score;
	public int			coinDelta;
	public AnimalType	type;
	public DateTime		first;
	public DateTime		last;
	public string		language;

	public PlayerInfo() {
		resistered = false;
		bestScore = 0;
		score = 0;
		coinDelta = 0;
		type = AnimalType.BEAR;
		first = DateTime.Now;
		last = DateTime.Now;
		language = null;
	}

	public	string GetId() {
		return SystemInfo.deviceUniqueIdentifier;
	}

	public	ThemeInfo GetThemeInfo() {
		return ThemeInfo.Find(type);
	}
}

public class PlayerInfoKeeper {
	private	static PlayerInfoKeeper instance;
	public	static PlayerInfoKeeper GetInstance() {
		if (instance==null) {
			instance = new PlayerInfoKeeper();
		}
		return instance;
	}

	private string filepath;
	private PlayerInfo info;

	public PlayerInfoKeeper () {
		filepath = Application.persistentDataPath + "/playerinfo.dat";
		Load ();
	}

	public PlayerInfo playerInfo {
		get {
			return info;
		}
		set {
			info = value;
		}
	}

	public void Save() {
		if (info!=null) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream fs = new FileStream(filepath, FileMode.Open);
			info.bestScore = (info.score>info.bestScore) ? info.score:info.bestScore;
			info.last = DateTime.Now;
			bf.Serialize(fs, info);
			fs.Close();
		} else {
			DebugComponent.Error("PlayerInfo is null");
		}
	}
	
	public void Load() {
		if (File.Exists(filepath)) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate);
			info = bf.Deserialize(fs) as PlayerInfo;
			info.last = DateTime.Now;
			bf.Serialize(fs, info);
			fs.Close();
//			LanguageManager.Instance.ChangeLanguage(info.language);
		} else {
			Create();
			Load();
		}
	}

	public void Create() {
		BinaryFormatter bf = new BinaryFormatter();
		FileStream fs = new FileStream(filepath, FileMode.Create);
		info = new PlayerInfo();
		bf.Serialize(fs, info);
		fs.Close();
	}
}