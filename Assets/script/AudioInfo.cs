using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class AudioInfo {
	public	string		id	{get {return _id;}}
	public	string		name{get {return _name;}}
	public	int			bpm	{get {return _bpm;}}
	public	float		spb	{get {return _spb;}}
	public	AudioType	type{get {return _type;}}

	private	string		_id;
	private	string		_name;
	private	int			_bpm;
	private	float		_spb;
	private	AudioType	_type;

	public	static AudioInfo Parse(string[] line) {
		AudioInfo audio	= new AudioInfo();
		audio._id		= line[0];
		audio._name		= line[1];
		audio._bpm		= int.Parse(line[2]);
		audio._type		= (AudioType)System.Enum.Parse(typeof(AudioType), line[3], true);
		audio._spb		= 1.0f/((float)audio._bpm/60.0f);
		return audio;
	}
}
