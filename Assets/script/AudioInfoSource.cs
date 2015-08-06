using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class AudioInfoSource : MonoBehaviour {
	public	AudioInfo			current {get {return _info;}}
	public	bool				hasFilter {get {return (_lowFilter!=null && _reverbFilter!=null);}}
	private	AudioInfo			_info;
	private	AudioSource			_source;
	private	AudioLowPassFilter	_lowFilter;
	private	AudioReverbFilter	_reverbFilter;

	public	void Start() {
		_source = GetComponent<AudioSource> ();
		_lowFilter = GetComponent<AudioLowPassFilter> ();
		_reverbFilter = GetComponent<AudioReverbFilter> ();
	}

	public	bool isPlaying {
		get {
			if (_source) {
				return _source.isPlaying;
			} else {
				return false;
			}
		}
	}

	public	void Stop(bool tween = false) {
		if (_source.isPlaying) {
			if (tween) {
				Hashtable hash = new Hashtable();
				hash.Add("volumn", 0);
				hash.Add("time", 0.5f);
				hash.Add("oncompletetarget", gameObject);
				hash.Add("oncomplete", "StopImmediately");
				iTween.AudioTo(gameObject, hash);
			} else {
				StopImmediately();
			}
		}
	}

	public	void StopImmediately() {
		if (_source.isPlaying) {
			_source.Stop();
			_info = null;
		}
	}

	public	void Play(AudioInfo info, bool loop, float volumn, bool tween = false) {
		if ((_info != null) && (info != null) && (_info.priority < info.priority) && (_source.isPlaying)) {
			return;
		} else {
			StopImmediately();
		}

		_info = info;
		_source.loop = loop;
		_source.volume = volumn;
//		_source.clip = Resources.Load("audio/"+info.id, typeof(AudioClip)) as AudioClip;
		_source.clip = CachedResource.Load<AudioClip> ("audio/" + info.id);
		_source.enabled = true;
		_source.Play ();

		if (tween) {
			iTween.AudioFrom(gameObject, 0, 1, 2f);
		}
	}

	public	void TweenFilterOn(float insideLowPassCutOff, float outsideLowPassCutOff, float outsideVolumn) {
		if (_source.isPlaying && hasFilter) {
			_lowFilter.enabled = true;
			_reverbFilter.enabled = true;
			
			Hashtable hash = new Hashtable();
			hash.Add("from", insideLowPassCutOff);
			hash.Add("to", outsideLowPassCutOff);
			hash.Add("time", 2f);
			hash.Add("onupdatetarget", gameObject);
			hash.Add("onupdate", "OnUpdateFilter");
			iTween.ValueTo(gameObject, hash);
			iTween.AudioTo(gameObject, outsideVolumn, 1f, 2f);
		}
	}

	public	void TweenFilterOff(float insideLowPassCutOff, float outsideLowPassCutOff, float insideVolumn) {
		if (_source.isPlaying && hasFilter) {
			_reverbFilter.enabled = false;
			
			Hashtable hash = new Hashtable();
			hash.Add("from", outsideLowPassCutOff);
			hash.Add("to", insideLowPassCutOff);
			hash.Add("time", 2f);
			hash.Add("onupdatetarget", gameObject);
			hash.Add("onupdate", "OnUpdateFilter");
			hash.Add("oncompletetarget", gameObject);
			hash.Add("oncomplete", "OnCompleteFilter");
			iTween.ValueTo(gameObject, hash);
			iTween.AudioTo(gameObject, insideVolumn, 1f, 2f);
		}
	}

	public	void OnUpdateFilter(float value) {
		_lowFilter.cutoffFrequency = value;
	}

	public	void OnCompleteFilter() {
		_lowFilter.enabled = false;
	}
}
