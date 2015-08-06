using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameStartOverlay : PopupOverlay {
	public	Image imageReady;
	public	Image imageGo;
	public	Vector3 fromScale;
	public	Vector3 toScale;
	
	void OnDisable() {
		CancelInvoke();
	}

	public	void Ready(OverlayEventHandler handler) {
		onClose += handler;

		OnShow ();
	}

	public override void OnShowComplete () {
		base.OnShowComplete ();

		imageReady.transform.position = positionUp;
		Hashtable hash = new Hashtable();
		hash.Add("position", Vector3.zero);
		hash.Add("time", 0.5f);
		hash.Add("easetype", iTween.EaseType.easeOutElastic);
		hash.Add("oncomplete", "OnReadyComplete");
		hash.Add("oncompletetarget", gameObject);
		iTween.MoveTo(imageReady.gameObject, hash);
	}

	public	void OnReadyComplete() {
		Invoke("OnGo", 0.5f);
	}

	public	void OnGo() {
		imageReady.gameObject.SetActive(false);

		imageGo.transform.localScale = fromScale;
		imageGo.transform.localPosition = Vector3.zero;
		Hashtable hash = new Hashtable();
		hash.Add("scale", toScale);
		hash.Add("time", 0.25f);
		hash.Add("easetype", iTween.EaseType.easeOutElastic);
		hash.Add("oncomplete", "OnGoComplete");
		hash.Add("oncompletetarget", gameObject);
		iTween.ScaleTo(imageGo.gameObject, hash);
	}

	public	void OnGoComplete() {
		Invoke("OnClose", 0.3f);
	}

	public override	void OnHide(Vector3 direction) {
		base.OnHide (direction);

		Hashtable hash = new Hashtable();
		hash.Add("position", direction);
		hash.Add("time", 0.2f);
		hash.Add("easetype", iTween.EaseType.easeInCubic);
		hash.Add("oncomplete", "OnHideComplete");
		hash.Add("oncompletetarget", gameObject);
		iTween.MoveTo(imageGo.gameObject, hash);
	}
}
