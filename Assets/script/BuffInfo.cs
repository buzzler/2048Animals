using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class BuffInfo {
	private	BuffType	_type;
	private float		_rate;
	private	float		_add;

	public	BuffType	type	{get {return _type;}}
	public	float		rate	{get {return _rate;}}
	public	float		add		{get {return _add;}}

	public	BuffInfo() {
		_type = BuffType.NONE;
		_rate = 0.5f;
		_add = 0;
	}

	public	static BuffInfo Max(BuffInfo a, BuffInfo b) {
		if (a.Calculate (100f) > b.Calculate (100f)) {
			return a;
		} else {
			return b;
		}
	}

	public	static BuffInfo Parse(string type, string rate, string add) {
		BuffInfo info = new BuffInfo();
		info._type = (BuffType)System.Enum.Parse(typeof(BuffType), type, true);
		if (info._type == BuffType.SCORE) {
			info._rate = 1f;
		} else {
			info._rate = float.Parse (rate);
		}
		info._add = float.Parse(add);
		return info;
	}

	public	float Calculate(float value) {
		return (value + _add) * _rate;
	}
}
