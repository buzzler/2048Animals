using UnityEngine;
using System;
using System.Collections;

public delegate void SocialDelegate(SocialResult result);

public class SystemCheckComponent : MonoBehaviour {
	public	static bool network { get { return _network; } }
	public	static bool data	{ get { return _data; } }
	public	static bool wifi	{ get { return _wifi; } }
	public	static int	pingTime{ get { return _pingTime; } }
	private	static bool _network;
	private	static bool _data;
	private	static bool	_wifi;
	private	static int	_pingTime;

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

	void OnApplicationPause(bool pauseStatus) {
		if (pauseStatus) {
			CancelInvoke();
		} else {
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
#if UNITY_EDITOR || UNITY_ANDROID || UNITY_IOS
			if (_network) {
				ConnectFacebook();
			}
#endif
		}
	}

	private	void Check() {
		if (CheckNetwork ()) {
			ping = new Ping ("8.8.8.8");
			pingStart = Time.time;
		} else {
			OnFail();
		}
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

	/**
	 * network check end
	 */

#if UNITY_EDITOR || UNITY_ANDROID || UNITY_IOS
	/**
	 * social check start
	 */

	/**
	 * connect facebook account
	 */
	public bool ConnectFacebook() {
		if (_network) {
			if (FB.IsInitialized) {
				oninitcomplete ();
			} else {
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
	 * Login facebook account
	 */
	public	bool LoginFacebook() {
		if (_network && FB.IsInitialized && (!FB.IsLoggedIn)) {
			FB.Login ("public_profile, user_friends, publish_actions", onlogincomplete);
		}
		return false;
	}

	/**
	 * Logout facebook account
	 */
	public bool LogoutFacebook() {
		if (_network && FB.IsInitialized && FB.IsLoggedIn) {
			FB.Logout();
			PlayerPrefs.SetInt("login", 0);
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
		if (_network && FB.IsInitialized && FB.IsLoggedIn) {
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
		if (_network && FB.IsInitialized && FB.IsLoggedIn) {
			FB.AppRequest(message, null, null, null, 1, "", title, onrequestcomplete);
			return true;
		}
		return false;
	}

	/**
	 * upload snapshot to facebook
	 */
	public	void UploadFacebook() {
		StartCoroutine(TakeScreenshot());
	}
	
	private	void oninitcomplete() {
		Observer observer = Observer.GetInstance ();
		if (FB.IsInitialized) {
			FB.ActivateApp();
			if (observer.fbConnet!=null) {observer.fbConnet(true);}
			if (FB.IsLoggedIn) {
				if (observer.fbLogin!=null) {observer.fbLogin(true);}
			} else if (PlayerPrefs.GetInt("login", 0)==1) {
				LoginFacebook();
			}
		} else {
			if (observer.fbConnet!=null) {observer.fbConnet(false);}
		}
	}
	
	private void onlogincomplete(FBResult result) {
		Observer observer = Observer.GetInstance ();
		if (FB.IsLoggedIn) {
			PlayerPrefs.SetInt("login", 1);
			if (observer.fbLogin!=null) {observer.fbLogin(true);}
		} else {
			if (observer.fbLogin!=null) {observer.fbLogin(false);}
		}
	}
	
	private void onfeedcomplete(FBResult result) {
		Observer observer = Observer.GetInstance ();
		if (String.IsNullOrEmpty(result.Error)) {
			if (observer.fbFeed!=null) {observer.fbFeed(true);}
		} else {
			if (observer.fbFeed!=null) {observer.fbFeed(false);}
		}
	}
	
	private void onrequestcomplete(FBResult result) {
		Observer observer = Observer.GetInstance ();
		if (String.IsNullOrEmpty(result.Error)) {
			if (observer.fbRequest!=null) {observer.fbRequest(true);}
		} else {
			if (observer.fbRequest!=null) {observer.fbRequest(false);}
		}
	}

	private IEnumerator TakeScreenshot()
	{
		yield return new WaitForEndOfFrame();
		
		var width = Screen.width;
		var height = Screen.height;
		var tex = new Texture2D(width, height, TextureFormat.RGB24, false);
		tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
		tex.Apply();
		byte[] screenshot = tex.EncodeToPNG();
		
		var wwwForm = new WWWForm();
		wwwForm.AddBinaryData("image", screenshot, DateTime.Now.ToUniversalTime()+".png");
		wwwForm.AddField("caption", "herp derp.  I did a thing!  Did I do this right?");
		
		FB.API("/me/photos", Facebook.HttpMethod.POST, onsnapshotcomplete, wwwForm);
	}

	private void onsnapshotcomplete(FBResult result) {
		//do something
	}
#endif
}
