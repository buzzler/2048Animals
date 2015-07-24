using UnityEngine;
using System.Collections;

public class AnalyticsComponent : MonoBehaviour {
    private static AnalyticsComponent _this;
    public  GoogleAnalyticsV3 google; 

    public  const string CATEGORY_GAME  = "Game";
    public  const string CATEGORY_AD    = "Advertisement";
    public  const string CATEGORY_ITEM  = "Item";
    public  const string CATEGORY_THEME = "Theme";
    public  const string CATEGORY_LEVEL = "Level";
    public  const string CATEGORY_BOOT  = "Boot";
    public  const string ACTION_SCORE   = "Score";
    public  const string ACTION_COIN    = "Coin";
    public  const string ACTION_SHOW    = "Show";
    public  const string ACTION_SKIP    = "Skip";
    public  const string ACTION_FINISH  = "Finish";
    public  const string ACTION_FAIL    = "Fail";
    public  const string ACTION_USE     = "Use";
    public  const string ACTION_PURCHASE= "Purchase";
    public  const string ACTION_UNLOCK  = "Unlock";
    public  const string ACTION_CLEAR   = "Clear";
    public  const string ACTION_CONNECT = "Connect";
    public  const string ACTION_DISCONNECT="Disconnect";
    public  const string ACTION_SHARE   = "Share";
    public  const string EXCEPTION      = "Exception";
    public  const string SNS_FACEBOOK   = "Facebook";

    void Awake() {
        _this = this;
        google.StartSession();
    }

	void OnApplicationPause(bool pauseStatus) {
		if (pauseStatus) {
			google.StartSession ();
		} else {
			google.StopSession ();
		}
	}

	void OnApplicationQuit() {
		google.StopSession ();
	}

    public  static void LogScreen(UIType type) {
        _this.google.LogScreen(type.ToString());
    }

    public  static void LogGameEvent(string action, long value) {
        _this.google.LogEvent(CATEGORY_GAME, action, CATEGORY_GAME+" "+action, value);
    }

    public  static void LogAdEvent(string action) {
        _this.google.LogEvent(CATEGORY_AD, action, CATEGORY_AD+" "+action, (long)PlayerInfoKeeper.GetInstance().playerInfo.ads.Minute);
    }

    public  static void LogItemEvent(string action, long value) {
        _this.google.LogEvent(CATEGORY_ITEM, action, CATEGORY_ITEM+" "+action, value);
    }

    public  static void LogThemeEvent(string action, long value) {
        _this.google.LogEvent(CATEGORY_THEME, action, CATEGORY_THEME+" "+action, value);
    }

    public  static void LogLevelEvent(string action, long value) {
        _this.google.LogEvent(CATEGORY_LEVEL, action, CATEGORY_LEVEL+" "+action, value);
    }

    public  static void LogException(bool fatal) {
        _this.google.LogException(EXCEPTION, fatal);
    }

    public  static void LogBootTiming(long timingInterval) {
        _this.google.LogTiming(CATEGORY_BOOT, timingInterval, SystemInfo.deviceModel, SystemInfo.operatingSystem);
    }

    public  static void LogSocial(string action) {
        _this.google.LogSocial(SNS_FACEBOOK, action, FB.UserId);
    }

    public  static void LogTransaction(string transanctionId, double revenue, string currency) {
        _this.google.LogTransaction(transanctionId, StoreAssetInfo.COIN, revenue, 0, 0, currency);
    }
}
