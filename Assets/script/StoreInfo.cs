using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class StoreInfo {
	public	static Dictionary<string, StoreInfo>	dictionary;

	public	string	id { get { return _id; } }
	public	string	description	{ get { return _description; } }
	public	string	name { get { return _name; } }
	public	string	currency { get { return _currency; } }
	public	int		amount { get { return _amount; } }
	public	string	productId { get { return _productId; } }
	public	double	price { get { return _price; } }

	private	string	_id;
	private string	_name;
	private	string	_description;
	private	string	_currency;
	private	int		_amount;
	private	string	_productId;
	private	double	_price;

	public	static void Resister(StoreInfo info) {
		if (dictionary==null) {
			dictionary = new Dictionary<string, StoreInfo>();
		}
		dictionary.Add(info.id, info);
	}
	
	public	static StoreInfo Find(string id) {
		if ((dictionary!=null)&&(dictionary.ContainsKey(id))) {
			return dictionary[id];
		} else {
			return null;
		}
	}

	public	static StoreInfo Parse(string[] line) {
		StoreInfo info = new StoreInfo ();
		info._id			= line [0];
		info._name			= line [1];
		info._description	= line [2];
		info._currency		= line [3];
		info._amount		= int.Parse(line[4]);
		info._productId		= line [5];
		info._price			=  double.Parse(line[6]);
		return info;
	}
}
