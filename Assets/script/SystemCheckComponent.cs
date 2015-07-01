using UnityEngine;
using System;
using System.Collections;

public delegate void SocialDelegate(SocialResult result);

public class SystemCheckComponent : MonoBehaviour {
	public	static bool network { get { return _network; } }
	public	static bool data	{ get { return _data; } }
	public	static bool wifi	{ get { return _wifi; } }
	public	static int	pingTime{ get { return _pingTime; } }
	public	static bool busy	{ get { return _busy; } }
	private	static bool _network;
	private	static bool _data;
	private	static bool	_wifi;
	private	static int	_pingTime;
	private	static bool	_busy;

	public	float	duration;
	public	float	pingTimeout;
	private	float	pingStart;
	private	Ping	ping;
	private	bool	_last;
	
	void Start() {
		_network = false;
		_data = false;
		_wifi = false;
		_pingTime = 0;
		_last = false;

		if (duration > 0) {
			InvokeRepeating("Check", 0, duration);
		}
	}

	void Update() {
		if (ping != null) {
			if (ping.isDone) {
				OnSuccess();
			} else if (Time.time-pingStart < pingTimeout) {
				// wait..
			} else {
				// timeout!!
				_pingTime = ping.time;
				ping.DestroyPing();
				ping = null;
				pingStart = 0;
				OnFail("time out");
			}
		}

		if (_last != _network) {
			_last = _network;
			Observer observer = Observer.GetInstance();
			if (observer.networkStatusChange!=null) {
				observer.networkStatusChange(_network);
			}
			if (_network && (!FB.IsInitialized)) {
				ConnectFacebook();
			}
		}
	}

	/**
	 * network check start
	 */

	private	void Check() {
//		StartCoroutine("CheckWWW");
		CheckPing ();
	}

	private	void OnFail(string error = "") {
		DebugComponent.Error (error);
		_network = false;
	}

	private	void OnSuccess() {
		_network = true;
	}

	private	bool CheckNetwork() {
		_data = false;
		_wifi = false;

		switch (Application.internetReachability) {
		case NetworkReachability.ReachableViaCarrierDataNetwork:
			_data = true;
			break;
		case NetworkReachability.ReachableViaLocalAreaNetwork:
			_wifi = true;
			break;
		default:
			break;
		}
		return _data || _wifi;
	}

	IEnumerator CheckWWW() {
		if (CheckNetwork ()) {
			WWW w = new WWW ("http://www.facebook.com");
			yield return w;
			if (!string.IsNullOrEmpty (w.error)) {
				OnFail (w.error);
			} else {
				OnSuccess ();
			}
		} else {
			OnFail();
		}
	}

	private	void CheckPing() {
		if (CheckNetwork ()) {
			ping = new Ping ("8.8.8.8");
			pingStart = Time.time;
		} else {
			OnFail();
		}
	}

	/**
	 * network check end
	 */

	/**
	 * social check start
	 */

	/**
	 * connect facebook account
	 */
	public bool ConnectFacebook() {
		if (_network && (!_busy)) {
			if (FB.IsInitialized) {
				oninitcomplete ();
			} else {
				_busy = true;
				FB.Init (oninitcomplete);
			}
			return true;
		}
		return false;
	}

	/**
	 * check account logged in
	 */
	public	static bool IsLoggedIn() {
		return (_network && FB.IsInitialized && FB.IsLoggedIn);
	}

	/**
	 * check this component working
	 */
	public	static bool IsNotBusy() {
		return IsLoggedIn () && (!_busy);
	}

	/**
	 * Login facebook account
	 */
	public	bool LoginFacebook() {
		if (_network && FB.IsInitialized && (!FB.IsLoggedIn) && (!_busy)) {
			_busy = true;
			FB.Login ("public_profile, user_friends", onlogincomplete);
		}
		return false;
	}

	/**
	 * Logout facebook account
	 */
	public bool LogoutFacebook() {
		if (IsNotBusy()) {
			FB.Logout();
			Observer observer = Observer.GetInstance();
			if (observer.fbLogin!=null) {observer.fbLogin(false);}
			return true;
		}
		return false;
	}
	
	/**
	 * feed on facebook
	 */
	public bool FeedFacebook(string title, string url, string description, string caption, string thumbnail) {
		if (IsNotBusy()) {
			_busy = true;
			FB.Feed(
				link: url,
				linkName: title,
				linkDescription: description,
				linkCaption: caption,
				picture: thumbnail,
				callback: onfeedcomplete
				);
			return true;
		}
		return false;
	}
	
	/**
	 * invite friend from facebook
	 */
	public bool RequestFacebook(string message, string title) {
		if (IsNotBusy()) {
			_busy = true;
			FB.AppRequest(message, null, null, null, 1, "", title, onrequestcomplete);
			return true;
		}
		return false;
	}
	
	private	void oninitcomplete() {
		_busy = false;
		Observer observer = Observer.GetInstance ();
		if (FB.IsInitialized) {
			if (observer.fbConnet!=null) {observer.fbConnet(true);}
			if (FB.IsLoggedIn) {
				if (observer.fbLogin!=null) {observer.fbLogin(true);}
			}
		} else {
			if (observer.fbConnet!=null) {observer.fbConnet(false);}
		}
	}
	
	private void onlogincomplete(FBResult result) {
		_busy = false;
		Observer observer = Observer.GetInstance ();
		if (FB.IsLoggedIn) {
			if (observer.fbLogin!=null) {observer.fbLogin(true);}
		} else {
			if (observer.fbLogin!=null) {observer.fbLogin(false);}
		}
	}
	
	private void onfeedcomplete(FBResult result) {
		_busy = false;
		Observer observer = Observer.GetInstance ();
		if (String.IsNullOrEmpty(result.Error)) {
			if (observer.fbFeed!=null) {observer.fbFeed(true);}
		} else {
			if (observer.fbFeed!=null) {observer.fbFeed(false);}
		}
	}
	
	private void onrequestcomplete(FBResult result) {
		_busy = false;
		Observer observer = Observer.GetInstance ();
		if (String.IsNullOrEmpty(result.Error)) {
			if (observer.fbRequest!=null) {observer.fbRequest(true);}
		} else {
			if (observer.fbRequest!=null) {observer.fbRequest(false);}
		}
	}
}
