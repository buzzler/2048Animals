using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ThemeInfo
{
	public	static Dictionary<AnimalType, ThemeInfo>	dictionary;
	public	static Dictionary<string, ThemeInfo>		dictionaryId;

	public	string				id 			{get{return _id;			}}
	public	string				code		{get{return _code;			}}
	public	string				name		{get{return _name;			}}
	public	string				description	{get{return _description;	}}	
	public	AnimalType			type		{get{return _type;			}}
	public	string				trigger		{get{return _trigger;		}}
	public	BackgroundType		bg			{get{return _bg;			}}
	public	BackgroundStatus	bgNormal	{get{return _bgNormal;		}}
	public	BackgroundStatus	bgFever		{get{return _bgFever;		}}
	public	int					coin		{get{return _coin;			}}
	public	string				bgm			{get{return _bgm;			}}
	public	BuffInfo			buffInfo	{get{return _buff;			}}

	private	string				_id;
	private	string				_code;
	private	string				_name;
	private	string				_description;
	private	AnimalType			_type;
	private	string				_trigger;
	private	BackgroundType		_bg;
	private	BackgroundStatus	_bgNormal;
	private	BackgroundStatus	_bgFever;
	private	int					_coin;
	private	string				_bgm;
	private	BuffInfo			_buff;

	public	static void Resister(ThemeInfo theme) {
		if (dictionary==null) {
			dictionary = new Dictionary<AnimalType, ThemeInfo>();
			dictionaryId = new Dictionary<string, ThemeInfo>();
		}
		dictionary.Add(theme.type, theme);
		dictionaryId.Add(theme.id, theme);
	}

	public	static ThemeInfo Find(AnimalType type) {
		if ((dictionary!=null)&&(dictionary.ContainsKey(type))) {
			return dictionary[type];
		} else {
			return null;
		}
	}

	public	static ThemeInfo Find(string id) {
		if ((dictionaryId!=null)&&(dictionaryId.ContainsKey(id))) {
			return dictionaryId[id];
		} else {
			return null;
		}
	}

	public	static ThemeInfo Parse(string[] line) {
		ThemeInfo theme	 = new ThemeInfo();
		theme._id 			= line[0];
		theme._code			= line[1];
		theme._name			= line[2];
		theme._description	= line[3];
		theme._coin			= int.Parse(line[4]);
		theme._type			= (AnimalType)System.Enum.Parse(typeof(AnimalType), theme.code, true);
		theme._trigger		= line[5];
		theme._bg			= (BackgroundType)System.Enum.Parse(typeof(BackgroundType), line[6], true);
		theme._bgNormal		= (BackgroundStatus)System.Enum.Parse(typeof(BackgroundStatus), line[7], true);
		theme._bgFever		= (BackgroundStatus)System.Enum.Parse(typeof(BackgroundStatus), line[8], true);
		theme._bgm			= line[9];
		theme._buff			= BuffInfo.Parse(line[10], line[11], line[12]);
		return theme;
	}
}
