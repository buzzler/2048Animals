using UnityEngine;
using System.Collections;

public class SystemCheckComponent : MonoBehaviour {

	void Start() {
//		CheckWWW();
		StartCoroutine("CheckWWW");
	}

	public	void CheckNetwork() {
		switch (Application.internetReachability) {
		case NetworkReachability.ReachableViaCarrierDataNetwork:
			break;
		case NetworkReachability.ReachableViaLocalAreaNetwork:
			break;
		default:
			break;
		}
	}

	IEnumerator CheckWWW() {
		WWW w = new WWW("http://www.facebook.com");
		yield return w;
		if (!string.IsNullOrEmpty(w.error)) {
			Debug.Log(w.error);
		} else {
			Debug.Log("SUCCESSS!!!!!!!!!!!!!!");
		}
	}
}
