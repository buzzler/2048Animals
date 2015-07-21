using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PackInfo {
	public	static Dictionary<string, PackInfo>	allInfos;
    public  static Dictionary<string, PackInfo> coinInfos;
    public  static Dictionary<string, PackInfo> footInfos;

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

	public	static void Resister(PackInfo info) {
		if (allInfos==null) {
			allInfos = new Dictionary<string, PackInfo>();
            coinInfos = new Dictionary<string, PackInfo>();
            footInfos = new Dictionary<string, PackInfo>();
		}
		allInfos.Add(info.id, info);
        if (info.currency.ToLower()==StoreAssetInfo.COIN) {
            coinInfos.Add(info.id, info);
        } else if (info.currency.ToLower()==StoreAssetInfo.FOOT) {
            footInfos.Add(info.id, info);
        }
	}
	
	public	static PackInfo Find(string id) {
		if ((allInfos!=null)&&(allInfos.ContainsKey(id))) {
			return allInfos[id];
		} else {
			return null;
		}
	}

	public	static PackInfo Parse(string[] line) {
		PackInfo info = new PackInfo ();
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
