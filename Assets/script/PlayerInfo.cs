using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SmartLocalization;

using System.Security.Cryptography;

[Serializable]
public class PlayerInfo {
	public string		language;
	public DateTime		ads;
	public AnimalType	lastAnimalType;
	public uint			bestScore;
	public int[]		stars;

	public uint			gameScore;
	public int			gameCoin;
	public int			highLevel;
	public BuffInfo		buffInfoCoin;
	public BuffInfo		buffInfoScore;
	public BuffInfo		buffInfoReward;

	public	JSONObject ToJSON() {
		JSONObject json = new JSONObject ();
		json.AddField ("lang", language);
		json.AddField ("ads", ads.ToString ());
		json.AddField ("theme", lastAnimalType.ToString ());
		json.AddField ("best", bestScore.ToString ());

		string[] ary = new string[stars.Length];
		for (int i = stars.Length-1 ; i >=0 ; i--) {
			ary[i] = stars[i].ToString();
		}
		json.AddField ("stars", string.Join (",", ary));
		return json;
	}

	public	void FromJSON(JSONObject json) {
		if (json.HasField ("lang")) {
			language = json.GetField ("lang").str;
		}
		if (json.HasField ("ads")) {
			ads = DateTime.Parse (json.GetField ("ads").str);
		}
		if (json.HasField ("theme")) {
			lastAnimalType = (AnimalType)Enum.Parse (typeof(AnimalType), json.GetField ("theme").str, true);
		}
		if (json.HasField ("best")) {
			bestScore = uint.Parse (json.GetField ("best").str);
		}
		if (json.HasField ("stars")) {
			string[] ary = json.GetField("stars").str.Split(","[0]);
			for (int i = stars.Length-1 ; i >= 0 ; i--) {
				stars[i] = int.Parse(ary[i]);
			}
		}
	}

	public PlayerInfo() {
		bestScore = 0;
		gameScore = 0;
		gameCoin = 0;
		highLevel = 0;
		stars = new int[200];
		lastAnimalType = AnimalType.BEAR;
		ads = DateTime.MinValue;
		buffInfoCoin = new BuffInfo();
		buffInfoScore = new BuffInfo();
		buffInfoReward = new BuffInfo();
		language = null;

		for (int i = stars.Length-1 ; i >= 0 ; i--) {
			stars[i] = 0;
		}
	}

	public	string GetId() {
		return SystemInfo.deviceUniqueIdentifier;
	}

	public	ThemeInfo GetThemeInfo() {
		return ThemeInfo.Find (lastAnimalType);
	}

	public	void Clear() {
		gameScore = 0;
		gameCoin = 0;
		highLevel = 0;
	}
}

public	class PlayerInfoManager {
	private static PlayerInfo _instance;
	private	const string _KEY	= "playerinfo";
	private	const string _PW	= "com.fnfgames.jellycraft";

	public	static PlayerInfo instance {
		get {
			if (_instance==null) {
				Load();
			}
			return _instance;
		}
	}

	public	static void Load() {
		if (_instance == null) {
			_instance = new PlayerInfo ();
		}

		if (PlayerPrefs.HasKey (_KEY)) {
			string str = DecryptText(PlayerPrefs.GetString(_KEY), true);
			_instance.FromJSON(new JSONObject(str));
		}
	}

	public	static void Save() {
		if (_instance == null) {
			_instance = new PlayerInfo ();
		}

		ThemeInfo themeInfo = _instance.GetThemeInfo();
		_instance.bestScore = (_instance.bestScore > _instance.gameScore) ? _instance.bestScore : _instance.gameScore;
		_instance.stars[themeInfo.order] = Mathf.Max(_instance.stars[themeInfo.order], _instance.highLevel);
		_instance.Clear();

		string str = _instance.ToJSON ().ToString ();
		DebugComponent.Log (str);
		PlayerPrefs.SetString (_KEY, EncryptText(str, true));
	}

	public static string EncryptText(string toEncrypt, bool useHashing)
	{
		try
		{
			byte[] keyArray;
			byte[] toEncryptArray = System.Text.UTF8Encoding.UTF8.GetBytes(toEncrypt);

			//System.Windows.Forms.MessageBox.Show(key);
			//If hashing use get hashcode regards to your key
			if (useHashing)
			{
				MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
				keyArray = hashmd5.ComputeHash(System.Text.UTF8Encoding.UTF8.GetBytes(_PW));
				//Always release the resources and flush data
				//of the Cryptographic service provide. Best Practice
				
				hashmd5.Clear();
			}
			else
				keyArray = System.Text.UTF8Encoding.UTF8.GetBytes(_PW);
			
			TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
			//set the secret key for the tripleDES algorithm
			tdes.Key = keyArray;
			//mode of operation. there are other 4 modes. We choose ECB(Electronic code Book)
			tdes.Mode = CipherMode.ECB;
			//padding mode(if any extra byte added)
			tdes.Padding = PaddingMode.PKCS7;
			
			ICryptoTransform cTransform = tdes.CreateEncryptor();
			//transform the specified region of bytes array to resultArray
			byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0,          toEncryptArray.Length);
			//Release resources held by TripleDes Encryptor
			tdes.Clear();
			//Return the encrypted data into unreadable string format
			return Convert.ToBase64String(resultArray, 0, resultArray.Length);
		}
		catch (Exception e)
		{
			throw e;
		}
	}
	
	//Decryption
	public static string DecryptText(string cipherString, bool useHashing)
	{
		
		try
		{
			byte[] keyArray;
			//get the byte code of the string
			
			byte[] toEncryptArray = Convert.FromBase64String(cipherString);
			
			if (useHashing)
			{
				//if hashing was used get the hash code with regards to your key
				MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
				keyArray = hashmd5.ComputeHash(System.Text.UTF8Encoding.UTF8.GetBytes(_PW));
				//release any resource held by the MD5CryptoServiceProvider
				
				hashmd5.Clear();
			}
			else
			{
				//if hashing was not implemented get the byte code of the key
				keyArray = System.Text.UTF8Encoding.UTF8.GetBytes(_PW);
			}
			
			TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
			//set the secret key for the tripleDES algorithm
			tdes.Key = keyArray;
			//mode of operation. there are other 4 modes.
			//We choose ECB(Electronic code Book)
			
			tdes.Mode = CipherMode.ECB;
			//padding mode(if any extra byte added)
			tdes.Padding = PaddingMode.PKCS7;
			
			ICryptoTransform cTransform = tdes.CreateDecryptor();
			byte[] resultArray = cTransform.TransformFinalBlock
				(toEncryptArray, 0, toEncryptArray.Length);
			//Release resources held by TripleDes Encryptor
			tdes.Clear();
			//return the Clear decrypted TEXT
			return System.Text.UTF8Encoding.UTF8.GetString(resultArray);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
/*
public class PlayerInfoKeeper2 {
	private	static PlayerInfoKeeper2 instance;
	public	static PlayerInfoKeeper2 GetInstance() {
		if (instance==null) {
			instance = new PlayerInfoKeeper2();
		}
		return instance;
	}

	private string filepath;
	private PlayerInfo info;

	public PlayerInfoKeeper2 () {
		Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
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
			info.bestScore = (info.gameScore>info.bestScore) ? info.gameScore:info.bestScore;
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
			bf.Serialize(fs, info);
			fs.Close();
		} else {
			Create();
			Load();
		}
	}

	public void Create() {
		BinaryFormatter bf = new BinaryFormatter();
		FileStream fs = new FileStream(filepath, FileMode.CreateNew);

		info = new PlayerInfo();
		bf.Serialize(fs, info);
		fs.Close();
	}

}

*/