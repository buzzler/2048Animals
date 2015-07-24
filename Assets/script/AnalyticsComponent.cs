using UnityEngine;
using System.Collections;

public class AnalyticsComponent : MonoBehaviour {
    public  GoogleAnalyticsV3 google; 

    void Awake() {
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
}
