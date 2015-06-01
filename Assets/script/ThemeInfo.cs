using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ThemeInfo
{
	public	static Dictionary<AnimalType, ThemeInfo>	dictionary;

	public	string				id 			{get{return _id;			}}
	public	string				name		{get{return _name;			}}
	public	string				description	{get{return _description;	}}	
	public	AnimalType			type		{get{return _type;			}}
	public	string				trigger		{get{return _trigger;		}}
	public	BackgroundType		bg			{get{return _bg;			}}
	public	BackgroundStatus	bgNormal	{get{return _bgNormal;		}}
	public	BackgroundStatus	bgFever		{get{return _bgFever;		}}
	public	int					coin		{get{return _coin;			}}
//	public	double				price		{get{return _price;			}}
//	public	string				productId	{get{return _productId;		}}
	public	string				categoryName{get{return _categoryName;	}}
	public	string				bgm			{get{return _bgm;			}}

	private	string				_id;
	private	string				_name;
	private	string				_description;
	private	AnimalType			_type;
	private	string				_trigger;
	private	BackgroundType		_bg;
	private	BackgroundStatus	_bgNormal;
	private	BackgroundStatus	_bgFever;
	private	int					_coin;
//	private	double				_price;
//	private	string				_productId;
	private	string				_categoryName;
	private	string				_bgm;

	public	static void Resister(ThemeInfo theme) {
		if (dictionary==null) {
			dictionary = new Dictionary<AnimalType, ThemeInfo>();
		}
		dictionary.Add(theme.type, theme);
	}

	public	static ThemeInfo Find(AnimalType type) {
		if ((dictionary!=null)&&(dictionary.ContainsKey(type))) {
			return dictionary[type];
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
//		theme._price		= double.Parse(line[4]);
//		theme._productId	= line[5];
		theme._categoryName	= line[6];
		theme._type			= (AnimalType)System.Enum.Parse(typeof(AnimalType), theme.name, true);
		theme._trigger		= line[7];
		theme._bg			= (BackgroundType)System.Enum.Parse(typeof(BackgroundType), line[8], true);
		theme._bgNormal		= (BackgroundStatus)System.Enum.Parse(typeof(BackgroundStatus), line[9], true);
		theme._bgFever		= (BackgroundStatus)System.Enum.Parse(typeof(BackgroundStatus), line[10], true);
		theme._bgm			= line[11];
		return theme;
	}
}
