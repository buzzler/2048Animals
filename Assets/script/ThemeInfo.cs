using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ThemeInfo
{
	public	static Dictionary<AnimalType, ThemeInfo>	dictionary;
	public	static Dictionary<string, ThemeInfo>		dictionaryId;

	public	string				id 			{get{return _id;			}}
	public	string				name		{get{return _name;			}}
	public	string				description	{get{return _description;	}}	
	public	AnimalType			type		{get{return _type;			}}
	public	string				trigger		{get{return _trigger;		}}
	public	BackgroundType		bg			{get{return _bg;			}}
	public	BackgroundStatus	bgNormal	{get{return _bgNormal;		}}
	public	BackgroundStatus	bgFever		{get{return _bgFever;		}}
	public	int					coin		{get{return _coin;			}}
	public	string				categoryName{get{return _categoryName;	}}
	public	string				bgm			{get{return _bgm;			}}
	public	BuffInfo			buffInfo	{get{return _buff;			}}

	private	string				_id;
	private	string				_name;
	private	string				_description;
	private	AnimalType			_type;
	private	string				_trigger;
	private	BackgroundType		_bg;
	private	BackgroundStatus	_bgNormal;
	private	BackgroundStatus	_bgFever;
	private	int					_coin;
	private	string				_categoryName;
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
		theme._name			= line[1];
		theme._description	= line[2];
		theme._coin			= int.Parse(line[3]);
		theme._categoryName	= line[6];
		theme._type			= (AnimalType)System.Enum.Parse(typeof(AnimalType), theme.name, true);
		theme._trigger		= line[7];
		theme._bg			= (BackgroundType)System.Enum.Parse(typeof(BackgroundType), line[8], true);
		theme._bgNormal		= (BackgroundStatus)System.Enum.Parse(typeof(BackgroundStatus), line[9], true);
		theme._bgFever		= (BackgroundStatus)System.Enum.Parse(typeof(BackgroundStatus), line[10], true);
		theme._bgm			= line[11];
		theme._buff			= BuffInfo.Parse(line[12], line[13], line[14]);
		return theme;
	}
}
