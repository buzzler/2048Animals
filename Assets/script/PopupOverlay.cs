using UnityEngine;
using System.Collections;

public class PopupOverlay : OverlayComponent {
	public override void OnShow () {
		base.OnShow ();

		if (objectPanel != null) {
			objectPanel.transform.position = positionUp;
			Hashtable hash = new Hashtable ();
			hash.Add ("position", Vector3.zero);
			hash.Add ("time", timeShow);
			hash.Add ("delay", timeShowDelay);
			hash.Add ("easetype", iTween.EaseType.easeOutElastic);
			hash.Add ("oncomplete", "OnShowComplete");
			hash.Add ("oncompletetarget", gameObject);
			iTween.MoveTo (objectPanel, hash);
		}

		if (objectShadow != null) {
			objectShadow.color = colorHide;
			Hashtable hash = new Hashtable ();
			hash.Add ("from", colorHide);
			hash.Add ("to", colorShow);
			hash.Add ("time", timeShow);
			hash.Add ("delay", timeShowDelay);
			hash.Add ("easetype", iTween.EaseType.easeOutElastic);
			hash.Add ("onupdate", "OnShadowUpdate");
			hash.Add ("onupdatetarget", gameObject);
			iTween.ValueTo (objectShadow.gameObject, hash);
		}
	}

	public	void OnShadowUpdate(Color color) {
		objectShadow.color = color;
//		Debug.Log (color.ToString());
	}

	public override void OnHide (Vector3 direction) {
		base.OnHide (direction);

		if (objectPanel != null) {
			objectPanel.transform.position = Vector3.zero;
			Hashtable hash = new Hashtable ();
			hash.Add ("position", direction);
			hash.Add ("time", timeHide);
			hash.Add ("delay", timeHideDelay);
			hash.Add ("easetype", iTween.EaseType.easeInCubic);
			hash.Add ("oncomplete", "OnHideComplete");
			hash.Add ("oncompletetarget", gameObject);
			iTween.MoveTo (objectPanel, hash);
		}
		
		if (objectShadow != null) {
			objectShadow.color = colorShow;
			Hashtable hash = new Hashtable ();
			hash.Add ("from", colorShow);
			hash.Add ("to", colorHide);
			hash.Add ("time", timeHide);
			hash.Add ("delay", timeHideDelay);
			hash.Add ("easetype", iTween.EaseType.easeInCubic);
			hash.Add ("onupdate", "OnShadowUpdate");
			hash.Add ("onupdatetarget", gameObject);
			iTween.ValueTo (objectShadow.gameObject, hash);
		}
	}
}
